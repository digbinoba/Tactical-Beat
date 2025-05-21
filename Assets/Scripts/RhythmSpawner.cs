using UnityEngine;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;

public class RhythmSpawner : MonoBehaviour
{
    [Header("References")]
    public AudioSource musicAudioSource;
    public GameObject enemyPrefab;
    public Transform enemyTargetZone;
    public SpawnPointCollector spawnPointCollector;

    [Header("Beat Map ScriptableObject")]
    public BeatMapData beatMapData;

    [Header("Enemy Travel Settings")]
    public float spawnBuffer = 0.1f;

    [Header("Alternating Spawn Points")]
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;

    private bool spawnLeftNext = true;
    private int nextBeatIndex = 0;
    private bool sceneReady = false;

    void Start()
    {
        if (beatMapData == null || beatMapData.beatTimes.Count == 0)
        {
            Debug.LogError("[RhythmSpawner] BeatMapData not assigned or empty.");
            return;
        }

        if (musicAudioSource == null)
        {
            Debug.LogError("[RhythmSpawner] Missing AudioSource.");
            return;
        }

        // MRUK scene loading check
        if (MRUK.Instance != null)
        {
            MRUK.Instance.RegisterSceneLoadedCallback(() =>
            {
                Debug.Log("[RhythmSpawner] MRUK scene loaded.");
                Invoke(nameof(InitializeSpawner), spawnBuffer);
            });
        }
        else
        {
            Debug.LogWarning("[RhythmSpawner] MRUK not found. Initializing anyway.");
            InitializeSpawner();
        }
    }

    void InitializeSpawner()
    {
        if (spawnPointCollector != null && spawnPointCollector.spawnTransforms.Count > 0)
        {
            Debug.Log($"[RhythmSpawner] {spawnPointCollector.spawnTransforms.Count} spawn points found.");
        }

        if (musicAudioSource != null)
        {
            musicAudioSource.clip = beatMapData.audioClip;
            musicAudioSource.PlayDelayed(beatMapData.offset);
        }

        Debug.Log($"[RhythmSpawner] Initialized with beat map of {beatMapData.beatTimes.Count} beats.");
        sceneReady = true;
    }

    void Update()
    {
        if (!sceneReady || musicAudioSource == null || nextBeatIndex >= beatMapData.beatTimes.Count)
            return;

        float songTime = musicAudioSource.time;
        float targetBeatTime = beatMapData.beatTimes[nextBeatIndex];
        float spawnTime = targetBeatTime - 0.01f; // Minimal buffer for precision

        if (songTime >= spawnTime)
        {
            SpawnEnemyForBeat(targetBeatTime);
            nextBeatIndex++;
        }
    }

    void SpawnEnemyForBeat(float beatTime)
    {
        if (enemyPrefab == null || enemyTargetZone == null)
            return;

        Transform spawnPoint = spawnLeftNext ? leftSpawnPoint : rightSpawnPoint;
        spawnLeftNext = !spawnLeftNext; // Alternate side

        if (spawnPoint == null)
        {
            Debug.LogWarning("[RhythmSpawner] One of the spawn points is not assigned.");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        HomingEnemy homing = enemy.GetComponent<HomingEnemy>();
        if (homing != null)
        {
            homing.target = enemyTargetZone;

            float distance = Vector3.Distance(spawnPoint.position, enemyTargetZone.position);
            float timeUntilImpact = beatTime - musicAudioSource.time;
            timeUntilImpact = Mathf.Max(timeUntilImpact, 0.01f); // Prevent divide by zero

            //homing.moveDuration = travelTime;
            homing.elapsedTime = 0f;
            homing.startPosition = spawnPoint.position;

            Debug.Log($"[RhythmSpawner] Spawned enemy at {(spawnLeftNext ? "Right" : "Left")} | Beat: {beatTime:F2}s | Speed:");
        }
        else
        {
            Debug.LogWarning("[RhythmSpawner] Enemy prefab missing HomingEnemy script.");
        }
    }
}
