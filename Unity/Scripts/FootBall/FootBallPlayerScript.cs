using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FootBallPlayerScript : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;

    [Header("Scripts")]
    [SerializeField] private FootBallCameraFollowing cameraFollowing;
    [SerializeField] private Timer timer;

    [Header("Movement")]
    [SerializeField] private float maxSpeed = 15f; // �ִ� �ӵ�
    [SerializeField] private float boostSpeed = 40f;
    [SerializeField] private float forceMultiplier = 10000f; // ���� ���
    [SerializeField] private float decelerationForce = 5f; // ���ӷ�
    [SerializeField] private float reverseForceMultiplier = 0.1f; // ���� �� ���
    public LayerMask roadLayerMask; // Road LayerMask ����

    [Header("Jump")]
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private float fallMultiplier = 10f; // ���� �� �������� �߷� ���ӵ� ���
    private bool isJumping = false;
    private int jumpCount = 0; // ���� Ƚ�� ���� ����
    private int maxJump = 1; // �ִ� ���� Ƚ��

    [Header("Steering & Drift")]
    [SerializeField] private float rotateSpeed = 40f;
    [SerializeField] private float driftPower = 1000f;
    private float originalRotateSpeed;
    private float currentRotationSpeed;
    public float tiltAngle = 10f;

    [Header("Booster")]
    public Slider driftSlider; // �帮��Ʈ ������ �����̴�
    [SerializeField] private float driftFillRate = 0.2f; // ������ ������
    private float driftDecreaseRate = 1.5f;
    public int maxBoost = 2; // �ִ� �ν��� ����
    public int currentBoost = 0; // ���� �ν��� ����
    public float boostDuration = 5f; // �ν��� Ȱ��ȭ ���� �ð�
    private bool isBoosting = false;

    [Header("Effects")]
    public ParticleSystem speedLineParticleSystem;
    public ParticleSystem LeftboostFire;
    public ParticleSystem RightboostFire;
    public TrailRenderer LeftSkid;
    public TrailRenderer RightSkid;

    [Header("Audio")]
    public AudioSource movingAudioSource; // ������ �Ҹ��� AudioSource
    public AudioSource boostingAudioSource; // �ν��� �Ҹ��� AudioSource
    public AudioSource boostWindAudioSource; // �ν��� ��� �� �ٶ� �Ҹ� AudioSource
    public AudioSource driftAudioSource; // �帮��Ʈ �Ҹ��� AudioSource
    public AudioClip[] audioClips;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Rigidbody�� �߷� ����
        rb.useGravity = true; // Rigidbody�� �⺻ �߷� ���

        originalRotateSpeed = rotateSpeed;
    }

    private void FixedUpdate()
    {
        if (!cameraFollowing.isStarting && !timer.isFinish)
        {
            Move();
            Steer();
            Jump();
            Boosts();

            // LeftShift Ű �Է� �����Ͽ� Drift ���� ����
            if (Input.GetKey(KeyCode.LeftShift) && IsGrounded())
            {
                if (!driftAudioSource.isPlaying) // ������� ���� ��� ������ ���� ����
                {
                    driftAudioSource.clip = audioClips[3]; // �迭 1�� Ŭ�� (�ν��� �Ҹ�)
                    driftAudioSource.Play(); // ����� Ŭ�� ��� ����
                }

                Drift();
                LeftSkid.emitting = true;
                RightSkid.emitting = true;
            }
            else
            {
                if (driftAudioSource.isPlaying)
                {
                    driftAudioSource.Stop();
                }

                LeftSkid.emitting = false;
                RightSkid.emitting = false;
            }
        }
        else if (timer.isFinish)
        {
            animator.SetBool("Cute", true);
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        bool isOnRoad = Physics.Raycast(transform.position, -transform.up, 1f, roadLayerMask);

        float speedMultiplier = isOnRoad ? 1.0f : 0.7f; // Road�� ��� 1.0, NotRoad�� ��� 0.6 ���

        if (Mathf.Abs(verticalInput) > 0.01f)
        {
            if (!movingAudioSource.isPlaying)
            {
                movingAudioSource.clip = audioClips[0];
                movingAudioSource.Play();
            }
            movingAudioSource.pitch = isBoosting ? 1.3f : 1.0f;
        }
        else if (movingAudioSource.isPlaying)
        {
            movingAudioSource.Stop();
        }

        Vector3 forceDirection = transform.forward * verticalInput * speedMultiplier;
        float forceMultiplierAdjusted = verticalInput < 0 ? forceMultiplier * reverseForceMultiplier : forceMultiplier;
        Vector3 force = forceDirection * forceMultiplierAdjusted * speedMultiplier;

        if (!isBoosting)
        {
            if (rb.velocity.magnitude > maxSpeed * speedMultiplier)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed * speedMultiplier;
            }

            if (Mathf.Approximately(verticalInput, 0) && rb.velocity.magnitude > 0.1f)
            {
                rb.AddForce(-rb.velocity.normalized * decelerationForce, ForceMode.Force);
            }
            else
            {
                rb.AddForce(force);
            }

            if (verticalInput < 0 && rb.velocity.magnitude > (maxSpeed * reverseForceMultiplier * speedMultiplier))
            {
                rb.velocity = rb.velocity.normalized * (maxSpeed * reverseForceMultiplier * speedMultiplier);
            }
        }
        else
        {
            // �ν�Ʈ Ȱ��ȭ ��
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 1.5f, roadLayerMask))
            {
                // ������� �浹�� �����ϰ�, ������ ��� ���͸� ����Ͽ� �̵� ������ ����
                Vector3 groundNormal = hit.normal;
                Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward * boostSpeed * speedMultiplier, groundNormal).normalized;

                // ���鿡 �����ϰ� �ν�Ʈ ����
                Vector3 boostForce = projectedForward * boostSpeed * speedMultiplier - rb.velocity;
                rb.AddForce(boostForce, ForceMode.VelocityChange);
            }
            else
            {
                // ���߿� ������ ��(��, ����� �浹���� ���� ��) ������ ���� ����
                Vector3 boostForce = transform.forward * boostSpeed * speedMultiplier - rb.velocity;
                rb.AddForce(boostForce, ForceMode.VelocityChange);
            }
        }

        animator.SetBool("Move", Mathf.Abs(verticalInput) > 0.01f);
    }


    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && (IsGrounded() || jumpCount < maxJump))
        {
            jumpCount++; // ���� Ƚ�� ����
            isJumping = true;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            animator.SetBool("Jump", true);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            animator.SetBool("Jump", false);
        }
    }

    private void Boosts()
    {
        if (Input.GetKey(KeyCode.LeftControl) && rb.velocity.magnitude < boostSpeed && currentBoost > 0 && !isBoosting)
        {
            StartCoroutine(BoostRoutine());
        }
    }

    private IEnumerator BoostRoutine()
    {
        isBoosting = true;
        currentBoost--;

        // ���߿� ���ִ��� ���θ� Ȯ��
        bool isInAir = !Physics.Raycast(transform.position, -transform.up, 1.5f, roadLayerMask);
        float currentBoostDuration = isInAir ? 0.3f : boostDuration; // ���߿� �������� 1��, �ƴϸ� �⺻ �ν��� �ð� ���

        BoostEffect(true);

        boostingAudioSource.clip = audioClips[1]; // �迭 2�� Ŭ�� (�ν��� �Ҹ�)
        boostingAudioSource.Play(); // ����� Ŭ�� ��� ����
        boostWindAudioSource.clip = audioClips[2];
        boostWindAudioSource.Play();

        // ���� �ν��� ���� �ð� ���
        yield return new WaitForSeconds(currentBoostDuration);

        // Boost ����
        isBoosting = false;
        BoostEffect(false);
    }

    public IEnumerator BoostPadRoutine()
    {
        // Boost ����
        isBoosting = true;
        BoostEffect(true);

        // ����� Ŭ���� �Ź� �����ϰ� ����ϵ��� ����
        boostingAudioSource.clip = audioClips[1]; // �迭 2�� Ŭ�� (�ν��� �Ҹ�)
        boostingAudioSource.Play(); // ����� Ŭ�� ��� ����

        boostWindAudioSource.clip = audioClips[2];
        boostWindAudioSource.Play();

        // Boost ���� �ð�
        yield return new WaitForSeconds(boostDuration);

        // Boost ����
        isBoosting = false;
        BoostEffect(false);
    }

    private void BoostEffect(bool shouldPlay)
    {
        if (speedLineParticleSystem != null)
        {
            if (shouldPlay) speedLineParticleSystem.Play();
            else speedLineParticleSystem.Stop();
        }
        if (LeftboostFire != null && RightboostFire != null)
        {
            if (shouldPlay)
            {
                LeftboostFire.Play();
                RightboostFire.Play();
            }
            else
            {
                LeftboostFire.Stop();
                RightboostFire.Stop();
            }
        }
        animator.SetBool("Run", shouldPlay);
    }


    private void Steer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        // ���� �ӵ��� ������� �� ȸ�� �ӵ� ����
        float speedFactor = rb.velocity.magnitude / maxSpeed;
        speedFactor = Mathf.Pow(speedFactor, 2);
        float rotateAmount = horizontalInput * rotateSpeed * Mathf.Max(speedFactor, 1) * Time.deltaTime;

        if (Mathf.Abs(horizontalInput) > 0)
        {
            transform.Rotate(Vector3.up, rotateAmount, Space.World);
        }
        else
        {
            // �帮��Ʈ�� ������ ������ ���� y�� �������� ȸ�� ���·� ����
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.eulerAngles.y, 0), Time.deltaTime * 5);

            // ���⼭�� �帮��Ʈ�� ������ rotateSpeed�� ����
            rotateSpeed = Mathf.Lerp(rotateSpeed, originalRotateSpeed, Time.deltaTime * 5);
        }
    }

    private void Drift()
    {
        float driftDirection = Input.GetAxis("Horizontal") > 0 ? 1f : -1f;
        float verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(driftDirection) > 0.1f)
        {
            // �帮��Ʈ �� ȸ���ӵ��� ����
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, originalRotateSpeed * 5f, Time.deltaTime * 2);

            if (verticalInput > 0 && driftDirection != 0)
            {
                // �̲����� ȿ�� �߰�
                Vector3 driftForce = transform.right * driftDirection * driftPower;
                rb.AddForce(driftForce, ForceMode.Force);
                FillDriftGauge();
            }
        }
        else
        {
            // �帮��Ʈ�� ������ ȸ���ӵ��� ���� ����
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, originalRotateSpeed, Time.deltaTime * 20);
        }
        // ȸ���ӵ� ������Ʈ
        rotateSpeed = currentRotationSpeed;

        // �帮��Ʈ �� �ӵ��� ����
        rb.velocity *= Mathf.Lerp(0.9f, 0.7f, rb.velocity.magnitude / maxSpeed);
    }

    private void FillDriftGauge()
    {
        // �帮��Ʈ ������ ���� ������ �̵� ����� ���� �ٶ󺸰� �ִ� ������ ���̸� ���
        float forwardDotProduct = Vector3.Dot(transform.forward, rb.velocity.normalized);
        float driftStrength = 1 - Mathf.Abs(forwardDotProduct); // ���̰� Ŭ���� �帮��Ʈ ���·� ����.

        // �帮��Ʈ ������ ���� �������� ����. ������ ���Ҽ��� �� ���� ����
        if (driftSlider.value < 1)
        {
            driftSlider.value += driftStrength * driftFillRate * Time.deltaTime;
        }
        else if (currentBoost < maxBoost)
        {
            currentBoost++;
            StartCoroutine(DecreaseDriftGauge());
        }
    }

    private IEnumerator DecreaseDriftGauge()
    {
        while (driftSlider.value > 0)
        {
            driftSlider.value -= driftDecreaseRate * Time.deltaTime;
            yield return null;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        float distance = 0.1f;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            if (!hit.collider.isTrigger)
            {
                isJumping = false;
                jumpCount = 0; // ���� ����� �� ���� Ƚ�� �ʱ�ȭ
                return true;
            }
        }

        return false;
    }

    public IEnumerator PreventCollision()
    {
        // Player �±׸� ���� ��� ���� ������Ʈ�� Ž��
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player != this.gameObject)
            {
                // ��� ���� �ٸ� �÷��̾���� �浹�� ����
                Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>(), true);
            }
        }

        // ������ �ð�(��: 2��) ���� ���
        yield return new WaitForSeconds(2f);

        // �浹 ���ø� �����մϴ�.
        foreach (var player in players)
        {
            if (player != this.gameObject)
            {
                Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>(), false);
            }
        }
    }
}
