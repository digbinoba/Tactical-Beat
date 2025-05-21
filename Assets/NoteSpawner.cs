// NoteSpawner.cs
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;  // Prefab for the note
    public Transform leftLane;     // Left lane position
    public Transform rightLane;    // Right lane position
    public float noteTravelTime = 2f; // Time it takes for notes to reach the player
    public List<BeatNote> beatNotes = new List<BeatNote>();  // List of notes to spawn

    void Start()
    {
        // Could potentially clear old notes
        // for (int i = 0; i < beatNotes.Count; i++)
        // {
        //     Destroy(beatNotes[i].gameObject);
        // }
    }

    void Update()
    {
        SpawnNotes();
    }

    // Function to spawn notes at the correct time
    void SpawnNotes()
    {
        float songTime = GetSongTime();  // Get current song time

        // Loop through all the notes that need to be spawned
        for (int i = beatNotes.Count - 1; i >= 0; i--)
        {
            BeatNote note = beatNotes[i];

            if (note.time <= songTime)
            {
                // Determine which lane the note should spawn in
                Transform laneTransform = (note.lane == LaneSide.Left) ? leftLane : rightLane;

                // Log the spawning process
                Debug.Log($"Spawning note at time: {note.time}, Lane: {note.lane}");

                // Instantiate the note and move it towards the player
                GameObject noteInstance = Instantiate(notePrefab, laneTransform.position, Quaternion.identity);
                NoteMover noteMover = noteInstance.GetComponent<NoteMover>();
                noteMover.Initialize(note.time, songTime, noteTravelTime);

                // Remove the note from the list once spawned
                beatNotes.RemoveAt(i);
            }
        }
    }


    // Function to get the current song time
    float GetSongTime()
    {
        return Time.timeSinceLevelLoad;  // Assuming the song starts right away
    }
}