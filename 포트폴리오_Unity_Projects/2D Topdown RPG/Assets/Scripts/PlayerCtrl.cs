using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    Rigidbody2D rigid; // 리지드바디
    Animator anim; // 애니메이터
    Vector3 frontVec; // 플레이어 전방 벡터
    GameObject scanObject; // 스캔할 오브젝트
    float xInput; //x축 방향
    float zInput; //z축 방향
    int speed; // 스피드
    bool isHorizonMove; // 수평이동 bool 변수

    void Awake()
    {
        // 초기화
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        speed = 10;
    }

    void Update()
    {
        // 상호작용중이 아니면 이동 가능
        if(!GameManager.instance.isInteraction)
        {
            // 방향키 입력
            xInput = Input.GetAxisRaw("Horizontal");
            zInput = Input.GetAxisRaw("Vertical");
            // 수평이동 체크
            if (Input.GetButtonDown("Horizontal"))
                isHorizonMove = true;
            else if (Input.GetButtonDown("Vertical"))
                isHorizonMove = false;
            else if (Input.GetButtonUp("Horizontal") || Input.GetButtonUp("Vertical"))
                isHorizonMove = xInput != 0;
            // 전방벡터 체크
            if (Input.GetButtonDown("Vertical") && zInput < 0)
                frontVec = Vector3.down;
            else if (Input.GetButtonDown("Vertical") && zInput > 0)
                frontVec = Vector3.up;
            else if (Input.GetButtonDown("Horizontal") && xInput < 0)
                frontVec = Vector3.left;
            else if (Input.GetButtonDown("Horizontal") && xInput > 0)
                frontVec = Vector3.right;
        }
        // 이동키를 누르고 있는 순간의 애니메이션 설정 여러키를 눌렀을때 애니메이션의 대응
        if (anim.GetInteger("xAxisRaw") != (int)xInput)
        {
            anim.SetBool("keyChange", true);
            anim.SetInteger("xAxisRaw", (int)xInput);
        }
        else if (anim.GetInteger("zAxisRaw") != (int)zInput)
        {
            anim.SetBool("keyChange", true);
            anim.SetInteger("zAxisRaw", (int)zInput);
        }
        else
            anim.SetBool("keyChange", false);
        // 상호작용
        if (Input.GetKeyDown(KeyCode.Space) && scanObject != null)
            GameManager.instance.Interaction(scanObject);
    }

    void FixedUpdate()
    {
        // 대각선 이동 방지
        Vector2 moveVec = isHorizonMove ? new Vector2(xInput, 0) : new Vector2(0, zInput);
        // 결정된 방향 이동
        rigid.velocity = moveVec * speed;
        // 확인용 디버그
        Debug.DrawRay(rigid.position, frontVec * 0.7f, new Color(1, 0, 0));
        // 레이캐스트
        RaycastHit2D scanRay = Physics2D.Raycast(rigid.position, frontVec, 0.7f, LayerMask.GetMask("Object"));
        // 레이캐스트히트에 잡힌 콜라이더가 널값이 아니면 스캔오브젝트에 할당
        if (scanRay.collider != null)
            scanObject = scanRay.collider.gameObject;
        else
            scanObject = null;
    }

}
