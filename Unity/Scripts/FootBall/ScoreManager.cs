using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text ResultScore;
    public int Score = 0;

    [Header("Result")]
    [SerializeField] private Timer timer;
    public GameObject Result;

    public IEnumerator Finish()
    {
     
        yield return new WaitForSeconds(0.2f);

        if (timer.isFinish)
        {
            // Player �±׸� ���� ��� ���� ������Ʈ�� Ž��
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                if (player != this.gameObject)
                {
                    // �ٸ� �÷��̾���� �浹�� ����
                    Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>(), true);
                }
            }

            if (ResultScore != null)
            {
                ResultScore.text = Score.ToString(); // ���â Ÿ�̸ӿ� ���� Ÿ�̸� �ؽ�Ʈ�� ����
            }

            CanvasGroup resultCanvasGroup = Result.GetComponent<CanvasGroup>();
            if (resultCanvasGroup != null)
            {
                float duration = 1.0f; // ���̵� ���ϴ� �� �ɸ��� �ð�(��)
                float elapsedTime = 0;

                // CanvasGroup�� alpha ���� 0���� 1�� ������ ����
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    resultCanvasGroup.alpha = Mathf.Lerp(resultCanvasGroup.alpha, 1, Time.deltaTime);
                    yield return null;
                }
                resultCanvasGroup.alpha = 1; // ���������� alpha ���� ������ 1�� �����Ͽ� Ȯ���� ���̰� ��
            }
        }
    }

}
