// MockBeatMapLoader.cs
using UnityEngine;

public class MockBeatMapLoader : MonoBehaviour
{
    public NoteSpawner spawner;
    public AudioSource audioSource;
    public AudioBeatDetector beatDetector;
    public float startOffset = 1f;

    private bool spawnLeft = true; // Toggle between lanes
    public float minNoteGap = 0.5f; // Minimum gap between notes in seconds
    private float lastSpawnedNoteTime = -999f; // Keeps track of last note time
    void Start()
    {
        audioSource.Play();
        InvokeRepeating("GenerateNotesFromBeats", 0f, 0.1f);
    }

    void GenerateNotesFromBeats()
    {
        var detectedBeats = beatDetector.GetDetectedBeats();
        if (detectedBeats.Count == 0) return;

        float currentTime = Time.timeSinceLevelLoad;

        foreach (var beatTime in detectedBeats)
        {
            // Skip if too soon since last note
            if (beatTime < lastSpawnedNoteTime + minNoteGap)
                continue;

            LaneSide lane = spawnLeft ? LaneSide.Left : LaneSide.Right;
            spawnLeft = !spawnLeft;

            spawner.beatNotes.Add(new BeatNote
            {
                time = beatTime + startOffset,
                lane = lane
            });

            lastSpawnedNoteTime = beatTime; // Update the last time we spawned a note

            Debug.Log($"[Note] Spawned beat at {beatTime + startOffset:F2}s on lane {lane}");
        }

        detectedBeats.Clear();
    }

}
