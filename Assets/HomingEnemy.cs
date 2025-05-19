using System.Collections;
using UnityEngine;

public class HomingEnemy : MonoBehaviour
{
    public Transform target;
    public float moveDuration = 2f;
    public AnimationCurve moveCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public float knockbackDistance = 2f;
    public float knockbackTime = 1.5f;

    public AudioSource audioSource;
    public AudioClip knockbackSound;
    public AudioClip punchSound;

    private Vector3 startPosition;
    private float elapsedTime = 0f;
    private bool knockedBack = false;
    private float knockbackTimer = 0f;

    private void Start()
    {
        startPosition = transform.position;

        // Optional safety check
        if (audioSource == null)
        {
            Debug.LogWarning("[HomingEnemy] AudioSource not assigned.");
        }
    }

    private void Update()
    {
        if (target == null || knockedBack) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / moveDuration);
        float curvedT = moveCurve.Evaluate(t);

        transform.position = Vector3.Lerp(startPosition, target.position, curvedT);
        transform.LookAt(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PunchController"))
        {
            if (!BlockZone.Instance.AreBothHandsBlocking)
            {
                Debug.Log("[HomingEnemy] Punch detected");

                if (punchSound == null)
                    Debug.LogWarning("Punch sound is not assigned!");

                if (audioSource == null)
                    Debug.LogWarning("AudioSource is missing!");

                if (audioSource && punchSound)
                {
                    Debug.Log("Playing punch sound...");
                    audioSource.PlayOneShot(punchSound);
                }

                ScoreManager.Instance.IncreaseScore(10);
                Destroy(gameObject);
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

        if (knockbackSound && audioSource && audioSource.enabled)
        {
            audioSource.PlayOneShot(knockbackSound);
        }

        float elapsed = 0f;
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
