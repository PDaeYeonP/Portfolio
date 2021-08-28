using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    Transform playerTransform; // 플레이어 트랜스폼
    Vector3 offset; // 간격유지를 위한 오프셋 거리
    void Awake()
    {
        //플레이어 트랜스폼 받아오기
        playerTransform = FindObjectOfType<Player>().transform;
        //씬에서 설정된 카메라 포지션에서 플레이어 포지션 거리 계산
        offset = transform.position - playerTransform.position;
    }

    void LateUpdate()
    {
        //카메라 포지션 갱신
        transform.position = playerTransform.position + offset;
    }
}
// f