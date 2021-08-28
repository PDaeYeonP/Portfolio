using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour, IDamageable
{
    // 필드
    Rigidbody2D playerRigidbody; // 리지드바디
    SpriteRenderer spriteRenderer; // 스프라이트렌더러
    Animator playerAnimator; // 애니메이터
    AudioSource playerAudio; // 오디오소스
    // 오디오클립들
    public AudioClip jumpSound;
    public AudioClip pickupSound;
    public AudioClip attackSound;
    public AudioClip damageSound;
    public AudioClip finishSound;
    public AudioClip dieSound;
    int maxSpeed; // 최대속도
    bool disableKey; // 데미지 입을때 잠시 키 무효화 변수
    bool damaged; // 데미지를 입고있는지 표시
    public int life { get; private set; } // 생명력


    void Awake()
    {
        // 초기화
        playerRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        maxSpeed = 8;
        disableKey = false;
        damaged = false;
        life = 3;
    }

    void Update()
    {
        // 점프 로직
        if (!disableKey && !playerAnimator.GetBool("isJump") && Input.GetButtonDown("Jump"))
        {
            //사운드
            playerAudio.PlayOneShot(jumpSound);
            // 애니메이터 파라미터 갱신
            playerAnimator.SetBool("isJump", true);
            // 리지드바디 y축 속도 초기화 (현재 행동이 점프에 방해하지 않기 위해)
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
            // addforce
            playerRigidbody.AddForce(Vector2.up * 18f, ForceMode2D.Impulse);
        }
        // 스프라이트 방향 대응
        if (!disableKey && Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        // 애니메이터 파라미터 갱신
        if (playerRigidbody.velocity.x == 0)
            playerAnimator.SetBool("isRun", false);
        else
            playerAnimator.SetBool("isRun", true);
    }

    void FixedUpdate()
    {
        // 방향키 입력
        float xInput = Input.GetAxisRaw("Horizontal");
        // 이동 로직
        if (!disableKey && xInput != 0)
        {
            // addforce
            playerRigidbody.AddForce(new Vector2(xInput, 0), ForceMode2D.Impulse);
            // 절대값으로 좌, 우 방향 최대속도 제한
            if (Mathf.Abs(playerRigidbody.velocity.x) >= maxSpeed)
                playerRigidbody.velocity = new Vector2(maxSpeed * xInput, playerRigidbody.velocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 깃발에 도달하면 다음 스테이지
        if (collision.tag == "Finish")
        {
            // 사운드
            playerAudio.PlayOneShot(finishSound);
            // 다음스테이지 호출
            GameManager.instance.NextStage();
        }
    }

    public void PickupSound()
    {
        // 아이템 픽업 사운드
        playerAudio.PlayOneShot(pickupSound);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 45도 이상 경사에서 점프상태 갱신
        if (collision.contacts[0].normal.y >= 0.5f)
            playerAnimator.SetBool("isJump", false);
        // 점프에 따른 어택 or 데미지
        if (!damaged && collision.gameObject.tag == "Enemy")
        {
            // enemy레이어 and 플레이어의 y축이 위에 있다면 어택
            if (collision.gameObject.layer == 7 && playerRigidbody.velocity.y < 0 && transform.position.y > collision.transform.position.y)
                OnAttack(collision.transform);
            // 아니면 대미지
            else
                OnDamage(collision.transform.position);
        }
    }

    public void RePosition()
    {
        //현재행동에 대한 속도 초기화
        playerRigidbody.velocity = Vector2.zero;
        //위치 재설정
        transform.position = new Vector3(0, 0, 0);
    }

    void OnAttack(Transform target)
    {
        // 사운드
        playerAudio.PlayOneShot(attackSound);
        // 점수 획득
        GameManager.instance.AddPoint(200);
        // addforce (반발력)
        playerRigidbody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // 인터페이스 호출
        IDamageable enemy = target.GetComponent<IDamageable>();
        enemy.OnDamage(target.position);
    }

    public void OnDamage(Vector2 targetPos)
    {
        // 사운드
        playerAudio.PlayOneShot(damageSound);
        // bool 갱신
        disableKey = true;
        damaged = true;
        // 충돌을 막기 위해 잠시 레이어 변경
        gameObject.layer = 8;
        // 생명력 다운
        LifeDown();
        // 반투명화
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // 충돌 반대방향으로 튕기는 로직
        int direction = transform.position.x - targetPos.x > 0 ? 1 : -1;
        playerRigidbody.AddForce(new Vector2(direction * 10f, 1), ForceMode2D.Impulse);
        // 애니메이터 파라미터 갱신
        playerAnimator.SetTrigger("Damage");
        // 인보크 (키 무효화, 대미지 상태 off)
        Invoke("EnableKey", 1.5f);
        Invoke("OffDamage", 3);
    }

    public void LifeDown()
    {
        // 생명력 다운
        life--;
        // UI 출력
        GameManager.instance.LifeDownUI();
        // die 확인
        Die();
    }

    void Die()
    {
        // 생명력이 없다면 die
        if(life <= 0)
        {
            playerAudio.PlayOneShot(dieSound);
            GameManager.instance.EndGame();
        }
    }

    void EnableKey()
    {
        // 무효화 된 키 입력 복구
        disableKey = false;
    }

    void OffDamage()
    {
        // 대미지 상태 복구
        damaged = false;
        // 레이어 복구
        gameObject.layer = 6;
        // 반투명화 복구
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}

// 메모
//public event Action onDeath; // 액션은 델리게이트(대리자), 이벤트는 외부 사용 금지