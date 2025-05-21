using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public AudioSource songAudioSource;
    public float songTime => songAudioSource.time;

    void Start()
    {
        songAudioSource.Play();
    }
}