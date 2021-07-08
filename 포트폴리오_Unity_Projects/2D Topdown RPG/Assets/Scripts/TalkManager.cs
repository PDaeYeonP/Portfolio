using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; // ��ũ ������
    Dictionary<int, Sprite> ImageData; // �̹��� ������

    public Sprite[] imageArr; // �̹��� �迭

    void Awake()
    {
        // �ʱ�ȭ
        talkData = new Dictionary<int, string[]>();
        ImageData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    void GenerateData()
    {
        // talk
        talkData.Add(100, new string[] { "�̰� �������ڴ�." });
        talkData.Add(101, new string[] { "������ ����ߴ� å���̴�" });
        talkData.Add(102, new string[] { "�ܴ��� ������ �ֳ�" });
        talkData.Add(1000, new string[] { "�ȳ�?:0", "�� ���� ó�� �Ա���?:1" });
        talkData.Add(2000, new string[] { "�ʴ� ������?:0", "���� Luna��� ��:1" });
        // quest1
        talkData.Add(1010, new string[] { "���:0", "�� ���� ó�� ����?:1" , "Luna���� ����, �ʿ��� �� �� �־�:0" });
        talkData.Add(2011, new string[] { "����:0", "�ȳ�? Ludo���� ��� �Ա���:1", "�ʿ��� �� �ڱ׸��� ������ �־�:0", "�� �ϳ��� ���ָ� ������ �ٰ�, �غ�Ǹ� �ٽ� ���� �ɾ���:1" });
        //quest2
        talkData.Add(2020, new string[] { "�غ� �ƾ�?:2", "Ludo���� �ִ� �ڽ��ϳ��� ������ ������ ��:0", "�׷� �غ��� ������ �ٰ�:1" });
        talkData.Add(121, new string[] { "�� ���ڰ� Luna�� ��Ź�� ������ ���ϴ�." });
        talkData.Add(2022, new string[] { "����!!:2", "��! �̰� �ʿ��� �� �����̾�:2" });

        //image
        ImageData.Add(1000 + 0, imageArr[0]);
        ImageData.Add(1000 + 1, imageArr[1]);
        ImageData.Add(1000 + 2, imageArr[2]);
        ImageData.Add(1000 + 3, imageArr[3]);
        ImageData.Add(2000 + 0, imageArr[4]);
        ImageData.Add(2000 + 1, imageArr[5]);
        ImageData.Add(2000 + 2, imageArr[6]);
        ImageData.Add(2000 + 3, imageArr[7]);
    }

    public string GetTalk(int id, int talkIndex)
    {
        // �޾ƿ� �ε������� ��ũ�������� ��ȭ������ �ʰ��ϸ� �ΰ� ��ȯ
        if (talkIndex >= talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public Sprite GetImage(int id, int imageIndex)
    {
        // �̹��� ���
        return ImageData[id + imageIndex];
    }
}
