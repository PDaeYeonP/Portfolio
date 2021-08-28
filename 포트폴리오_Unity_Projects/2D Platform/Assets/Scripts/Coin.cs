using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 트리거가 플레이어라면 픽업
        if (collision.tag == "Player")
        {
            // 플레이어컨트롤 호출
            PlayerController player = collision.GetComponent<PlayerController>();
            // 사운드
            player.PickupSound();
            // 점수 획득 후 제거
            GetCoin();
        }
    }

    protected virtual void GetCoin()
    {
        // 제거
        Destroy(gameObject);
    }
}
