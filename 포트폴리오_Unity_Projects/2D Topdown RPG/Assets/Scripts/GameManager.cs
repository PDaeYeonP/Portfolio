using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager m_instance; // �̱��� �ν��Ͻ�
    TalkManager talkManager; // ��ũ�Ŵ���
    QuestManager questManager; // ����Ʈ�Ŵ���
    PlayerCtrl player;// �÷��̾�

    public GameObject menuSet; // ���Ӹ޴�
    public Animator Panel; // ��ȭâ �ǳ�
    public TypingEffect text; // ������ �ؽ�Ʈ
    public Image npcImage; // NPC �̹���
    int talkIndex; // ��ȭ ����
    public bool isInteraction { get; private set; } // ��ȣ�ۿ� bool ����

    public static GameManager instance // �ν��Ͻ� ����
    {
        get
        {
            // �ν��Ͻ��� ���̸� ���ӸŴ��� ������Ʈ�� ã�� ��ȯ
            if (m_instance == null)
                m_instance = FindObjectOfType<GameManager>();
            return m_instance;
        }
    }

    void Awake()
    {
        // ��ũ�Ŵ����� ����Ʈ�Ŵ��� ������Ʈ ã�Ƽ� �Ҵ�
        talkManager = FindObjectOfType<TalkManager>();
        questManager = FindObjectOfType<QuestManager>();
        //�÷��̾� �Ҵ�
        player = FindObjectOfType<PlayerCtrl>();
        // �ʵ� �ʱ�ȭ
        isInteraction = false;
        talkIndex = 0;
    }

    private void Start()
    {
        GameLoad();
    }

    void Update()
    {
        // �޴� ȣ�� ESCŰ
        if (Input.GetButtonDown("Cancel"))
        {
            // �޴� â�� �������� ���� �ݱ�
            if (menuSet.activeSelf)
                menuSet.SetActive(false);
            else
                menuSet.SetActive(true);
        }
    }

    public void Interaction(GameObject scanObject)
    {
        // ��ȣ�ۿ�
        isInteraction = true;
        // ������Ʈ������ ������ �� �Ҵ�
        ObjectData objectData = scanObject.GetComponent<ObjectData>();
        // ��ũ ����
        Talk(objectData);
        // ��ȭâ ����
        Panel.SetBool("isShow", isInteraction);
    }

    void Talk(ObjectData objectData)
    {
        int questTalkIndex = 0;
        string talkString = null;
        if (!text.isEffect)
        {
            // ����Ʈ�� �����Ѵٸ� ����Ʈ ��ȭ ������ �޾ƿ�
            questTalkIndex = questManager.GetQuestTalkIndex(objectData.id);
            // ���� �������� ��ȭ�� ������ �°� ��Ʈ�� �޾ƿ�
            talkString = talkManager.GetTalk(objectData.id + questTalkIndex, talkIndex);
        }
        else
        {
            text.EffectStart("");
            return;
        }
        // ��ȭ������ ���̳���
        if (talkString == null)
        {
            // ����Ʈ üũ
            questManager.CheckQuest(objectData.id);
            // ��ȣ�ۿ� ����
            isInteraction = false;
            talkIndex = 0;
            return;
        }
        // ��ȣ�ۿ��� ������Ʈ�� NPC��� �̹��� ���
        if(objectData.isNPC)
        {
            // ���ø��� �̿��� ':'�� �������� ��ȭ ������ �ؽ�Ʈ�� �ű�� ��������Ʈ ����
            text.EffectStart(talkString.Split(':')[0]);
            // ��������Ʈ �ο�
            npcImage.sprite = talkManager.GetImage(objectData.id, int.Parse(talkString.Split(':')[1]));
            // �̹��� ���
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
        // ĳ������ ��ǥ, ����Ʈ ���̵��ȣ, �ش� ����Ʈ ���� ����
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
