using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // private ���� ���� �ʵ�
    public GameObject bulletA;
    public GameObject bulletB;
    public GameObject boomEffect;
    public float speed;
    int power;
    int maxPower;
    int boom;
    int maxBoom;
    bool isBoom;
    public GameManager gameManager;
    public ObjectManager objectManager;
    public int life;
    public int score;

    // private �ʵ�
    Animator anim;
    float curFireDelay;
    float maxFireDelay;
    bool isTouchTop;
    bool isTouchBottom;
    bool isTouchLeft;
    bool isTouchRight;

    public GameObject[] followers;

    void Awake()
    {
        anim = GetComponent<Animator>();
        curFireDelay = 0.0f;
        maxFireDelay = 0.15f;
        power = 1;
        maxPower = 6;
        boom = 0;
        maxBoom = 2;
        isBoom = false;
    }

    void Update()
    {
        //�̵�
        Move();
        //�߻�
        Fire();
        // ��
        Boom();
        //���ε�
        Reload();
    }

    void Move()
    {
        float h, v;
        // ���� ���� ��
        h = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))
            h = 0;
        v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
            v = 0;
        // ���� �����ǰ� ���� ������
        Vector3 curPos = transform.position;
        // Ʈ�������� �̿��� �̵��� �׻� deltaŸ���� �̿����� ��.
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;
        // �̵� ���
        transform.position = curPos + nextPos;

        // �ִϸ��̼� ����
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
        gameManager.UpdateBoomIcon(boom);
        isBoom = true;
        // ����Ʈ �ѱ�
        boomEffect.SetActive(true);
        Invoke("offBoomEffect", 3);
        // ���� �����ϴ� ���ʹ̿� ź�� ����
        GameObject[] enemiesLv1 = objectManager.GetPool("EnemyLv1");
        GameObject[] enemiesLv2 = objectManager.GetPool("EnemyLv2");
        GameObject[] enemiesLv3 = objectManager.GetPool("EnemyLv3");

        for (int index = 0; index < enemiesLv1.Length; index++)
        {
            if(enemiesLv1[index].activeSelf)
            {
                Enemy enemyLogic = enemiesLv1[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        for (int index = 0; index < enemiesLv2.Length; index++)
        {
            if (enemiesLv2[index].activeSelf)
            {
                Enemy enemyLogic = enemiesLv2[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        for (int index = 0; index < enemiesLv3.Length; index++)
        {
            if (enemiesLv3[index].activeSelf)
            {
                Enemy enemyLogic = enemiesLv3[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        GameObject[] bulletsA = objectManager.GetPool("EnemyBulletA");
        GameObject[] bulletsB = objectManager.GetPool("EnemyBulletB");

        for (int index = 0; index < bulletsA.Length; index++)
        {
            if(bulletsA[index].activeSelf)
                bulletsA[index].SetActive(false);
        }
        for (int index = 0; index < bulletsB.Length; index++)
        {
            if (bulletsB[index].activeSelf)
                bulletsB[index].SetActive(false);
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
                // ������ ��������
                // bulletC = Instantiate(bulletA, transform.position, transform.rotation);
                bulletC = objectManager.MakeObject("PlayerBulletA");
                bulletC.transform.position = transform.position;
                // ������ٵ� �Ҵ�
                rigidC = bulletC.GetComponent<Rigidbody2D>();
                // �߻�
                rigidC.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                // �¿�� �ι�
                bulletR = objectManager.MakeObject("PlayerBulletA");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                bulletL = objectManager.MakeObject("PlayerBulletA");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;
                rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
                break;
            default:
                // �߰� ��ȭ
                bulletC = objectManager.MakeObject("PlayerBulletB");
                bulletC.transform.position = transform.position;
                bulletR = objectManager.MakeObject("PlayerBulletA");
                bulletR.transform.position = transform.position + Vector3.right * 0.3f;
                bulletL = objectManager.MakeObject("PlayerBulletA");
                bulletL.transform.position = transform.position + Vector3.left * 0.3f;
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
        // BorderLine�� �÷��̾ ������ �÷��� ���� ����
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
            gameManager.UpdateLifeIcon(life);

            if (life == 0)
                gameManager.GameOver();
            else
                gameManager.RespawnPlayer();

            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
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
                    {
                        power++;
                        AddFollower();
                    }
                    else
                        score += 300;
                    break;
                case "Boom":
                    if (boom < maxBoom)
                    {
                        boom++;
                        gameManager.UpdateBoomIcon(boom);
                    }
                    else
                        score += 500;
                    break;
            }
            // ���� �������� ����
            collision.gameObject.SetActive(false);
        }
    }

    void AddFollower()
    {
        if(power == 4)
            followers[0].SetActive(true);
        else if(power == 5)
            followers[1].SetActive(true);
        else if (power == 6)
            followers[2].SetActive(true);
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
