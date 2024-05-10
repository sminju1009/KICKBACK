using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public class LoginManager : MonoBehaviour
{
    [Header ("Commons")]
    [SerializeField] private GameObject SignUpPopUp; // ȸ�� ���� �˾� ������Ʈ
    [SerializeField] private GameObject LoginCanvas; // �α��� ĵ����
    [SerializeField] private GameObject LobbyCanvas; // �κ� ĵ����

    [Header ("Login")]
    [SerializeField] private TMP_InputField LoginEmail; // �α��� �̸��� ��ǲ�ʵ�
    [SerializeField] private TMP_InputField LoginPassword; // �н����� ��ǲ �ʵ�
    [SerializeField] private Button LoginSignUpBtn; // ȸ�� ���� �˾� ��� ��ư
    [SerializeField] private Button LoginBtn; // �α��� ��ư
    [SerializeField] private Button LoginExit; // ���� ��ư
    [SerializeField] private GameObject ErrorPopup; // �α��� ���� �� �˾�

    [Header ("SignUp")]
    [SerializeField] private TMP_InputField SignUpEmail; // ȸ������ �̸��� ��ǲ�ʵ�
    [SerializeField] private TMP_InputField SignUpName; // ȸ������ �̸� ��ǲ�ʵ�
    [SerializeField] private TMP_InputField SignUpPassword; // ȸ������ ��й�ȣ ��ǲ�ʵ�
    [SerializeField] private TMP_InputField SignUpPasswordCheck; // ȸ������ ��й�ȣ ���Է� ��ǲ�ʵ�
    [SerializeField] private TMP_Text SignUpFeedback; // ȸ������ ���� / ���� ���� �ǵ�� �ؽ�Ʈ
    [SerializeField] private Button SignUpConfirmBtn; // ȸ�� ���� �ϱ� ��ư
    [SerializeField] private Button SignUpCancelBtn; // ȸ������ ���� ��ư

    private string url = "https://k10c209.p.ssafy.io/api/v1"; // ��û URL

    // ���� �α��� �� ���¿��� �α��� �Ŵ��� ���� �� �α��� �ʿ� ������ �κ�� �ٷ� �̵�
    private void OnEnable()
    {
        if (DataManager.Instance.loginUserInfo != null && DataManager.Instance.loginUserInfo.UserId != 0)
        {
            goToLobby();
        }
    }

    #region ȸ�� ����

    // ȸ�� �������� ����
    public void GoSignUp()
    {
        closeErrPopUp();
        SignUpPopUp.SetActive(true);
        SignUpConfirmBtn.interactable = true;
    }

    // ȸ�� ���� ��ư Ŭ�� ��
    public void SignUpConfirm()
    {
        string email = SignUpEmail.text;
        string nickname = SignUpName.text;
        string password = SignUpPassword.text;
        string passwordCheck = SignUpPasswordCheck.text;

        closeErrPopUp();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(nickname) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordCheck))
        {
            SignUpFeedback.text = "��� �ʵ带 �Է����ּ���.";
        }
        else if (password != passwordCheck)
        {
            SignUpFeedback.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
            return;
        }
        else
        {
            SignUpFeedback.text = "";
        }

        SignUpConfirmBtn.interactable = false;
        // ȸ�� ���� ��û ������
        StartCoroutine(SignUpRequest(email, password, nickname));
        StartCoroutine(SignUpConfirmCancel());
    }

    // ȸ�� ���� ��� ��
    public void SignUpCancel()
    {
        // ȸ�� ���� ��� �� �Է� �ʵ� �� �ǵ�� �ؽ�Ʈ �ʱ�ȭ
        // ȸ������ ��� �� �Է� �ʵ� �� �ǵ�� �ؽ�Ʈ �ʱ�ȭ
        SignUpEmail.text = "";
        SignUpName.text = "";
        SignUpPassword.text = "";
        SignUpPasswordCheck.text = "";
        SignUpFeedback.text = "";

        closeErrPopUp();

        // �α��� �������� �̵�
        SignUpPopUp.SetActive(false);
    }

    public IEnumerator SignUpConfirmCancel()
    {
        yield return new WaitForSeconds(1.0f);

        // ȸ�� ���� ��� �� �Է� �ʵ� �� �ǵ�� �ؽ�Ʈ �ʱ�ȭ
        // ȸ������ ��� �� �Է� �ʵ� �� �ǵ�� �ؽ�Ʈ �ʱ�ȭ
        SignUpEmail.text = "";
        SignUpName.text = "";
        SignUpPassword.text = "";
        SignUpPasswordCheck.text = "";
        SignUpFeedback.text = "";

        closeErrPopUp();

        // �α��� �������� �̵�
        SignUpPopUp.SetActive(false);
    }

    // ȸ�� ���� ��û ������
    private IEnumerator SignUpRequest(string email, string password, string nickname)
    {
        closeErrPopUp();

        string requestUrl = url + "/member/signup";
        User user = new User
        {
            Email = email,
            Password = password,
            NickName = nickname
        };

        string jsonRequestBody = JsonUtility.ToJson(user);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);

        // ��û ����
        using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            // ��û ���� ��
            if (request.result == UnityWebRequest.Result.Success)
            {
                SignUpFeedback.text = "ȸ�� ������ �Ϸ�Ǿ����ϴ�.";
            }
            // ��û ���� ��
            else
            {
                Debug.LogError("Error : " + request.error);
                // ���̵� �ߺ�
                if (request.responseCode == 409)
                {
                    SignUpFeedback.text = "�ߺ��� ���̵��Դϴ�.";
                }
                // �� ��
                else
                {
                    SignUpFeedback.text = "������ ������ �� �����ϴ�.";
                }

                SignUpConfirmBtn.interactable = true;
                yield break;
            }
        }
    }

    #endregion

    #region �α���

    // �α��� ��ư Ŭ�� ��
    public void LoginConfirm()
    {
        string email = LoginEmail.text;
        string password = LoginPassword.text;

        // �Է� Ȯ��
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return;
        }

        //LoginBtn.interactable = true;
        StartCoroutine(LoginRequest(email, password));
    }


    // �α��� ��û ������
    IEnumerator LoginRequest(string email, string password)
    {
        string requestUrl = url + "/member/login";

        User user = new User
        {
            Email = email,
            Password = password
        };

        string json = JsonUtility.ToJson(user);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        // ��û ����
        using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");


            yield return request.SendWebRequest();

            // ��û ���� ��
            if (request.result == UnityWebRequest.Result.Success)
            {
                closeErrPopUp();

                DataManager.Instance.accessToken = request.GetResponseHeader("accessToken");
                //DataManager.Instance.refreshToken = request.GetResponseHeader("refreshToken");

                // �α��� �� ���� ���� �ҷ�����
                using (UnityWebRequest userProfileRequest = UnityWebRequest.Get(url + "/member/get"))
                {
                    userProfileRequest.SetRequestHeader("Authorization", DataManager.Instance.accessToken);

                    yield return userProfileRequest.SendWebRequest();

                    // ���� ��û ���� ��
                    if (userProfileRequest.result == UnityWebRequest.Result.Success)
                    {
                        DataManager.Instance.loginUserInfo = JsonUtility.FromJson<User>(userProfileRequest.downloadHandler.text);
                    }
                    // ���� ��û ���� ��
                    else
                    {
                        Debug.Log(userProfileRequest.error);

                    }
                }

                goToLobby();
            }
            // ��û ���� ��
            else
            {
                Debug.LogError("Error : " + request.error);
                ErrorPopup.SetActive(true);

                // ���̵� �ߺ�
                if (request.responseCode == 400)
                {
                    Debug.Log("�α��� ����");
                }
                // �� ��
                else
                {
                    Debug.Log("������ ������ �� �����ϴ�.");
                }

                LoginBtn.interactable = true;
                yield break;
            }
        }
    }
    #endregion

    #region ��Ÿ ���
    // �α��� ���� �˾� �ݱ�
    public void closeErrPopUp()
    {
        ErrorPopup.SetActive(false);
    }

    // �α��� ���� �κ� �ѱ�
    public void goToLobby()
    {
        closeErrPopUp();
        // SceneManager.LoadScene("Room");
        SceneManager.LoadScene("Lobby");
        LoginCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);
    }

    public void QuitBtnClicked()
    {
        Application.Quit();
    }
    #endregion
}
