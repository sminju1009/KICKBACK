using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FootBallCountDownController : MonoBehaviour
{
    [SerializeField] FootBallCameraFollowing CameraFollowing;

    public Sprite[] sprites;
    public GameObject countDown;
    public Image countdownImage;

    public AudioClip[] audioClips;
    public AudioSource audioSources;

    public bool isCountDown;

    public IEnumerator StartGame()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            countdownImage.sprite = sprites[i];
            // ���̵��� ����
            PlayCountdownSound(i); // �ε����� �ش��ϴ� �Ҹ� ���

            // ������ ��������Ʈ�� �����ϰ� 0.5�� ��ٸ� (���̵�ƿ� ����)
            if (i < sprites.Length - 1)
            {
                StartCoroutine(FadeImage(true, 1f));
                // ���̵�ƿ� ����
                StartCoroutine(FadeImage(false, 1f));
                yield return new WaitForSecondsRealtime(1);
            }
            else
            {
                StartCoroutine(FadeImage(true, 0.3f));

                // ������ ��������Ʈ������ ���̵�ƿ� ���� �ٷ� ��ȯ
                yield return new WaitForSecondsRealtime(0.3f); // ���̵��ΰ� �Բ� 0.5�� ��ٸ�
            }
        }

        FinishCountdown(); // ���� ���� ó��
    }


    private IEnumerator FadeImage(bool fadeIn, float duration)
    {
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + duration)
        {
            float t = (Time.realtimeSinceStartup - startTime) / duration;
            Color color = countdownImage.color;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            countdownImage.color = color;
            yield return null;
        }

        // ���� ���İ� ����
        Color finalColor = countdownImage.color;
        finalColor.a = endAlpha;
        countdownImage.color = finalColor;
    }

    public void PlayCountdownSound(int index)
    {
        if (index < audioClips.Length)
        {
            audioSources.clip = audioClips[index];
            audioSources.Play();
        }
    }

    private void FinishCountdown()
    {
        countdownImage.gameObject.SetActive(false);
        isCountDown = false;
        CameraFollowing.isStarting = false; // ��Ʈ�ξ� ����
    }
}
