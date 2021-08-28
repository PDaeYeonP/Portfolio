using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject[] items; // 플랫폼당 부여된 아이템 배열
    public bool isSpawn; // 스폰 상태
    Floor floor; // 플랫폼 바닥

    void Awake()
    {
        // floor 불러오기
        floor = FindObjectOfType<Floor>();
    }

    void Start()
    {
        // 최초 스폰상태 false
        isSpawn = false;
    }

    void OnEnable()
    {
        // 활성화마다 스폰상태 true
        isSpawn = true;
        // 아이템 랜덤으로 활성화
        for(int i = 0; i < items.Length; i ++)
        {
            if (Random.Range(1, 3) == 1)
                items[i].SetActive(true);
            else
                items[i].SetActive(false);
        }
        // 사용되지 않는 플랫폼 제거 코루틴
        StartCoroutine(Unused());
    }

    void Update()
    {
        // 플랫폼의 floor의 isGone 상태가 true가 되면 2초뒤 제거해주는 코루틴 실행
        if (floor.isGone)
            StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        // 2초뒤 오브젝트 비활성화
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private IEnumerator Unused()
    {
        // 사용되지 않으면 6초뒤 비활성화
        yield return new WaitForSeconds(6f);
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        isSpawn = false;
    }
}
