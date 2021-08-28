using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f; // 탄알 속도
    private Rigidbody bulletRigidbody; // 리지드바디

    void Start()
    {
        //리지드바디 가져오기
        bulletRigidbody = GetComponent<Rigidbody>();
        //리지드바디 속도 갱신
        bulletRigidbody.velocity = transform.forward * speed;
        // 3초뒤 게임오브젝트 삭제
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //충돌체가 플레이어라면
        if(other.tag == "Player")
        {
            //플레이어 변수를 받아와서 Die함수 실행
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController)
                playerController.Die();
        }
    }
}
