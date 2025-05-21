using UnityEngine;

public class NoteMover : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveDuration;
    private float startTime;

    public void SetNoteTarget(Vector3 start, Vector3 target, float duration)
    {
        startPosition = start;
        targetPosition = target;
        moveDuration = duration;
        startTime = Time.time;
    }

    void Update()
    {
        float elapsed = Time.time - startTime;
        float t = Mathf.Clamp01(elapsed / moveDuration);
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);

        if (t >= 1f)
        {
            // Reached the target
            // Optionally destroy or mark as hittable
        }
    }
}