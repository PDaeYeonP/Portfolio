using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance; // 싱글턴
    public GameObject mainUI; // 초반 메인 화면
    public GameObject GameOverUI; // 게임오버시 화면
    public Text scoreText; // 점수 텍스트
    public Text bestScoreText; // 최고점수 텍스트
    public bool powerOn; // 실행상태

    void Awake()
    {
        // 최초 실행상태 false
        powerOn = false;
        // 싱글턴
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public static UIManager getInstance
    {
        // 싱글턴 프로퍼티
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }

    public void UpdateScoreText(int newscore)
    {
        // 점수 텍스트 갱신
        scoreText.text = "SCORE : " + newscore;
    }

    public void UpdateBestScoreText(int newscore)
    {
        // 최고 점수 텍스트 갱신
        bestScoreText.text = "Best Score : " + newscore;
    }

    public void SetActiveGameOverUI(bool active)
    {
        // 게임오버 화면 활성화
        GameOverUI.SetActive(true);
    }

    public void GameStart()
    {
        // 최초 게임 시작시 실행 true
        powerOn = true;
        // 저장체 기록 모두 삭제
        PlayerPrefs.DeleteAll();
        // 메인 화면 비활성화
        mainUI.SetActive(false);
    }

    public void GameRestart()
    {
        // 리스타트 버튼 누르면 새게임 시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // 한번 실행했으면 메인 화면은 비활성화
        if (powerOn)
            mainUI.SetActive(false);
        // 게임오버 화면 비활성화
        GameOverUI.SetActive(false);
        // 점수 텍스트 초기화
        UpdateScoreText(0);
    }
}
