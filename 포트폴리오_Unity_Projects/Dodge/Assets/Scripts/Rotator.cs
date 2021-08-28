using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60f; // 회전속도

    void Update()
    {
        // 회전
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
