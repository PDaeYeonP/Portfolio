using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI
using UnityEngine.SceneManagement; // Scene매니저
using System.IO; // 파일 관리

public class GameManager : MonoBehaviour
{
    string[] enemies;
    public Transform[] spawnPoints;
    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeIcon;
    public Image[] boomIcon;
    public GameObject gameOverSet;
    public ObjectManager objectManager;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool isSpawnEnd;

    void Awake()
    {
        spawnList = new List<Spawn>();
        enemies = new string[] { "EnemyLv1", "EnemyLv2", "EnemyLv3" };
        ReadSpawnFile();
    }

    void ReadSpawnFile()
    {
        // 변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        isSpawnEnd = false;
        // 파일 읽기
        TextAsset textFile = Resources.Load("stage 0") as TextAsset; // 텍스트에셋인지 확인하기위한 as 키워드 거짓이면 null값
        // 파일 열기
        StringReader stringReader = new StringReader(textFile.text);

        while(stringReader != null)
        {
            string line = stringReader.ReadLine();

            if (line == null)
            {
                Debug.Log(spawnList.Count);
                break;
            }

            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.level = int.Parse(line.Split(',')[1]);
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }
        // 파일 닫기
        stringReader.Close();
        //첫번째 리스폰 시간
        nextSpawnDelay = spawnList[0].delay;
    }

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > nextSpawnDelay && !isSpawnEnd)
        {
            // 스폰
            SpawnEnemy();
            curSpawnDelay = 0;
        }

        // UI 점수
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = "SCORE = " + string.Format("{0:n0}", playerLogic.score);
    }

    void SpawnEnemy()
    {
        // 복제 생성
        // 기존 프리펩 생성문 >> GameObject enemy = Instantiate(enemys[randomEnemy], spawnPoints[randomPoint].position, spawnPoints[randomPoint].rotation);
        // 오브젝트풀링으로 프리펩 생성문 49, 50줄
        GameObject enemy = objectManager.MakeObject(enemies[spawnList[spawnIndex].level - 1]);
        enemy.transform.position = spawnPoints[spawnList[spawnIndex].point].position;
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        if (spawnList[spawnIndex].point >= 5 && spawnList[spawnIndex].point <= 7)
        {
            enemy.transform.Rotate(Vector3.back * 45);
            rigid.velocity = new Vector3(enemyLogic.speed * (-1), -1, 0);
        }
        else if (spawnList[spawnIndex].point >= 8 && spawnList[spawnIndex].point <= 11)
        {
            enemy.transform.Rotate(Vector3.forward * 45);
            rigid.velocity = new Vector3(enemyLogic.speed, -1, 0);
        }
        else
            rigid.velocity = new Vector3(0, enemyLogic.speed * (-1), 0);

        spawnIndex++;
        if(spawnIndex == spawnList.Count)
        {
            isSpawnEnd = true;
            return;
        }

        nextSpawnDelay = spawnList[spawnIndex].delay;
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
