using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab; // 원본 프리팹
    public float spawnRateMin = 0.5f; // 스폰 주기 최소값
    public float spawnRateMax = 3f; // 스폰 주기 최대값

    private Transform target; // 불릿 타겟 오브젝트의 트랜스폼
    private float spawnRate; // 생성 주기
    private float timeAfterSpawn; // 스폰 후 시간 (갱신용)

    void Start()
    { // 최초 갱신시간 0초
        timeAfterSpawn = 0f;
        // 생성 주기 Min, Max사이 할당
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        // 트랜스폼 컴포넌트 변수에 PlayerController컴포넌트를 가진 게임 오브젝트의 트랜스폼 컴포넌트를 할당
        target = FindObjectOfType<PlayerController>().transform; 
    }

    void Update()
    {
        // 생성주기 갱신
        timeAfterSpawn += Time.deltaTime;
        // 생성주기 조건 확인 
        if(timeAfterSpawn >= spawnRate)
        { // 누적시간 초기화
            timeAfterSpawn = 0f;
            // 불릿프리팹을 위치와 회전으로 복제본 생성
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            // 복제본이 타겟을 향하게 설정
            bullet.transform.LookAt(target);
            // 생성 주기 재설정
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }
}
