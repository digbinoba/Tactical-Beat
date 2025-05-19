using UnityEngine;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit; // Required for MRUK

public class SpawnPointCollector : MonoBehaviour
{
    public string spawnTag = "SpawnPoint";
    public Transform searchRoot; // Assign the parent where FindSpawnPositions places spawns
    public List<Transform> spawnTransforms = new List<Transform>();

    private void Start()
    {
        if (MRUK.Instance == null)
        {
            Debug.LogError("SpawnPointCollector: MRUK.Instance is null. Scene understanding might not be initialized.");
            return;
        }

        // Wait for scene load
        MRUK.Instance.RegisterSceneLoadedCallback(() =>
        {
            Debug.Log("SpawnPointCollector: Scene loaded, collecting spawn points...");
            CollectSpawnPoints();
        });
    }

    private void CollectSpawnPoints()
    {
        spawnTransforms.Clear();

        if (searchRoot == null)
        {
            var allTagged = GameObject.FindGameObjectsWithTag(spawnTag);
            foreach (var go in allTagged)
            {
                spawnTransforms.Add(go.transform);
            }
        }
        else
        {
            foreach (Transform child in searchRoot.GetComponentsInChildren<Transform>(true))
            {
                if (child.CompareTag(spawnTag))
                {
                    spawnTransforms.Add(child);
                }
            }
        }

        Debug.Log($"SpawnPointCollector: Collected {spawnTransforms.Count} spawn points.");
    }
}