using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TypingEffect : MonoBehaviour
{
    public GameObject cursor; // ��ȭ ���� �˸��� Ŀ��
    AudioSource audio; // �����
    string targetText; // �� ��ȭ����
    Text effectText; // ����Ʈ���� ��ȭ����
    float charPerSeconds; // �ӵ�
    int index; // ���ڿ� ����
    public bool isEffect { get; private set; }

    void Awake()
    {
        //�ʱ�ȭ
        effectText = GetComponent<Text>();
        audio = GetComponent<AudioSource>();
        charPerSeconds = 0.05f;
        isEffect = false;
    }

    public void EffectStart(string newText)
    {
        // �ؽ�Ʈ ������̶��
        if(isEffect)
        {
            effectText.text = targetText;
            CancelInvoke();
            EffectEnd();
        }
        else
        {
            // Ŀ�� ��Ȱ��ȭ
            cursor.SetActive(false);
            // ����� �� ��ȭ���� ���
            targetText = newText;
            index = 0;
            // ����Ʈ���� ��ȭ���� ����
            effectText.text = "";
            // ����Ʈ ����
            isEffect = true;
            Invoke("Effecting", 0.5f);
        }
    }

    void Effecting()
    {
        // ��ȭ������ ��� ����Ͽ����� ���� ȣ��
        if(targetText == effectText.text)
        {
            EffectEnd();
            return;
        }
        // ����ϱ�
        effectText.text += targetText[index];
        // ����
        if (targetText[index] != ' ')
            audio.Play();
        // �ε��� ����
        index++;
        // ����Լ�
        Invoke("Effecting", charPerSeconds);
    }

    void EffectEnd()
    {
        isEffect = false;
        // Ŀ�� Ȱ��ȭ
        cursor.SetActive(true);
    }
}
