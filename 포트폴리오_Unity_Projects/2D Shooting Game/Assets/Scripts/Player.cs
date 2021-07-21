using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // private 변경 예정 필드
    public GameObject bulletA;
    public GameObject bulletB;
    public GameObject boomEffect;
    public float speed;
    int power;
    int maxPower;
    int boom;
    int maxBoom;
    bool isBoom;
    public GameManager manager;
    public int life;
    public int score;

    // private 필드
    Animator anim;
    float curFireDelay;
    float maxFireDelay;
    bool isTouchTop;
    bool isTouchBottom;
    bool isTouchLeft;
    bool isTouchRight;

    void Awake()
    {
        anim = GetComponent<Animator>();
        curFireDelay = 0.0f;
        maxFireDelay = 0.15f;
        power = 1;
        maxPower = 3;
        boom = 0;
        maxBoom = 2;
        isBoom = false;
    }

    void Update()
    {
        //이동
        Move();
        //발사
        Fire();
        // 붐
        Boom();
        //리로드
        Reload();
    }

    void Move()
    {
        float h, v;
        // 수직 수평 값
        h = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))
            h = 0;
        v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
            v = 0;
        // 현재 포지션과 다음 포지션
        Vector3 curPos = transform.position;
        // 트랜스폼을 이용한 이동은 항상 delta타임을 이용해줄 것.
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;
        // 이동 계산
        transform.position = curPos + nextPos;

        // 애니메이션 설정
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int)h);
        }
    }

    void Boom()
    {
        if (!Input.GetButton("Fire2"))
            return;

        if (isBoom)
            return;

        if (boom == 0)
            return;

        boom--;
        manager.UpdateBoomIcon(boom);
        isBoom = true;
        // 이펙트 켜기
        boomEffect.SetActive(true);
        Invoke("offBoomEffect", 3);
        // 씬에 존재하는 에너미와 탄알 제거
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int index = 0; index < enemies.Length; index++)
        {
            Enemy enemyLogic = enemies[index].GetComponent<Enemy>();
            enemyLogic.OnHit(1000);
        }
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int index = 0; index < bullets.Length; index++)
        {
            Destroy(bullets[index]);
        }
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        if (curFireDelay < maxFireDelay)
            return;

        GameObject bulletC;
        GameObject bulletR;
        GameObject bulletL;
        Rigidbody2D rigidC;
        Rigidbody2D rigidR;
        Rigidbody2D rigidL;

        switch (power)
        {
            case 1:
                // 프리팹 복제생성
                bulletC = Instantiate(bulletA, transform.position, transform.rotation);
                // 리지드바디 할당
                rigidC = bulletC.GetComponent<Rigidbody2D>();
                // 발사
                rigidC.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                // 좌우로 두발
                bulletR = Instantiate(bulletA, transform.position + Vector3.right * 0.1f, transform.rotation);
                bulletL = Instantiate(bulletA, transform.position + Vector3.left * 0.1f, transform.rotation);
                rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
                // 중간 강화
                bulletC = Instantiate(bulletB, transform.position, transform.rotation);
                bulletR = Instantiate(bulletA, transform.position + Vector3.right * 0.35f, transform.rotation);
                bulletL = Instantiate(bulletA, transform.position + Vector3.left * 0.35f, transform.rotation);
                rigidC = bulletC.GetComponent<Rigidbody2D>();
                rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidC.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
                rigidR.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
                break;
        }
        curFireDelay = 0;
    }

    void Reload()
    {
        curFireDelay += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // BorderLine에 플레이어가 닿으면 플래그 상태 갱신
        if(collision.gameObject.tag == "Border")
        {
            switch(collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
            }
        }
        else if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            life--;
            manager.UpdateLifeIcon(life);

            if (life == 0)
                manager.GameOver();
            else
                manager.RespawnPlayer();

            gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch(item.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if (power < maxPower)
                        power++;
                    else
                        score += 300;
                    break;
                case "Boom":
                    if (boom < maxBoom)
                    {
                        boom++;
                        manager.UpdateBoomIcon(boom);
                    }
                    else
                        score += 500;
                    break;
            }
            // 먹은 아이템은 삭제
            Destroy(collision.gameObject);
        }
    }

    void offBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoom = false;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
            }
        }
    }
}
