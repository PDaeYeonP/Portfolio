using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody playerRigidbody; // 플레이어 리지드바디
    float jumpForce, speed; // 점프 힘과, 스피드
    int jumpCount; // 최대 점프 수
    bool readyToJump; // 점프 준비 상태

    void Awake()
    {
        // 초기화
        playerRigidbody = GetComponent<Rigidbody>();
        jumpForce = 45f;
        speed = 30f;
        jumpCount = 0;
        readyToJump = false;
    }

    void FixedUpdate()
    {
        // UI 스타트 전이면 리턴
        if (!UIManager.getInstance.powerOn)
            return;
        // x, z 입력 값 받기
        float xInput = Input.GetAxisRaw("Horizontal") * speed;
        float zInput = Input.GetAxisRaw("Vertical") * speed;
        // 받은 입력값으로 오브젝트 힘 부여
        playerRigidbody.AddForce(new Vector3(xInput * Time.deltaTime, 0, zInput * Time.deltaTime), ForceMode.Impulse);
        //
    }

    void Update()
    {
        // UI 스타트 전이면 리턴
        if (!UIManager.getInstance.powerOn)
            return;

        // 점프키를 누르고 최대점프수가 아니라면
        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            // 점프 수 증가
            jumpCount++;
            // y포지션 속도 제로만들기 (점프에 물리적 방해를 막기위해)
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            // 점프 실행 힘주기
            playerRigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
        // 점프키를 떼면 y속도가 양수일때 점프 힘을 반감 -> 오래누르고 있을수록 높이 점프
        else if (Input.GetButtonUp("Jump") && playerRigidbody.velocity.y > 0)
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;
    }
    void OnCollisionEnter(Collision collision)
    {
        // 플랫폼 floor 오브젝트에 충돌했을때 점프상태 필드 초기화
        if(collision.gameObject.tag == "Floor")
        {
            readyToJump = true;
            jumpCount = 0;
        }
    }
}
