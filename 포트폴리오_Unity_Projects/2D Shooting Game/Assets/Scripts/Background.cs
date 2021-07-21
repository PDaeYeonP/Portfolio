using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

    float viewHeight;

    void Awake()
    {
        viewHeight = Camera.main.orthographicSize * 2;
    }

    void Update()
    {
        // 스프라이트 움직임 로직
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;
        // 스크롤링 헷갈림 주의!!
        if(sprites[endIndex].position.y < viewHeight * (-1))
        {
            // 스프라이트 스크롤링
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            //Vector3 frontSpritePos = sprites[endIndex].localPosition;
            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up * viewHeight;
            // 치환
            int endIndexSave = endIndex;
            startIndex = endIndex;
            endIndex = (endIndexSave - 1 == -1) ? sprites.Length - 1 : endIndexSave - 1;
        }
    }
}
