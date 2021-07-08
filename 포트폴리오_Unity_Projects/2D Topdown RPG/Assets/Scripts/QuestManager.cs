using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId { get; set; } // 현재 퀘스트 아이디
    Dictionary<int, QuestData> questList; // 퀘스트리스트
    public int questSequenceIndex { get; set; } // 퀘스트 순서
    public GameObject[] questObjects; // 퀘스트 오브젝트들

    void Awake()
    {
        // 초기화
        questList = new Dictionary<int, QuestData>();
        questId = 10;
        questSequenceIndex = 0;
        GenerateData();
    }

    void GenerateData()
    {
        // 퀘스트 입력
        questList.Add(10, new QuestData("첫 마을 방문", new int[] { 1000, 2000 }));
        questList.Add(20, new QuestData("루나의 박스 찾아주기", new int[] { 2000, 100, 2000 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        // 스캔오브젝트의 아이디가 퀘스트리스트중 현재 진행중인 퀘스트의 순서에 맞는 npc 아이디와 같으면
        // 퀘스트 대사를 넘겨주고 아니면 기본대사를 출력시킨다.
        if (questList.ContainsKey(questId) && id == questList[questId].npcId[questSequenceIndex])
            return questId + questSequenceIndex;
        else
            return 0;
    }

    public void CheckQuest(int id)
    {
        // 퀘스트리스트에 매개변수로 받아온 아이디에 해당하는 키값이 동일하면 실행
        if (questList.ContainsKey(questId) && id == questList[questId].npcId[questSequenceIndex])
        {
            // 퀘스트 순서를 증가
            questSequenceIndex++;
            // 퀘스트에 필요한 오브젝트 컨트롤
            ControlObject();
            // 해당 퀘스트가 끝났다면 다음 퀘스트로 변경
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
