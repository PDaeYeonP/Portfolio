using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    int score; // 해당 아이템 점수

    void OnEnable()
    {
        // 아이템을 1 ~ 100점 까지 랜덤 부여
        score = Random.Range(1, 100);
    }

    void Update()
    {
        // 아이템 오브젝트 회전
        transform.Rotate(Vector3.up * 50 * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        // 플레이어가 먹으면
        if(other.tag == "Player")
        {
            // 플레이어 오브젝트에 픽업 사운드 출력
            other.GetComponent<AudioSource>().Play();
            // 점수 추가
            GameManager.getInstance.AddScore(score);
            // 아이템 제거
            gameObject.SetActive(false);
        }
    }
}
//f