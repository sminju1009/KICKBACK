using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [Header ("LoginUserInfo")]
    public string accessToken; // access Token
    //public string refreshToken; // refresh Token
    public User loginUserInfo; // �α��� �� ���� ����

    [Header("ChannelInfo")]
    public int channelIndex = -1; // ���� ä�� �ε���
    public string channelName; // ���� ä�� �̸�
    public int cnt; // ä�� ���� �ο�
    public bool isOnGame; // ���� �� ����
    public List<SessionData> sessionList; // ���� ���� ���� ���� (players�� �ε��� ����)
    public int clearUserId = -1; // Ŭ���� �� ���� ���̵�

    [Header("Player")]
    public int myIdx = -1; // sessionList �� ���� �ε���
    public List<GameObject> players; // �÷��̾�� ���� (sessionList�� �ε��� ����)
    public int[] score; // �ε��� �� ����

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool isMe(string userName)
    {
        return sessionList[myIdx].UserName == userName;
    }

    public bool isMe(int userId)
    {
        return sessionList[myIdx].UserId == userId;
    }

    public int getUserIndex(string userName)
    {
        for (int i = 0; i < cnt; i++)
        {
            if (sessionList[i].UserName == userName)
            {
                return i;
            }
        }

        return -1;
    }

    public int getUserIndex(int userId)
    {
        for (int i = 0; i < cnt; i++)
        {
            if (sessionList[i].UserId == userId)
            {
                return i;
            }
        }

        return -1;
    }

    public void dataClear()
    {
        accessToken = "";
        //refreshToken = "";
        loginUserInfo = null;
        channelIndex = -1;
        channelName = "";
        cnt = -1;
        isOnGame = false;
        sessionList.Clear();
        clearUserId = -1;
        myIdx = -1;
        players.Clear();
        score = null;
    }

    public void gameDataClear()
    {
        channelIndex = -1;
        channelName = "";
        cnt = -1;
        isOnGame = false;
        //sessionList.Clear();
        clearUserId = -1;
        myIdx = -1;
        players.Clear();
        score = null;
    }
}
