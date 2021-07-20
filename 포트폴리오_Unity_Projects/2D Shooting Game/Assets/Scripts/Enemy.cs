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
    public int enemyScore;

    SpriteRenderer spriteRenderer;

    public GameObject bulletA;
    public GameObject bulletB;
    float curFireDelay;
    float maxFireDelay;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        curFireDelay = 0.0f;
        maxFireDelay = 2.0f;
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
            GameObject bullet = Instantiate(bulletA, transform.position, transform.rotation);
            // 리지드바디 할당
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 fireDir = player.transform.position - transform.position;
            // 발사
            rigid.AddForce(fireDir.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyLevel == 3)
        {
            // 프리팹 복제생성
            GameObject bullet = Instantiate(bulletB, transform.position, transform.rotation);
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

    void OnHit(int damage)
    {
        health -= damage;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;
            Destroy(gameObject);
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
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            Destroy(collision.gameObject);
        }
    }
}
