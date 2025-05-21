using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaneSide { Left = 0, Right = 1 }

[System.Serializable]
public class BeatNote
{
    public float time;           // When the note should be hit
    public LaneSide lane;        // Left or Right lane
}