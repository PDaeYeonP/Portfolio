using System.Collections;
using System.Collections.Generic;

public class QuestData
{
    // 퀘스트 이름
    public string questName;
    // 퀘스트에 들어갈 NPC 아이디 배열
    public int[] npcId;
    // 생성자
    public QuestData(string name, int[] npc)
    {
        questName = name;
        npcId = npc;
    }
}
