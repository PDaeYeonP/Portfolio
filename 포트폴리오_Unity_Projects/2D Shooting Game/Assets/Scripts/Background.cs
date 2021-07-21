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
        // ��������Ʈ ������ ����
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;
        // ��ũ�Ѹ� �򰥸� ����!!
        if(sprites[endIndex].position.y < viewHeight * (-1))
        {
            // ��������Ʈ ��ũ�Ѹ�
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            //Vector3 frontSpritePos = sprites[endIndex].localPosition;
            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up * viewHeight;
            // ġȯ
            int endIndexSave = endIndex;
            startIndex = endIndex;
            endIndex = (endIndexSave - 1 == -1) ? sprites.Length - 1 : endIndexSave - 1;
        }
    }
}
