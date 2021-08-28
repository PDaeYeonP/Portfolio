using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance; // 싱글턴 인스턴스
    public bool isGameOver { get; private set; } // 게임오버 상태 프로퍼티
    int score; // 점수

    // 게임 내에서 게임매니저 인스턴스는 싱글턴으로 하나만 존재
    void Awake()
    {
        // 인스턴스가 null이면
        if (instance == null)
        {
            // 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            instance = this;
            //씬 전환이 되더라도 파괴되지 않도록.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재한다면 자신을 삭제
            Destroy(this.gameObject);
        }
    }

    // 게임매니저 인스턴스에 접근할 수 있는 프로퍼티
    public static GameManager getInstance
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }

    // 점수 추가
    public void AddScore(int newScore)
    {
        // 게임오버가 아니라면
        if(!isGameOver)
        {
            // 받아온 인자값 갱신
            score += newScore;
            // UI 점수 출력
            UIManager.getInstance.UpdateScoreText(score);
        }
    }

    // 박스콜라이더에 플레이어 죽음 컨트롤
    void OnTriggerEnter(Collider other)
    {
        // 충돌체가 플레이어라면
        if(other.tag == "Player")
        {
            // 최고점수를 저장체에 불러옴
            int bestscore = PlayerPrefs.GetInt("BestScore");
            // 최고점수를 갱신했다면 기록 수정
            if(bestscore < score)
            {
                // UI 출력
                UIManager.getInstance.UpdateBestScoreText(score);
                PlayerPrefs.SetInt("BestScore", score);
            }
            // 게임오버 UI 활성화
            UIManager.getInstance.SetActiveGameOverUI(true);
            // 점수 초기화
            score = 0;
        }
    }
}
