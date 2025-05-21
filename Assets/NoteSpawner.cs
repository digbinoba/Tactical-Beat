using UnityEngine;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;

    public Transform leftLaneStart;
    public Transform rightLaneStart;

    public Transform leftLaneEnd;
    public Transform rightLaneEnd;

    public float noteTravelTime = 1.5f;

    public List<BeatNote> beatNotes = new List<BeatNote>();
    public AudioSource audioSource;

    private int nextNoteIndex = 0;

    void Start()
    {
        Debug.Log($"NoteSpawner started. Beat notes: {beatNotes.Count}");
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    void Update()
    {
        if (nextNoteIndex >= beatNotes.Count) return;

        float songTime = audioSource.time;
        Debug.Log($"Current Song Time: {songTime}");

        while (nextNoteIndex < beatNotes.Count &&
               beatNotes[nextNoteIndex].time - noteTravelTime <= songTime)
        {
            Debug.Log($"Spawning Note at Time: {beatNotes[nextNoteIndex].time}");
            SpawnNote(beatNotes[nextNoteIndex]);
            nextNoteIndex++;
        }
    }

    void SpawnNote(BeatNote beatNote)
    {
        Transform start = beatNote.lane == LaneSide.Left ? leftLaneStart : rightLaneStart;
        Transform end = beatNote.lane == LaneSide.Left ? leftLaneEnd : rightLaneEnd;

        GameObject noteObj = Instantiate(notePrefab, start.position, Quaternion.identity);

        NoteMover mover = noteObj.GetComponent<NoteMover>();
        if (mover != null)
        {
            mover.SetNoteTarget(start.position, end.position, noteTravelTime);
        }
    }
}