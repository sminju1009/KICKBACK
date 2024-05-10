using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrangeTeamGoalCheck : MonoBehaviour
{
    [Header("Script")]
    [SerializeField] private FootBallCountDownController countDownController;
    [SerializeField] private FootBallCameraFollowing cameraFollowing;

    [Header("Trigger")]
    public TextMeshProUGUI OrangeScore;
    private int score;
    public AudioSource audioSource;
    public AudioSource whistleAudio;
    public ParticleSystem[] particleSystems; // ��ƼŬ �ý��� �迭 �߰�

    [Header("Reset")]
    [SerializeField] private Transform[] orangeTeamPosition;
    [SerializeField] private Transform[] blueTeamPosition;
    public GameObject ball;
    public GameObject[] players;
    public Image CountDownImage;

    void Start()
    {
        score = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            score++;
            OrangeScore.text = score.ToString();
            audioSource.Play();
            PlayAllParticles(); // ��ƼŬ ��� �Լ� ȣ��

            Invoke("ResetPositions", 2.0f);
        }
    }

    private void PlayAllParticles()
    {
        foreach (var ps in particleSystems)
        {
            ps.Play();
        }
    }

    private void StopAllParticles()
    {
        foreach (var ps in particleSystems)
        {
            ps.Stop();
        }
    }

    private void ResetPositions()
    {
        whistleAudio.Play();

        cameraFollowing.isStarting = true;
        CountDownImage.gameObject.SetActive(true);

        // �� ��ġ ����
        ball.transform.position = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;

        // ���� Rigidbody ������Ʈ ã��
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
            // ���� �ӵ��� ���ӵ� �ʱ�ȭ
            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
        }

        // �÷��̾� ��ġ �� ȸ�� ����
        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = orangeTeamPosition[i].position;
            players[i].transform.rotation = Quaternion.identity; // �÷��̾��� ȸ���� 0���� ����
        }

        StopAllParticles();

        StartCoroutine(countDownController.StartGame());
    }
}
