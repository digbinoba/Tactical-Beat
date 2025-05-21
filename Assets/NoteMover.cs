using UnityEngine;

public class NoteMover : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveDuration;
    private float startTime;
    private float beatTime;

    public float perfectWindow = 0.1f;
    public float goodWindow = 0.2f;
    public float okWindow = 0.3f;

    private ControllerFeedback controllerFeedback;
    private bool wasHit = false;

    private AudioSource audioSource;

    void Start()
    {
        controllerFeedback = FindObjectOfType<ControllerFeedback>();
        audioSource = GameObject.FindObjectOfType<AudioSource>();

        if (controllerFeedback == null)
            Debug.LogWarning("[NoteMover] ControllerFeedback script not found in scene!");

        if (audioSource == null)
            Debug.LogWarning("[NoteMover] AudioSource not found!");
    }

    public void SetNoteTarget(Vector3 start, Vector3 target, float duration, float beatTime)
    {
        startPosition = start;
        targetPosition = target;
        moveDuration = duration;
        this.beatTime = beatTime; // This is now in audio time
        startTime = Time.time;

        Debug.Log($"[NoteMover] Note initialized. Target beat time (song): {beatTime:F2}");
    }

    void Update()
    {
        float elapsed = Time.time - startTime;
        float t = Mathf.Clamp01(elapsed / moveDuration);
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);

        if (t >= 1f && !wasHit)
        {
            controllerFeedback?.ShowFeedback("Miss");
            Debug.Log("[NoteMover] Missed note.");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (wasHit) return;

        if (other.CompareTag("PunchController"))
        {
            wasHit = true;

            float songTime = audioSource.time;
            float timeDifference = Mathf.Abs(songTime - beatTime);

            Debug.Log($"[NoteMover] HIT! Audio Time: {songTime:F2}, Expected Beat Time: {beatTime:F2}, Δ: {timeDifference:F2}");

            if (timeDifference <= perfectWindow)
            {
                controllerFeedback?.ShowFeedback("Perfect");
                Debug.Log("[NoteMover] Hit registered: PERFECT");
            }
            else if (timeDifference <= goodWindow)
            {
                controllerFeedback?.ShowFeedback("Good");
                Debug.Log("[NoteMover] Hit registered: GOOD");
            }
            else if (timeDifference <= okWindow)
            {
                controllerFeedback?.ShowFeedback("Ok");
                Debug.Log("[NoteMover] Hit registered: OK");
            }
            else
            {
                controllerFeedback?.ShowFeedback("Bad");
                Debug.Log("[NoteMover] Hit registered: BAD");
            }

            Destroy(gameObject);
        }
    }
}
