using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 필드
    int totalPoint; // 총합 점수
    int stagePoint; // 각 스테이지 점수
    int stageIndex; // 스테이지 인덱스
    public GameObject[] stages; // 스테이지들
    PlayerController player; // 플레이어
    static GameManager m_instance; // 싱글턴을 위한 인스턴스

    public Image[] playerLife; // 라이프 이미지
    // 텍스트, 버튼
    public Text pointText;
    public Text stageText;
    public Button retryButton;

    public static GameManager instance // 싱글턴 프로퍼티
    {
        get
        {
            // 인스턴스가 null이라면 오브젝트를 찾아서 리턴
            if (m_instance == null)
                m_instance = FindObjectOfType<GameManager>();
            return m_instance;
        }
    }

    void Awake()
    {
        // 초기화
        player = FindObjectOfType<PlayerController>();
        totalPoint = 0;
        stagePoint = 0;
        stageIndex = 0;
        // UI 출력
        StageUp();
    }

    public void AddPoint(int newPoint)
    {
        // 점수 획득
        stagePoint += newPoint;
        pointText.text = "POINT : " + (totalPoint + stagePoint);
    }

    public void NextStage()
    {
        // 스테이지 인덱스가 총 스테이지 갯수보다 적으면 스테이지 업
        if (stageIndex < stages.Length - 1)
        {
            // 현재스테이지 off후 다음스테이지 on
            stages[stageIndex++].SetActive(false);
            stages[stageIndex].SetActive(true);
            // 플레이어 위치 재설정
            player.RePosition();
            // 점수 갱신
            totalPoint += stagePoint;
            stagePoint = 0;
            StageUp();
        }
        // 아니면 게임 클리어
        else
        {
            // 멈춘 후 게임클리어 버튼 호출
            Text buttonText = retryButton.GetComponentInChildren<Text>();
            buttonText.text = "Game Clear";
            EndGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 허공에 플레이어가 떨어지면 라이프 다운
        if(collision.tag == "Player")
        {
            // 라이프 다운
            player.LifeDown();
            // 플레이어 위치 재설정
            player.RePosition();
        }
    }

    public void EndGame()
    {
        // 게임 종료
        Time.timeScale = 0;
        retryButton.gameObject.SetActive(true);
    }
 
    public void ReStart()
    {
        // 씬 로드 후 시간 재개
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    void StageUp()
    {
        // UI 출력
        stageText.text = "STAGE " + (stageIndex + 1);
    }

    public void LifeDownUI()
    {
        // UI 출력
        playerLife[player.life].color = new Color(1, 1, 1, 0.2f);
    }
}
