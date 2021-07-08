using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    Rigidbody2D rigid; // ������ٵ�
    Animator anim; // �ִϸ�����
    Vector3 frontVec; // �÷��̾� ���� ����
    GameObject scanObject; // ��ĵ�� ������Ʈ
    float xInput; //x�� ����
    float zInput; //z�� ����
    int speed; // ���ǵ�
    bool isHorizonMove; // �����̵� bool ����

    void Awake()
    {
        // �ʱ�ȭ
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        speed = 10;
    }

    void Update()
    {
        // ��ȣ�ۿ����� �ƴϸ� �̵� ����
        if(!GameManager.instance.isInteraction)
        {
            // ����Ű �Է�
            xInput = Input.GetAxisRaw("Horizontal");
            zInput = Input.GetAxisRaw("Vertical");
            // �����̵� üũ
            if (Input.GetButtonDown("Horizontal"))
                isHorizonMove = true;
            else if (Input.GetButtonDown("Vertical"))
                isHorizonMove = false;
            else if (Input.GetButtonUp("Horizontal") || Input.GetButtonUp("Vertical"))
                isHorizonMove = xInput != 0;
            // ���溤�� üũ
            if (Input.GetButtonDown("Vertical") && zInput < 0)
                frontVec = Vector3.down;
            else if (Input.GetButtonDown("Vertical") && zInput > 0)
                frontVec = Vector3.up;
            else if (Input.GetButtonDown("Horizontal") && xInput < 0)
                frontVec = Vector3.left;
            else if (Input.GetButtonDown("Horizontal") && xInput > 0)
                frontVec = Vector3.right;
        }
        // �̵�Ű�� ������ �ִ� ������ �ִϸ��̼� ���� ����Ű�� �������� �ִϸ��̼��� ����
        if (anim.GetInteger("xAxisRaw") != (int)xInput)
        {
            anim.SetBool("keyChange", true);
            anim.SetInteger("xAxisRaw", (int)xInput);
        }
        else if (anim.GetInteger("zAxisRaw") != (int)zInput)
        {
            anim.SetBool("keyChange", true);
            anim.SetInteger("zAxisRaw", (int)zInput);
        }
        else
            anim.SetBool("keyChange", false);
        // ��ȣ�ۿ�
        if (Input.GetKeyDown(KeyCode.Space) && scanObject != null)
            GameManager.instance.Interaction(scanObject);
    }

    void FixedUpdate()
    {
        // �밢�� �̵� ����
        Vector2 moveVec = isHorizonMove ? new Vector2(xInput, 0) : new Vector2(0, zInput);
        // ������ ���� �̵�
        rigid.velocity = moveVec * speed;
        // Ȯ�ο� �����
        Debug.DrawRay(rigid.position, frontVec * 0.7f, new Color(1, 0, 0));
        // ����ĳ��Ʈ
        RaycastHit2D scanRay = Physics2D.Raycast(rigid.position, frontVec, 0.7f, LayerMask.GetMask("Object"));
        // ����ĳ��Ʈ��Ʈ�� ���� �ݶ��̴��� �ΰ��� �ƴϸ� ��ĵ������Ʈ�� �Ҵ�
        if (scanRay.collider != null)
            scanObject = scanRay.collider.gameObject;
        else
            scanObject = null;
    }

}
