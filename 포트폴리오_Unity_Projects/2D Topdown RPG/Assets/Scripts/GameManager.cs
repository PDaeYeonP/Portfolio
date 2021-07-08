using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager m_instance; // 싱글톤 인스턴스
    TalkManager talkManager; // 토크매니저
    QuestManager questManager; // 퀘스트매니저
    PlayerCtrl player;// 플레이어

    public GameObject menuSet; // 게임메뉴
    public Animator Panel; // 대화창 판넬
    public TypingEffect text; // 이펙팅 텍스트
    public Image npcImage; // NPC 이미지
    int talkIndex; // 대화 순서
    public bool isInteraction { get; private set; } // 상호작용 bool 변수

    public static GameManager instance // 인스턴스 게터
    {
        get
        {
            // 인스턴스가 널이면 게임매니저 컴포넌트를 찾아 반환
            if (m_instance == null)
                m_instance = FindObjectOfType<GameManager>();
            return m_instance;
        }
    }

    void Awake()
    {
        // 토크매니저와 퀘스트매니저 컴포넌트 찾아서 할당
        talkManager = FindObjectOfType<TalkManager>();
        questManager = FindObjectOfType<QuestManager>();
        //플레이어 할당
        player = FindObjectOfType<PlayerCtrl>();
        // 필드 초기화
        isInteraction = false;
        talkIndex = 0;
    }

    private void Start()
    {
        GameLoad();
    }

    void Update()
    {
        // 메뉴 호출 ESC키
        if (Input.GetButtonDown("Cancel"))
        {
            // 메뉴 창의 유무따라 열고 닫기
            if (menuSet.activeSelf)
                menuSet.SetActive(false);
            else
                menuSet.SetActive(true);
        }
    }

    public void Interaction(GameObject scanObject)
    {
        // 상호작용
        isInteraction = true;
        // 오브젝트데이터 변수에 값 할당
        ObjectData objectData = scanObject.GetComponent<ObjectData>();
        // 토크 실행
        Talk(objectData);
        // 대화창 실행
        Panel.SetBool("isShow", isInteraction);
    }

    void Talk(ObjectData objectData)
    {
        int questTalkIndex = 0;
        string talkString = null;
        if (!text.isEffect)
        {
            // 퀘스트가 존재한다면 퀘스트 대화 순서를 받아옴
            questTalkIndex = questManager.GetQuestTalkIndex(objectData.id);
            // 현재 진행중인 대화의 순서에 맞게 스트링 받아옴
            talkString = talkManager.GetTalk(objectData.id + questTalkIndex, talkIndex);
        }
        else
        {
            text.EffectStart("");
            return;
        }
        // 대화내용이 끝이나면
        if (talkString == null)
        {
            // 퀘스트 체크
            questManager.CheckQuest(objectData.id);
            // 상호작용 종료
            isInteraction = false;
            talkIndex = 0;
            return;
        }
        // 상호작용의 오브젝트가 NPC라면 이미지 출력
        if(objectData.isNPC)
        {
            // 스플릿을 이용해 ':'를 기준으로 대화 내용을 텍스트에 옮기고 스프라이트 실행
            text.EffectStart(talkString.Split(':')[0]);
            // 스프라이트 부여
            npcImage.sprite = talkManager.GetImage(objectData.id, int.Parse(talkString.Split(':')[1]));
            // 이미지 출력
            npcImage.gameObject.SetActive(true);
        }
        else
        {
            text.EffectStart(talkString);
            npcImage.gameObject.SetActive(false);
        }

        talkIndex++;
    }

    public void GameSave()
    {
        // 캐릭터의 좌표, 퀘스트 아이디번호, 해당 퀘스트 순서 저장
        PlayerPrefs.SetFloat("Player_x", player.transform.position.x);
        PlayerPrefs.SetFloat("Player_y", player.transform.position.y);
        PlayerPrefs.SetInt("Quest_id", questManager.questId);
        PlayerPrefs.SetInt("Quest_sequenceindex", questManager.questSequenceIndex);
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }

    public void GameLoad()
    {
        if(PlayerPrefs.HasKey("Player_x"))
        {
            float x = PlayerPrefs.GetFloat("Player_x");
            float y = PlayerPrefs.GetFloat("Player_y");
            player.transform.position = new Vector3(x, y, 0);
            questManager.questId = PlayerPrefs.GetInt("Quest_id");
            questManager.questSequenceIndex = PlayerPrefs.GetInt("Quest_sequenceindex");
            questManager.ControlObject();
        }
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void GameExitDeleteAll()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
