using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    string[] enemies;
    public Transform[] spawnPoints;
    public float maxSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeIcon;
    public Image[] boomIcon;
    public GameObject gameOverSet;
    public ObjectManager objectManager;

    void Awake()
    {
        enemies = new string[] { "EnemyLv1", "EnemyLv2", "EnemyLv3" };
    }

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > maxSpawnDelay)
        {
            // 스폰
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }

        // UI 점수
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = "SCORE = " + string.Format("{0:n0}", playerLogic.score);
    }

    void SpawnEnemy()
    {
        int randomEnemy = Random.Range(0, 3);
        int randomPoint = Random.Range(0, 11);
        // 복제 생성
        // 기존 프리펩 생성문 >> GameObject enemy = Instantiate(enemys[randomEnemy], spawnPoints[randomPoint].position, spawnPoints[randomPoint].rotation);
        // 오브젝트풀링으로 프리펩 생성문 49, 50줄
        GameObject enemy = objectManager.MakeObject(enemies[randomEnemy]);
        enemy.transform.position = spawnPoints[randomPoint].position;
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        if (randomPoint >= 5 && randomPoint <= 7)
        {
            enemy.transform.Rotate(Vector3.back * 45);
            rigid.velocity = new Vector3(enemyLogic.speed * (-1), -1, 0);
        }
        else if (randomPoint >= 8 && randomPoint <= 11)
        {
            enemy.transform.Rotate(Vector3.forward * 45);
            rigid.velocity = new Vector3(enemyLogic.speed, -1, 0);
        }
        else
            rigid.velocity = new Vector3(0, enemyLogic.speed * (-1), 0);
    }

    public void UpdateLifeIcon(int life)
    {
        for (int index = 0; index < 3; index++)
        {
            lifeIcon[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < life; index++)
        {
            lifeIcon[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        for (int index = 0; index < 2; index++)
        {
            boomIcon[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < boom; index++)
        {
            boomIcon[index].color = new Color(1, 1, 1, 1);
        }
    }


    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
    }

    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 4.0f;
        player.SetActive(true);
    }
}
