using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BeatMapGeneratorEditor : EditorWindow
{
    private AudioClip musicClip;
    private float bpm = 120f;
    private float offset = 0f;
    private int beatLimit = 100;
    private string assetName = "NewBeatMap";

    [MenuItem("Tools/Beat Map Generator")]
    public static void ShowWindow()
    {
        GetWindow<BeatMapGeneratorEditor>("Beat Map Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Generate and Save Beat Map", EditorStyles.boldLabel);

        musicClip = (AudioClip)EditorGUILayout.ObjectField("Music Clip", musicClip, typeof(AudioClip), false);
        bpm = EditorGUILayout.FloatField("BPM", bpm);
        offset = EditorGUILayout.FloatField("Offset (seconds)", offset);
        beatLimit = EditorGUILayout.IntField("Max Beats", beatLimit);
        assetName = EditorGUILayout.TextField("Asset Name", assetName);

        if (GUILayout.Button("Generate & Save Beat Map"))
        {
            GenerateAndSave();
        }
    }

    void GenerateAndSave()
    {
        if (musicClip == null)
        {
            Debug.LogError("Please assign a music clip.");
            return;
        }

        float interval = 60f / bpm;
        List<float> beatMap = new List<float>();
        float songLength = musicClip.length;

        for (float t = offset; t < songLength && beatMap.Count < beatLimit; t += interval)
        {
            beatMap.Add(t);
        }

        var asset = ScriptableObject.CreateInstance<BeatMapData>();
        asset.audioClip = musicClip;
        asset.bpm = bpm;
        asset.offset = offset;
        asset.beatTimes = beatMap;

        string path = $"Assets/BeatMaps/{assetName}.asset";

        Directory.CreateDirectory("Assets/Resources/BeatMaps");
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();

        Debug.Log($"[BeatMapGenerator] Saved beat map with {beatMap.Count} beats to: {path}");
    }
}
