using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // 프리펩
    public GameObject enemyLv1;
    public GameObject enemyLv2;
    public GameObject enemyLv3;

    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBoom;

    public GameObject playerBulletA;
    public GameObject playerBulletB;
    public GameObject enemyBulletA;
    public GameObject enemyBulletB;

    // 오브젝트 풀링
    GameObject[] enemiesLv1;
    GameObject[] enemiesLv2;
    GameObject[] enemiesLv3;

    GameObject[] itemsCoin;
    GameObject[] itemsPower;
    GameObject[] itemsBoom;

    GameObject[] playerBulletsA;
    GameObject[] playerBulletsB;
    GameObject[] enemyBulletsA;
    GameObject[] enemyBulletsB;

    GameObject[] targetPool;

    void Awake()
    {
        enemiesLv1 = new GameObject[30];
        enemiesLv2 = new GameObject[30];
        enemiesLv3 = new GameObject[30];

        itemsCoin = new GameObject[10];
        itemsPower = new GameObject[10];
        itemsBoom = new GameObject[10];

        playerBulletsA = new GameObject[100];
        playerBulletsB = new GameObject[50];
        enemyBulletsA = new GameObject[50];
        enemyBulletsB = new GameObject[50];

        Generate();
    }

    void Generate()
    {
        // enemy
        for(int index = 0; index < enemiesLv1.Length; index++)
        {
            enemiesLv1[index] =  Instantiate(enemyLv1);
            enemiesLv1[index].SetActive(false);
        }
        for(int index = 0; index < enemiesLv2.Length; index++)
        {
            enemiesLv2[index] = Instantiate(enemyLv2);
            enemiesLv2[index].SetActive(false);
        }
        for(int index = 0; index < enemiesLv3.Length; index++)
        {
            enemiesLv3[index] = Instantiate(enemyLv3);
            enemiesLv3[index].SetActive(false);
        }
        // item
        for(int index = 0; index < itemsCoin.Length; index++)
        {
            itemsCoin[index] = Instantiate(itemCoin);
            itemsCoin[index].SetActive(false);
        }
        for(int index = 0; index < itemsPower.Length; index++)
        {
            itemsPower[index] = Instantiate(itemPower);
            itemsPower[index].SetActive(false); 
        }
        for(int index = 0; index < itemsBoom.Length; index++)
        {
            itemsBoom[index] = Instantiate(itemBoom);
            itemsBoom[index].SetActive(false);
        }
        // bullet
        for(int index = 0; index < playerBulletsA.Length; index++)
        {
            playerBulletsA[index] = Instantiate(playerBulletA);
            playerBulletsA[index].SetActive(false);
        }
        for (int index = 0; index < playerBulletsB.Length; index++)
        {
            playerBulletsB[index] = Instantiate(playerBulletB);
            playerBulletsB[index].SetActive(false);
        }
        for (int index = 0; index < enemyBulletsA.Length; index++)
        {
            enemyBulletsA[index] = Instantiate(enemyBulletA);
            enemyBulletsA[index].SetActive(false);  
        }
        for (int index = 0; index < enemyBulletsB.Length; index++)
        {
            enemyBulletsB[index] = Instantiate(enemyBulletB);
            enemyBulletsB[index].SetActive(false);
        }
    }

    public GameObject MakeObject(string type)
    {
        switch(type)
        {
            case "EnemyLv1":
                targetPool = enemiesLv1;
                break;
            case "EnemyLv2":
                targetPool = enemiesLv2;
                break;
            case "EnemyLv3":
                targetPool = enemiesLv3;
                break;
            case "ItemCoin":
                targetPool = itemsCoin;
                break;
            case "ItemPower":
                targetPool = itemsPower;
                break;
            case "ItemBoom":
                targetPool = itemsBoom;
                break;
            case "PlayerBulletA":
                targetPool = playerBulletsA;
                break;
            case "PlayerBulletB":
                targetPool = playerBulletsB;
                break;
            case "EnemyBulletA":
                targetPool = enemyBulletsA;
                break;
            case "EnemyBulletB":
                targetPool = enemyBulletsB;
                break;
        }

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "EnemyLv1":
                targetPool = enemiesLv1;
                break;
            case "EnemyLv2":
                targetPool = enemiesLv2;
                break;
            case "EnemyLv3":
                targetPool = enemiesLv3;
                break;
            case "ItemCoin":
                targetPool = itemsCoin;
                break;
            case "ItemPower":
                targetPool = itemsPower;
                break;
            case "ItemBoom":
                targetPool = itemsBoom;
                break;
            case "PlayerBulletA":
                targetPool = playerBulletsA;
                break;
            case "PlayerBulletB":
                targetPool = playerBulletsB;
                break;
            case "EnemyBulletA":
                targetPool = enemyBulletsA;
                break;
            case "EnemyBulletB":
                targetPool = enemyBulletsB;
                break;
        }

        return targetPool;
    }
}
