using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // �ʵ�
    int totalPoint; // ���� ����
    int stagePoint; // �� �������� ����
    int stageIndex; // �������� �ε���
    public GameObject[] stages; // ����������
    PlayerController player; // �÷��̾�
    static GameManager m_instance; // �̱����� ���� �ν��Ͻ�

    public Image[] playerLife; // ������ �̹���
    // �ؽ�Ʈ, ��ư
    public Text pointText;
    public Text stageText;
    public Button retryButton;

    public static GameManager instance // �̱��� ������Ƽ
    {
        get
        {
            // �ν��Ͻ��� null�̶�� ������Ʈ�� ã�Ƽ� ����
            if (m_instance == null)
                m_instance = FindObjectOfType<GameManager>();
            return m_instance;
        }
    }

    void Awake()
    {
        // �ʱ�ȭ
        player = FindObjectOfType<PlayerController>();
        totalPoint = 0;
        stagePoint = 0;
        stageIndex = 0;
        // UI ���
        StageUp();
    }

    public void AddPoint(int newPoint)
    {
        // ���� ȹ��
        stagePoint += newPoint;
        pointText.text = "POINT : " + (totalPoint + stagePoint);
    }

    public void NextStage()
    {
        // �������� �ε����� �� �������� �������� ������ �������� ��
        if (stageIndex < stages.Length - 1)
        {
            // ���罺������ off�� ������������ on
            stages[stageIndex++].SetActive(false);
            stages[stageIndex].SetActive(true);
            // �÷��̾� ��ġ �缳��
            player.RePosition();
            // ���� ����
            totalPoint += stagePoint;
            stagePoint = 0;
            StageUp();
        }
        // �ƴϸ� ���� Ŭ����
        else
        {
            // ���� �� ����Ŭ���� ��ư ȣ��
            Text buttonText = retryButton.GetComponentInChildren<Text>();
            buttonText.text = "Game Clear";
            EndGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ����� �÷��̾ �������� ������ �ٿ�
        if(collision.tag == "Player")
        {
            // ������ �ٿ�
            player.LifeDown();
            // �÷��̾� ��ġ �缳��
            player.RePosition();
        }
    }

    public void EndGame()
    {
        // ���� ����
        Time.timeScale = 0;
        retryButton.gameObject.SetActive(true);
    }
 
    public void ReStart()
    {
        // �� �ε� �� �ð� �簳
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    void StageUp()
    {
        // UI ���
        stageText.text = "STAGE " + (stageIndex + 1);
    }

    public void LifeDownUI()
    {
        // UI ���
        playerLife[player.life].color = new Color(1, 1, 1, 0.2f);
    }
}
