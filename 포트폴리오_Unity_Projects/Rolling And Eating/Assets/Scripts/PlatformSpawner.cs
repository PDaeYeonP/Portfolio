using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab; // 복사할 플랫폼 프리팹
    GameObject[] platforms; // 미리 생성해둘 플랫폼 배열
    int platformCount; // 3개 만들 예정
    int currentIndex; // 현재 인덱스 값
    float spawnTimeMin; // 스폰시간 최소값
    float spawnTimeMax; // 스폰시간 최대값
    float spawnTimeGap; // 스폰 간격
    float lastSpawnTime; // 마지막 스폰 시간
    float createMin; // 생성치 최소값
    float createMax; // 생성치 최대값
    float[] createZ; // z포지션 생성치 배열
    Vector3 poolPos; // 플랫폼 임시 공간.

    void Awake()
    {
        // 필드 초기화
        platformCount = 3;
        currentIndex = 0;
        spawnTimeMin = 1f;
        spawnTimeMax = 3f;
        spawnTimeGap = 0;
        lastSpawnTime = 0;
        createMin = -3;
        createMax = 3;
        createZ = new float[3];
        poolPos = new Vector3(0, -10, 0);
        platforms = new GameObject[platformCount];
        // 프리팹 복사
        for(int i = 0; i < platformCount; i++)
        {
            platforms[i] = Instantiate(platformPrefab, poolPos, Quaternion.identity);
            createZ[i] = 0f;
        }
    }

    void Update()
    {
        // 첫 UI화면이거나 플랫폼이 게임 진행 중 모두 스폰상태일때는 리턴
        if (!UIManager.getInstance.powerOn || platforms[currentIndex].GetComponent<Platform>().isSpawn)
            return;
        // 그게 아니라면 마지막 스폰시간에 스폰간격보다 시간초과시 새로운 포지션 할당
        if (Time.time >= lastSpawnTime + spawnTimeGap)
        {
            // 시간 갱신
            lastSpawnTime = Time.time;
            // 스폰간격 랜덤 할당
            spawnTimeGap = Random.Range(spawnTimeMin, spawnTimeMax);
            // x, y 포지션 랜덤 할당
            float xPos = Random.Range(createMin, createMax);
            float yPos = Random.Range(createMin, createMax);
            // 오류방지 비활성화 후 즉시 활성화
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);
            // 플랫폼 순서 인덱스값으로 정리
            int prevIndex = currentIndex - 1;
            if (prevIndex < 0)
                prevIndex = 2;
            // z포지션을 앞의 플랫폼에 따라 할당
            createZ[currentIndex] = platforms[prevIndex].transform.position.z + 10;
            // 플랫폼 위치 선정
            platforms[currentIndex].transform.position = new Vector3(xPos, yPos, createZ[currentIndex++]);
            
            if (currentIndex >= platformCount)
                currentIndex = 0;
        }
    }
}
