using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    //�ʵ�
    Rigidbody2D enemyRigidbody; // ������ٵ�
    CapsuleCollider2D enemyCollider; // �ݶ��̴�
    Animator enemyAnimator; // �ִϸ�����
    SpriteRenderer spriteRenderer; // ��������Ʈ������
    int nextMove; // �����ൿ

    void Awake()
    {
        //�ʱ�ȭ
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
        //�����ൿ �Լ� ȣ��
        Invoke("Think", 3);
    }

    void FixedUpdate()
    {
        // �̵�
        enemyRigidbody.velocity = new Vector2(nextMove, enemyRigidbody.velocity.y);
        // �����ֽø� ���� ���� ����
        Vector2 front = new Vector2(enemyRigidbody.position.x + nextMove * 0.3f, enemyRigidbody.position.y);
        // �����ֽø� ���� ����ĳ��Ʈ        
        RaycastHit2D rayHit = Physics2D.Raycast(front, Vector3.down, 1, LayerMask.GetMask("Platform"));
        // �տ� �ʵ尡 ���ٸ� ���� ��ȯ
        if (rayHit.collider == null)
        {
            // �ݴ����
            nextMove *= -1;
            spriteRenderer.flipX = nextMove == 1;
            // �κ�ũ ���� �� �ٽ� ����
            CancelInvoke();
            Invoke("Think", 3);
        }
    }

    void Think() // ���� �ൿ�� �ϱ����� �Լ�
    { 
        // ���� �ൿ�� ���� ���� ����
        nextMove = Random.Range(-1, 2);
        float nextThink = Random.Range(1f, 6f);
        // �κ�ũ
        Invoke("Think", nextThink);
        // �ִϸ����� �Ķ���� ��
        enemyAnimator.SetInteger("isRun", nextMove);
        // ����� �ƴ϶�� ��������Ʈ ����
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;
    }

    public void OnDamage(Vector2 targetPos)
    {
        // ������
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // ��������Ʈ ����
        spriteRenderer.flipY = true;
        // �ݶ��̴� ����
        enemyCollider.enabled = false;
        // addforce
        enemyRigidbody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // 2�� �� ����
        Destroy(gameObject, 2);
    }
}
