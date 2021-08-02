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
        //발사
        Fire();
        //리로드
        Reload();
    }

    void Fire()
    {
        if (curFireDelay < maxFireDelay)
            return;

        if (enemyLevel == 1)
        {
            // 프리팹 복제생성
            GameObject bullet = objectManager.MakeObject("EnemyBulletA");
            bullet.transform.position = transform.position;
            // 리지드바디 할당
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 fireDir = player.transform.position - transform.position;
            // 발사
            rigid.AddForce(fireDir.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyLevel == 3)
        {
            // 프리팹 복제생성
            GameObject bullet = objectManager.MakeObject("EnemyBulletB");
            bullet.transform.position = transform.position;
            // 리지드바디 할당
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 fireDir = player.transform.position - transform.position;
            // 발사
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
            // 아이템 드랍
            int random = Random.Range(0, 10);
            if(random < 3)
            {
                int index = Random.Range(0, 3);
                GameObject item = objectManager.MakeObject(items[index]);
                item.transform.position = transform.position;
            }
            gameObject.SetActive(false);
            // 회전값 초기화
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
            // 회전값 초기화
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
