using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBeatMap", menuName = "Rhythm/Beat Map Data")]
public class BeatMapData : ScriptableObject
{
    public AudioClip audioClip;
    public float bpm;
    public float offset;
    public List<float> beatTimes = new List<float>();
}