using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody; // 리지드바디
    public float speed = 15f; // 속도

    void Start()
    {
        // 리지드바디 가져오기
        playerRigidbody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        // 키입력
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        // 속도 필드 설정
        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;
        // 벡터 생성
        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        // 새로운 리지드바디 속도 할당
        playerRigidbody.velocity = newVelocity;
    }

    public void Die()
    {
        // 본 게임 오브젝트 비활성화
        gameObject.SetActive(false);
        // 씬에 존재하는 매니저 호출
        GameManager gameManager = FindObjectOfType<GameManager>();
        // 호출된 매니저 EndGame() 실행
        gameManager.EndGame();
    }
}
