using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
   public AudioClip deathClip; // 사망시 재생할 오디오 클립
   public float jumpForce = 700f; // 점프 힘

   private int jumpCount = 0; // 누적 점프 횟수
   private bool isGrounded = false; // 바닥에 닿았는지 나타냄
   private bool isDead = false; // 사망 상태

   private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
   private Animator animator; // 사용할 애니메이터 컴포넌트
   private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트

   private void Start() 
   {
        // 초기화
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
   }

   private void Update() 
   {
        // 사용자 입력을 감지하고 점프하는 처리
        if (isDead)
            return;
        
        if(Input.GetKeyDown(KeyCode.Space) && jumpCount < 2) // 최대점프 이하에서 마우스 클릭시
        { // 점프 개수 증가
            jumpCount++;
            // 점프시 순간 속도 제로 > 다음 점프의 물리적인 방해 예방을 위해
            playerRigidbody.velocity = Vector2.zero; 
            playerRigidbody.AddForce(new Vector2(0, jumpForce)); //점프 힘 적용
            playerAudio.Play(); // 사운드
        }
        else if(Input.GetKeyUp(KeyCode.Space) && playerRigidbody.velocity.y > 0) // 마우스를 떼는 순간 y값이 양수라면 현재속도 절반
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;

        // 애니메이터의 파라미터를 isGrounded 값으로 갱신
        animator.SetBool("Grounded", isGrounded);
   }

   private void Die() 
   {
        // 사망 처리
        animator.SetTrigger("Die");
        // 오디오 클립 변경
        playerAudio.clip = deathClip;
        // 사운드
        playerAudio.Play();
        // 속도 제로
        playerRigidbody.velocity = Vector2.zero;
        // 사망
        isDead = true;
        // 게임매니저 게임오버 처리
        GameManager.instance.OnPlayerDead();
   }

   private void OnTriggerEnter2D(Collider2D other) 
   {
       // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
       if(other.tag == "Dead" && !isDead)
            Die();
    }

    private void OnCollisionEnter2D(Collision2D collision) 
   {
       // 바닥에 닿았음을 감지하는 처리
       if(collision.contacts[0].normal.y > 0.7f)
        { // 어떤 콜라이더와 닿고, 표면의 위쪽이라면 상태 전환
            isGrounded = true;
            jumpCount = 0;
        }
   }

   private void OnCollisionExit2D(Collision2D collision) 
   {
        // 바닥에서 벗어났음을 감지하는 처리
        isGrounded = false;
   }
}