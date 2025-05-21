// NoteMover.cs
using UnityEngine;

public class NoteMover : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float travelTime;
    private float spawnTime;

    public void Initialize(float noteTime, float currentSongTime, float travelTime)
    {
        this.travelTime = travelTime;
        this.spawnTime = currentSongTime;

        // Target is Z = 0 (where the player punches), starting Z is current
        startPosition = transform.position;
        targetPosition = new Vector3(startPosition.x, startPosition.y, 0f);
    }

    void Update()
    {
        float elapsed = Time.timeSinceLevelLoad - spawnTime;
        float t = Mathf.Clamp01(elapsed / travelTime);
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);

        if (t >= 1f)
        {
            Destroy(gameObject); // Auto destroy when it reaches the end
        }
    }
}
