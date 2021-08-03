using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    float maxFireDelay;
    float curFireDelay;
    public ObjectManager objectManager;

    public Vector3 followPos;
    int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;

    void Awake()
    {
        parentPos = new Queue<Vector3>();
        followDelay = 12;
        curFireDelay = 0.0f;
        maxFireDelay = 0.25f;
    }
    void Update()
    {
        Follow();
        Fire();
        Reload();
    }

    void Follow()
    {
        if(!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position);

        if (parentPos.Count > followDelay)
            followPos = parentPos.Dequeue();
        else if (parentPos.Count < followDelay)
            followPos = parent.position;

        transform.position = followPos;
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;
        if (curFireDelay < maxFireDelay)
            return;

        GameObject bullet = objectManager.MakeObject("FollowerBullet");
        bullet.transform.position = transform.position;

        // 회전값 초기화
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector3.up * 10, ForceMode2D.Impulse);

        curFireDelay = 0;
    }

    void Reload()
    {
        curFireDelay += Time.deltaTime;
    }
}
