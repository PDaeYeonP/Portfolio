using System.Collections;
using System.Collections.Generic;

public class QuestData
{
    // ����Ʈ �̸�
    public string questName;
    // ����Ʈ�� �� NPC ���̵� �迭
    public int[] npcId;
    // ������
    public QuestData(string name, int[] npc)
    {
        questName = name;
        npcId = npc;
    }
}
