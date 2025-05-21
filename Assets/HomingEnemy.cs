using UnityEngine;
using System.Collections;

public class HomingEnemy : MonoBehaviour
{
    public Transform target;
    public float moveDuration = 2f;
    public float knockbackDistance = 2f;
    public float knockbackTime = 1.5f;

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public float elapsedTime = 0f;

    private bool knockedBack = false;
    private float knockbackTimer = 0f;

    public AudioSource audioSource;
    public AudioClip knockbackSound;
    public AudioClip punchSound;

    private void Start()
    {
        if (startPosition == Vector3.zero)
            startPosition = transform.position;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (target == null || knockedBack) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / moveDuration);

        // Smooth lerp from spawn to target
        transform.position = Vector3.Lerp(startPosition, target.position, t);
        transform.LookAt(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PunchController"))
        {
            if (!BlockZone.Instance.AreBothHandsBlocking)
            {
                Debug.Log("[HomingEnemy] Punched and destroyed.");
                audioSource.PlayOneShot(punchSound);
                Destroy(gameObject);

                ScoreManager.Instance.IncreaseScore(10);
                Debug.Log("Enemy hit! Score increased.");
            }
            else
            {
                Debug.Log("[HomingEnemy] Punch ignored — player is blocking.");
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBlockZone"))
        {
            if (BlockZone.Instance.AreBothHandsBlocking)
            {
                KnockBackFromPlayer();
            }
        }
    }

    private void KnockBackFromPlayer()
    {
        if (!knockedBack)
        {
            StartCoroutine(PerformKnockback());
        }
    }

    private IEnumerator PerformKnockback()
    {
        knockedBack = true;
        knockbackTimer = knockbackTime;

        Vector3 start = transform.position;
        Vector3 direction = (transform.position - target.position).normalized;
        Vector3 end = start + direction * knockbackDistance;

        float elapsed = 0f;

        if (knockbackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(knockbackSound);
        }

        while (elapsed < knockbackTime)
        {
            float t = elapsed / knockbackTime;
            transform.position = Vector3.Lerp(start, end, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        knockedBack = false;
        elapsedTime = 0f;
        startPosition = transform.position;
    }
}
