using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TypingEffect : MonoBehaviour
{
    public GameObject cursor; // 대화 끝을 알리는 커서
    AudioSource audio; // 오디오
    string targetText; // 본 대화내용
    Text effectText; // 이펙트중인 대화내용
    float charPerSeconds; // 속도
    int index; // 문자열 순서
    public bool isEffect { get; private set; }

    void Awake()
    {
        //초기화
        effectText = GetComponent<Text>();
        audio = GetComponent<AudioSource>();
        charPerSeconds = 0.05f;
        isEffect = false;
    }

    public void EffectStart(string newText)
    {
        // 텍스트 출력중이라면
        if(isEffect)
        {
            effectText.text = targetText;
            CancelInvoke();
            EffectEnd();
        }
        else
        {
            // 커서 비활성화
            cursor.SetActive(false);
            // 출력할 본 대화내용 담기
            targetText = newText;
            index = 0;
            // 이펙트중인 대화내용 비우기
            effectText.text = "";
            // 이펙트 시작
            isEffect = true;
            Invoke("Effecting", 0.5f);
        }
    }

    void Effecting()
    {
        // 대화내용을 모두 출력하였으면 엔드 호출
        if(targetText == effectText.text)
        {
            EffectEnd();
            return;
        }
        // 출력하기
        effectText.text += targetText[index];
        // 사운드
        if (targetText[index] != ' ')
            audio.Play();
        // 인덱스 증가
        index++;
        // 재귀함수
        Invoke("Effecting", charPerSeconds);
    }

    void EffectEnd()
    {
        isEffect = false;
        // 커서 활성화
        cursor.SetActive(true);
    }
}
