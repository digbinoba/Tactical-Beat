// BeatMapGenerator.cs
using System.Collections.Generic;
using UnityEngine;

public static class BeatMapGenerator2
{
    public static List<BeatNote> GenerateFromBPM(float bpm, float songLength, float startOffset = 1f)
    {
        List<BeatNote> notes = new List<BeatNote>();
        float beatInterval = 60f / bpm;
        float currentBeatTime = startOffset;
        bool leftNext = true;

        while (currentBeatTime < songLength)
        {
            notes.Add(new BeatNote
            {
                time = currentBeatTime,
                lane = leftNext ? LaneSide.Left : LaneSide.Right
            });

            leftNext = !leftNext;
            currentBeatTime += beatInterval;
        }

        return notes;
    }
}