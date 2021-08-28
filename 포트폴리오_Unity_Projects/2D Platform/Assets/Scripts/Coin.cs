using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���Ű� �÷��̾��� �Ⱦ�
        if (collision.tag == "Player")
        {
            // �÷��̾���Ʈ�� ȣ��
            PlayerController player = collision.GetComponent<PlayerController>();
            // ����
            player.PickupSound();
            // ���� ȹ�� �� ����
            GetCoin();
        }
    }

    protected virtual void GetCoin()
    {
        // ����
        Destroy(gameObject);
    }
}
