using UnityEngine;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit; // Make sure this is imported for MRUK

public class RhythmSpawner : MonoBehaviour
{
    [Header("Rhythm Settings")]
    public float bpm = 120f;
    public GameObject enemyPrefab;
    public float beatOffset = 0f;

    [Header("Target Zone")]
    public Transform enemyTargetZone;

    [Header("Spawn Control")]
    public SpawnPointCollector spawnPointCollector; // Reference to the component that gathers spawn points
    public float spawnDelayAfterSceneLoad = 0.2f; // buffer delay after scene loads before first beat

    private float beatInterval;
    private float nextBeatTime;
    private bool sceneReady = false;
    private List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        beatInterval = 60f / bpm;

        // Wait for MRUK scene to load before starting spawn logic
        if (MRUK.Instance != null)
        {
            MRUK.Instance.RegisterSceneLoadedCallback(() =>
            {
                Debug.Log("[RhythmSpawner] Scene loaded.");
                Invoke(nameof(InitializeSpawner), spawnDelayAfterSceneLoad);
            });
        }
        else
        {
            Debug.LogWarning("[RhythmSpawner] MRUK not found. Starting spawner anyway (dev mode).");
            InitializeSpawner();
        }
    }

    void InitializeSpawner()
    {
        spawnPoints = spawnPointCollector.spawnTransforms;

        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("[RhythmSpawner] No spawn points available.");
            return;
        }

        Debug.Log($"[RhythmSpawner] Starting beat-based spawning. Found {spawnPoints.Count} spawn points.");
        sceneReady = true;
        nextBeatTime = Time.time + beatOffset;
    }

    void Update()
    {
        if (!sceneReady || spawnPoints.Count == 0 || enemyPrefab == null || enemyTargetZone == null)
            return;

        if (Time.time >= nextBeatTime)
        {
            SpawnEnemyOnBeat();
            nextBeatTime += beatInterval;
        }
    }

    void SpawnEnemyOnBeat()
    {
        // Pick a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        HomingEnemy homing = enemy.GetComponent<HomingEnemy>();
        if (homing != null)
        {
            homing.target = enemyTargetZone;
            homing.moveDuration = beatInterval * Random.Range(3f, 6f); // Adjust this for timing
        }
        else
        {
            Debug.LogWarning("[RhythmSpawner] Spawned enemy missing HomingEnemy script.");
        }
    }
}
