using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity {
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더

    public AudioClip deathClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리
    public AudioClip itemPickupClip; // 아이템 습득 소리

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트

    private void Awake() {
        // 사용할 컴포넌트를 가져오기
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable() {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        // 체력바 활성화
        healthSlider.gameObject.SetActive(true);
        // 체력바 최댓값을 기본 체력값으로 설정
        healthSlider.maxValue = startingHealth;
        // 체력바 값을 현재 체력값으로
        healthSlider.value = health;

        // 플레이어 조작을 받는 컴포넌트 활성화
        playerMovement.enabled = true;
        playerAudioPlayer.enabled = true;
    }

    // 체력 회복
    public override void RestoreHealth(float newHealth) {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);
        // 체력바 갱신
        healthSlider.value = health;
    }

    // 데미지 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection) {
        if(!dead)
        {
            //사망하지 않은 경우에만 사운드
            playerAudioPlayer.PlayOneShot(hitClip);
        }
        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage, hitPoint, hitDirection);
        // 체력바 갱신
        healthSlider.value = health;
    }

    // 사망 처리
    public override void Die() {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();
        // 체력바 비활성화
        healthSlider.gameObject.SetActive(false);
        // 사망 사운드
        playerAudioPlayer.PlayOneShot(deathClip);
        // 애니메이터 Die 트리거 발동
        playerAnimator.SetTrigger("Die");
        // 플레이어 조작을 받는 컴포넌트 비활성화
        //playerAnimator.enabled = false;
        //playerAudioPlayer.enabled = false; // 참고. 코드 처리시 사망효과음 안들림
    }

    private void OnTriggerEnter(Collider other) {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        if(!dead)
        {
            // 충돌 대상의 IItem 컴포넌트 가져오기
            IItem item = other.GetComponent<IItem>();
            // 성공이면
            if(item != null)
            {
                // Use 메서드 실행
                item.Use(gameObject);
                // 아이템 습득 사운드
                playerAudioPlayer.PlayOneShot(itemPickupClip);
            }
        }
    }
}