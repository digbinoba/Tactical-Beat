using UnityEngine;
using System.Collections.Generic;

public class AudioBeatDetector : MonoBehaviour
{
    public AudioSource audioSource;
    public float sensitivity = 1.0f;  // Sensitivity to beat detection (lower is more sensitive)
    public float minFrequency = 20f; // Lowest frequency to scan
    public float maxFrequency = 2000f; // Highest frequency to scan
    public float tempoVariance = 0.2f; // Allow tempo variation within +-20%

    private List<float> detectedBeats = new List<float>();
    private float previousTime = 0f;
    private float bpm = 120f;
    private float secondsPerBeat;
    private float timeSinceLastBeat = 0f;
    private float beatThreshold = 0.3f; // Volume threshold for detecting beats

    void Start()
    {
        secondsPerBeat = 60f / bpm;
    }

    void Update()
    {
        AnalyzeAudioForBeats();
    }

    // Analyze the audio data to detect beat peaks
    void AnalyzeAudioForBeats()
    {
        // Get frequency spectrum data from AudioSource
        float[] spectrum = new float[256];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // Sum frequencies within the desired range (for detecting beats)
        float energy = 0f;
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (i >= minFrequency && i < maxFrequency)
            {
                energy += spectrum[i];
            }
        }

        // Log the energy for debugging
        Debug.Log($"[AudioBeatDetector] Energy: {energy}");

        // If we detect a peak loud enough, we consider it a beat
        if (energy > beatThreshold && (Time.time - timeSinceLastBeat) > secondsPerBeat * (1 - tempoVariance) && (Time.time - timeSinceLastBeat) < secondsPerBeat * (1 + tempoVariance))
        {
            // It's a beat!
            detectedBeats.Add(audioSource.time);
            timeSinceLastBeat = Time.time;

            // Adjust tempo for future beats
            secondsPerBeat = Mathf.Lerp(secondsPerBeat, 60f / bpm, Time.deltaTime * 0.5f);  // Smooth tempo changes

            // Log when a beat is detected
            Debug.Log($"[AudioBeatDetector] Detected a beat at {Time.time:F2}");
        }
    }


    public List<float> GetDetectedBeats()
    {
        return detectedBeats;
    }

    // Use this function to adjust tempo based on detected beats or manual adjustments
    public void SetBPM(float bpm)
    {
        this.bpm = bpm;
        secondsPerBeat = 60f / bpm;
    }
}
