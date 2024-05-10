using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlueTeamGoalCheck : MonoBehaviour
{
    public TextMeshProUGUI BlueScore;
    private int score;
    public AudioSource audioSource;
    public ParticleSystem[] particleSystems; // ��ƼŬ �ý��� �迭 �߰�

    void Start()
    {
        score = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            score++;
            BlueScore.text = score.ToString();
            audioSource.Play();
            PlayAllParticles(); // ��ƼŬ ��� �Լ� ȣ��
        }
    }

    void PlayAllParticles()
    {
        foreach (var ps in particleSystems)
        {
            ps.Play();
        }
    }
}
