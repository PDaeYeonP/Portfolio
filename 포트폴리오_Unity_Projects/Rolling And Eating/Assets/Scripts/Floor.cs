using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public bool isGone; // 플레이어가 떠났는지 확인

    void OnEnable()
    {
        // 활성화시 false
        isGone = false;
    }

    void OnCollisionExit(Collision collision)
    {
        // 충돌이 일어나고 충돌체가 표면에서 떨어지면 true 반환
        if (collision.gameObject.tag == "Player")
            isGone = true;
    }
}
//f