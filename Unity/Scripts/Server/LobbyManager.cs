using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject LoginCanvas; // �α��� ĵ����
    [SerializeField] private GameObject LobbyCanvas; // �κ� ĵ����
    [SerializeField] private Button LogoutBtn; // �α� �ƿ� ��ư

    void Start()
    {
        TCPConnectManager.Instance.ConnectToServer();
        BusinessManager.Instance.ConnectToServer();

        // �α��� ĵ������ �κ� ĵ������ �±׷� ã�Ƽ� �Ҵ�
        LoginCanvas = GameObject.FindWithTag("Login Canvas");
        LobbyCanvas = GameObject.FindWithTag("Lobby Canvas");


        // ������ �α׾ƿ� ��ư�� ã�Ƽ� ��ư ������Ʈ�� �Ҵ�
        GameObject logoutButtonObject = GameObject.Find("Logout Btn");
        if (logoutButtonObject != null)
        {
            LogoutBtn = logoutButtonObject.GetComponent<Button>();

            // �α׾ƿ� ��ư�� Ŭ�� ������ �߰�
            if (LogoutBtn != null)
            {
                LogoutBtn.onClick.RemoveAllListeners(); // ���� ������ ����
                LogoutBtn.onClick.AddListener(LobbyOut); // ���ο� ������ �߰�
            }
        }
    }

    public void LobbyOut()
    {
        SceneManager.LoadScene("Login");

        // �α��� ĵ���� Ȱ��ȭ, �κ� ĵ���� ��Ȱ��ȭ
        if (LoginCanvas != null)
            LoginCanvas.SetActive(true);
        if (LobbyCanvas != null)
            LobbyCanvas.SetActive(false);

        // ��Ÿ �ʱ�ȭ �۾� ����
        TCPConnectManager.Instance.OnApplicationQuit();
        BusinessManager.Instance.OnApplicationQuit();
        DataManager.Instance.dataClear();
    }
}
