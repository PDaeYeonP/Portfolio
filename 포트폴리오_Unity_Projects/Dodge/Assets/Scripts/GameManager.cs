using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText;
    // 컴포넌트의 내용을 변경하고 싶으면 해당 컴포넌트 변수를 만들어 활용.
    public Text timeText; // 시간 갱신
    public Text recordText; // 기록 갱신

    private float surviveTime; // 생존 시간
    private bool isGameover; // 게임 상태

    void Start()
    {
        //초기화
        surviveTime = 0f;
        isGameover = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameover)
        {
            //시간 갱신
            surviveTime += Time.deltaTime;
            // 출력
            timeText.text = "Time : " + (int)surviveTime;
        }
        else
        { // R키를 누르면 씬을 로드
            if(Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene("SampleScene");
        }
    }

    public void EndGame()
    {
        // 상태 전환
        isGameover = true;
        // 텍스트 활성화
        gameoverText.SetActive(true);
        // 이전 최고 기록 가져오기
        float bestTime = PlayerPrefs.GetFloat("BestTime");
        // 기록을 갱신하였다면
        if(surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }
        // 출력
        recordText.text = "Best Time : " + (int)bestTime;
    }
}
