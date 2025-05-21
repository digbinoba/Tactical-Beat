using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class BeatMapGenerator : EditorWindow
{
    private AudioClip musicClip;
    private float bpm = 120f;
    private float offset = 0f;
    private int beatLimit = 100;
    private const int sampleRate = 44100; // Standard sample rate for audio clips

    private List<float> beatMap = new List<float>();

    [MenuItem("Tools/Beat Map Generator")]
    public static void ShowWindow()
    {
        GetWindow<BeatMapGenerator>("Beat Map Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Generate Beat Map", EditorStyles.boldLabel);

        musicClip = (AudioClip)EditorGUILayout.ObjectField("Music Clip", musicClip, typeof(AudioClip), false);
        bpm = EditorGUILayout.FloatField("BPM", bpm);
        offset = EditorGUILayout.FloatField("Offset (seconds)", offset);
        beatLimit = EditorGUILayout.IntField("Max Beats", beatLimit);

        if (GUILayout.Button("Generate Beat Map"))
        {
            GenerateBeatMap();
        }
    }

    void GenerateBeatMap()
    {
        if (musicClip == null)
        {
            Debug.LogError("No music clip assigned.");
            return;
        }

        // First, we extract audio samples
        AudioSource audioSource = new AudioSource();
        audioSource.clip = musicClip;
        float[] samples = new float[musicClip.samples];
        musicClip.GetData(samples, 0);

        // Process the audio to detect beats
        DetectBeats(samples);

        // Generate Beat Map based on detected beats
        Debug.Log($"[BeatMapGenerator] Generated {beatMap.Count} beats.");

        // Copy to clipboard for easy pasting
        string result = "beatTimes = new List<float> {\n    " + string.Join("f, ", beatMap.Select(bt => bt.ToString("F3")).ToArray()) + "f\n};";
        EditorGUIUtility.systemCopyBuffer = result;

        Debug.Log("[BeatMapGenerator] Beat map copied to clipboard!");
    }

    void DetectBeats(float[] samples)
    {
        // Convert the sample data into an array of amplitudes or volume values.
        // Use FFT or simple averaging to detect peaks.
        int chunkSize = 1024; // Analyzing chunks of audio
        float threshold = 0.1f; // Minimum amplitude to consider a peak as a beat

        // Reset the beat map list
        beatMap.Clear();

        // Analyze audio in chunks
        for (int i = 0; i < samples.Length - chunkSize; i += chunkSize)
        {
            // Get the chunk of audio samples
            float[] chunk = new float[chunkSize];
            System.Array.Copy(samples, i, chunk, 0, chunkSize);

            // Calculate the RMS (Root Mean Square) to estimate the amplitude of the chunk
            float rms = CalculateRMS(chunk);

            // If the RMS value is above the threshold, we consider it a beat
            if (rms > threshold)
            {
                float time = i / (float)sampleRate;
                beatMap.Add(time);
            }
        }
    }

    float CalculateRMS(float[] samples)
    {
        float sum = 0f;
        foreach (var sample in samples)
        {
            sum += sample * sample; // Square each sample
        }
        return Mathf.Sqrt(sum / samples.Length); // Return the RMS value
    }
}
