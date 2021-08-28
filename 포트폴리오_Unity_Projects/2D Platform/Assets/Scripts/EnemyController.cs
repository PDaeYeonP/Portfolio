using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    //필드
    Rigidbody2D enemyRigidbody; // 리지드바디
    CapsuleCollider2D enemyCollider; // 콜라이더
    Animator enemyAnimator; // 애니메이터
    SpriteRenderer spriteRenderer; // 스프라이트렌더러
    int nextMove; // 다음행동

    void Awake()
    {
        //초기화
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
        //다음행동 함수 호출
        Invoke("Think", 3);
    }

    void FixedUpdate()
    {
        // 이동
        enemyRigidbody.velocity = new Vector2(nextMove, enemyRigidbody.velocity.y);
        // 전방주시를 위한 벡터 설정
        Vector2 front = new Vector2(enemyRigidbody.position.x + nextMove * 0.3f, enemyRigidbody.position.y);
        // 전방주시를 위한 레이캐스트        
        RaycastHit2D rayHit = Physics2D.Raycast(front, Vector3.down, 1, LayerMask.GetMask("Platform"));
        // 앞에 필드가 없다면 방향 전환
        if (rayHit.collider == null)
        {
            // 반대방향
            nextMove *= -1;
            spriteRenderer.flipX = nextMove == 1;
            // 인보크 제거 후 다시 설정
            CancelInvoke();
            Invoke("Think", 3);
        }
    }

    void Think() // 다음 행동을 하기위한 함수
    { 
        // 다음 행동과 생각 랜덤 설정
        nextMove = Random.Range(-1, 2);
        float nextThink = Random.Range(1f, 6f);
        // 인보크
        Invoke("Think", nextThink);
        // 애니메이터 파라미터 셋
        enemyAnimator.SetInteger("isRun", nextMove);
        // 멈춘게 아니라면 스프라이트 반전
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;
    }

    public void OnDamage(Vector2 targetPos)
    {
        // 반투명
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // 스프라이트 반전
        spriteRenderer.flipY = true;
        // 콜라이더 제거
        enemyCollider.enabled = false;
        // addforce
        enemyRigidbody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // 2초 후 제거
        Destroy(gameObject, 2);
    }
}
