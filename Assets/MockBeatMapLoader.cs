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
    private bool isAudioReady = false; // To check if audio is ready

    void Start()
    {
        // Start audio and set up the beat detection system
        audioSource.Play();
        isAudioReady = true; // Assume audio is ready, but you may want to ensure this using a timer if needed
        InvokeRepeating("GenerateNotesFromBeats", 1f, 0.1f); // Start generating notes after a small delay
    }

    void Update()
    {
        if (isAudioReady && beatDetector.GetDetectedBeats().Count == 0)
        {
            Debug.Log("[MockBeatMapLoader] Audio is playing and beat detection should start.");
        }
    }

    void GenerateNotesFromBeats()
    {
        var detectedBeats = beatDetector.GetDetectedBeats();
        if (detectedBeats.Count == 0) return;

        float songTime = audioSource.time; // ⬅ Always use this

        foreach (var beatTime in detectedBeats)
        {
            if (beatTime < lastSpawnedNoteTime + minNoteGap)
                continue;

            LaneSide lane = spawnLeft ? LaneSide.Left : LaneSide.Right;
            spawnLeft = !spawnLeft;

            // Calculate the time (in song time) when this note should reach the player
            float hitTime = beatTime + startOffset + spawner.noteTravelTime;

            spawner.beatNotes.Add(new BeatNote
            {
                time = hitTime,  // song time when it should be hit
                lane = lane
            });

            lastSpawnedNoteTime = beatTime;
            Debug.Log($"[MockBeatMapLoader] BeatTime (song): {beatTime:F2}, Target HitTime: {hitTime:F2}");

            Debug.Log($"[Note] Scheduled note for {hitTime:F2}s (song time) on lane {lane}");
        }

        detectedBeats.Clear();
    }


}
