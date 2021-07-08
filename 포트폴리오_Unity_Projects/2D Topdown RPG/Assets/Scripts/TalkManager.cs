using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; // 토크 데이터
    Dictionary<int, Sprite> ImageData; // 이미지 데이터

    public Sprite[] imageArr; // 이미지 배열

    void Awake()
    {
        // 초기화
        talkData = new Dictionary<int, string[]>();
        ImageData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    void GenerateData()
    {
        // talk
        talkData.Add(100, new string[] { "이건 나무상자다." });
        talkData.Add(101, new string[] { "누군가 사용했던 책상이다" });
        talkData.Add(102, new string[] { "단단한 바위가 있네" });
        talkData.Add(1000, new string[] { "안녕?:0", "이 곳에 처음 왔구나?:1" });
        talkData.Add(2000, new string[] { "너는 누구니?:0", "나는 Luna라고 해:1" });
        // quest1
        talkData.Add(1010, new string[] { "어서와:0", "이 곳에 처음 왔지?:1" , "Luna에게 가봐, 너에게 줄 게 있어:0" });
        talkData.Add(2011, new string[] { "여어:0", "안녕? Ludo에게 듣고 왔구나:1", "너에게 줄 자그마한 선물이 있어:0", "일 하나만 해주면 선물을 줄게, 준비되면 다시 말을 걸어줘:1" });
        //quest2
        talkData.Add(2020, new string[] { "준비 됐어?:2", "Ludo옆에 있는 박스하나만 나에게 가져다 줘:0", "그럼 준비한 선물을 줄게:1" });
        talkData.Add(121, new string[] { "이 상자가 Luna가 부탁한 상자인 듯하다." });
        talkData.Add(2022, new string[] { "고마워!!:2", "자! 이게 너에게 줄 선물이야:2" });

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
        // 받아온 인덱스값이 토크데이터의 대화내용을 초과하면 널값 반환
        if (talkIndex >= talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public Sprite GetImage(int id, int imageIndex)
    {
        // 이미지 출력
        return ImageData[id + imageIndex];
    }
}
