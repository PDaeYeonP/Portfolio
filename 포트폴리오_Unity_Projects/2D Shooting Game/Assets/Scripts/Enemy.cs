using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyLevel;
    public float speed;
    public int health;
    public Sprite[] sprites;
    public GameObject player;
    public ObjectManager objectManager;
    public int enemyScore;

    SpriteRenderer spriteRenderer;

    public GameObject bulletA;
    public GameObject bulletB;
    //public GameObject[] items;
    float curFireDelay;
    float maxFireDelay;
    string[] items;


    void Awake()
    {
        items = new string[] { "ItemCoin", "ItemPower", "ItemBoom" };
        spriteRenderer = GetComponent<SpriteRenderer>();
        curFireDelay = 0.0f;
        maxFireDelay = 2.0f;
    }

    void OnEnable()
    {
        switch(enemyLevel)
        {
            case 1:
                health = 10;
                break;
            case 2:
                health = 15;
                break;
            case 3:
                health = 30;
                break;
        }
    }

    void Update()
    {
        //�߻�
        Fire();
        //���ε�
        Reload();
    }

    void Fire()
    {
        if (curFireDelay < maxFireDelay)
            return;

        if (enemyLevel == 1)
        {
            // ������ ��������
            GameObject bullet = objectManager.MakeObject("EnemyBulletA");
            bullet.transform.position = transform.position;
            // ������ٵ� �Ҵ�
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 fireDir = player.transform.position - transform.position;
            // �߻�
            rigid.AddForce(fireDir.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyLevel == 3)
        {
            // ������ ��������
            GameObject bullet = objectManager.MakeObject("EnemyBulletB");
            bullet.transform.position = transform.position;
            // ������ٵ� �Ҵ�
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 fireDir = player.transform.position - transform.position;
            // �߻�
            rigid.AddForce(fireDir.normalized * 3, ForceMode2D.Impulse);
        }

        curFireDelay = 0;
    }

    void Reload()
    {
        curFireDelay += Time.deltaTime;
    }

    public void OnHit(int damage)
    {
        health -= damage;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;
            // ������ ���
            int random = Random.Range(0, 10);
            if(random < 3)
            {
                int index = Random.Range(0, 3);
                GameObject item = objectManager.MakeObject(items[index]);
                item.transform.position = transform.position;
            }
            gameObject.SetActive(false);
            // ȸ���� �ʱ�ȭ
            transform.rotation = Quaternion.identity;
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder")
        {
            gameObject.SetActive(false);
            // ȸ���� �ʱ�ȭ
            transform.rotation = Quaternion.identity;
        }
        else if(collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            collision.gameObject.SetActive(false);
        }
    }
}
