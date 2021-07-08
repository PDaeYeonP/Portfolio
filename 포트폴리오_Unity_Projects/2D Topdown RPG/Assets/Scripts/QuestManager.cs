using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId { get; set; } // ���� ����Ʈ ���̵�
    Dictionary<int, QuestData> questList; // ����Ʈ����Ʈ
    public int questSequenceIndex { get; set; } // ����Ʈ ����
    public GameObject[] questObjects; // ����Ʈ ������Ʈ��

    void Awake()
    {
        // �ʱ�ȭ
        questList = new Dictionary<int, QuestData>();
        questId = 10;
        questSequenceIndex = 0;
        GenerateData();
    }

    void GenerateData()
    {
        // ����Ʈ �Է�
        questList.Add(10, new QuestData("ù ���� �湮", new int[] { 1000, 2000 }));
        questList.Add(20, new QuestData("�糪�� �ڽ� ã���ֱ�", new int[] { 2000, 100, 2000 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        // ��ĵ������Ʈ�� ���̵� ����Ʈ����Ʈ�� ���� �������� ����Ʈ�� ������ �´� npc ���̵�� ������
        // ����Ʈ ��縦 �Ѱ��ְ� �ƴϸ� �⺻��縦 ��½�Ų��.
        if (questList.ContainsKey(questId) && id == questList[questId].npcId[questSequenceIndex])
            return questId + questSequenceIndex;
        else
            return 0;
    }

    public void CheckQuest(int id)
    {
        // ����Ʈ����Ʈ�� �Ű������� �޾ƿ� ���̵� �ش��ϴ� Ű���� �����ϸ� ����
        if (questList.ContainsKey(questId) && id == questList[questId].npcId[questSequenceIndex])
        {
            // ����Ʈ ������ ����
            questSequenceIndex++;
            // ����Ʈ�� �ʿ��� ������Ʈ ��Ʈ��
            ControlObject();
            // �ش� ����Ʈ�� �����ٸ� ���� ����Ʈ�� ����
            if (questSequenceIndex == questList[questId].npcId.Length)
                NextQuest();
        }
    }

    void NextQuest()
    {
        questId += 10;
        questSequenceIndex = 0;
    }

    public void ControlObject()
    {
        switch(questId)
        {
            case 10:
                if (questSequenceIndex == questList[10].npcId.Length)
                    questObjects[0].SetActive(true);
                break;
            case 20:
                if(questSequenceIndex < 2)
                    questObjects[0].SetActive(true);
                if (questSequenceIndex == 2)
                    questObjects[0].SetActive(false);
                break;
        }
    }
}
