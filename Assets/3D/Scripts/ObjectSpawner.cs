using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Prefab to spawn")]
    public GameObject spawnPrefab;

    [Tooltip("How many seconds between spawns")]
    public float spawnRate = 1f;

    [Tooltip("Maximum number of spawned objects (0 for unlimited)")]
    public int maxObjects = 10;

    [Tooltip("Should spawning happen automatically?")]
    public bool autoSpawn = true;

    [Header("Position Settings")]
    [Tooltip("Spawn at spawner's position")]
    public bool useSpawnerPosition = true;

    [Tooltip("Custom spawn position (if not using spawner's position)")]
    public Vector3 spawnPosition;

    [Tooltip("Random position offset range")]
    public Vector3 randomOffset = Vector3.zero;

    [Header("Rotation Settings")]
    [Tooltip("Spawn with spawner's rotation")]
    public bool useSpawnerRotation = true;

    [Tooltip("Custom rotation (if not using spawner's rotation)")]
    public Vector3 spawnRotation;

    [Tooltip("Random rotation offset range")]
    public Vector3 randomRotation = Vector3.zero;

    [Header("Parenting")]
    [Tooltip("Parent spawned objects to the spawner")]
    public bool parentToSpawner = false;

    [Header("Debug")]
    [Tooltip("Show debug messages")]
    public bool debugMode = false;

    private float nextSpawnTime = 0f;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        if (spawnPrefab == null)
        {
            Debug.LogError("Spawn prefab is not assigned!");
            enabled = false;
            return;
        }

        if (autoSpawn)
        {
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void Update()
    {
        if (!autoSpawn || spawnPrefab == null) return;

        if (Time.time >= nextSpawnTime)
        {
            TrySpawnObject();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    /// <summary>
    /// Attempts to spawn an object if conditions are met
    /// </summary>
    public void TrySpawnObject()
    {
        if (maxObjects > 0 && spawnedObjects.Count >= maxObjects)
        {
            if (debugMode) Debug.Log("Max objects reached, not spawning.");
            return;
        }

        SpawnObject();
    }

    /// <summary>
    /// Spawns an object immediately
    /// </summary>
    public GameObject SpawnObject()
    {
        // Calculate position
        Vector3 position = useSpawnerPosition ? transform.position : spawnPosition;
        position += new Vector3(
            Random.Range(-randomOffset.x, randomOffset.x),
            Random.Range(-randomOffset.y, randomOffset.y),
            Random.Range(-randomOffset.z, randomOffset.z)
        );

        // Calculate rotation
        Quaternion rotation = useSpawnerRotation ? transform.rotation : Quaternion.Euler(spawnRotation);
        rotation *= Quaternion.Euler(
            Random.Range(-randomRotation.x, randomRotation.x),
            Random.Range(-randomRotation.y, randomRotation.y),
            Random.Range(-randomRotation.z, randomRotation.z)
        );

        // Instantiate the object
        GameObject newObj = Instantiate(spawnPrefab, position, rotation);

        // Parent if needed
        if (parentToSpawner)
        {
            newObj.transform.SetParent(transform);
        }

        // Track the object
        spawnedObjects.Add(newObj);

        // Clean up null references (in case objects are destroyed elsewhere)
        spawnedObjects.RemoveAll(item => item == null);

        if (debugMode) Debug.Log($"Spawned new object at {position}. Total: {spawnedObjects.Count}");

        return newObj;
    }

    /// <summary>
    /// Destroys all spawned objects
    /// </summary>
    public void ClearAllSpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear();

        if (debugMode) Debug.Log("Cleared all spawned objects");
    }

    /// <summary>
    /// Destroys the oldest spawned object
    /// </summary>
    public void RemoveOldestObject()
    {
        if (spawnedObjects.Count == 0) return;

        GameObject oldest = spawnedObjects[0];
        if (oldest != null)
        {
            Destroy(oldest);
        }
        spawnedObjects.RemoveAt(0);

        if (debugMode) Debug.Log("Removed oldest object");
    }

    /// <summary>
    /// Returns the number of currently spawned objects
    /// </summary>
    public int GetSpawnedObjectCount()
    {
        spawnedObjects.RemoveAll(item => item == null);
        return spawnedObjects.Count;
    }
}