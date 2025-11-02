using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObjectManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("List of throwable prefabs to randomly spawn")]
    public GameObject[] throwablePrefabs;

    [Tooltip("Available spawn points for prefabs")]
    public Transform[] spawnPoints;

    [Tooltip("Time interval between spawn attempts (seconds)")]
    public float spawnInterval = 5f;

    [Tooltip("Time before an idle object is destroyed (seconds)")]
    public float idleDestroyTime = 5f;

    // Track active objects at spawn points
    private Dictionary<Transform, GameObject> activeObjects = new Dictionary<Transform, GameObject>();

    private void Start()
    {
        foreach (Transform point in spawnPoints)
            activeObjects[point] = null;

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            TrySpawnAtEmptyPoints();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void TrySpawnAtEmptyPoints()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (activeObjects[spawnPoint] != null)
                continue; // skip occupied spawn points

            int randomIndex = Random.Range(0, throwablePrefabs.Length);
            GameObject prefab = throwablePrefabs[randomIndex];

            GameObject obj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            activeObjects[spawnPoint] = obj;

            // Ensure Rigidbody exists
            if (!obj.TryGetComponent<Rigidbody>(out var rb))
                rb = obj.AddComponent<Rigidbody>();

            // Add motion watcher
            MotionPhysicsHandler handler = obj.GetComponent<MotionPhysicsHandler>();
            if (handler == null)
                handler = obj.AddComponent<MotionPhysicsHandler>();

            handler.Initialize(this, spawnPoint, idleDestroyTime);
        }
    }

    // Called when the object is destroyed or fully gone
    public void NotifyObjectReleased(Transform spawnPoint)
    {
        if (spawnPoint != null && activeObjects.ContainsKey(spawnPoint))
            activeObjects[spawnPoint] = null;
    }
}

public class MotionPhysicsHandler : MonoBehaviour
{
    private ThrowableObjectManager manager;
    private Transform assignedSpawnPoint;
    private Vector3 lastPosition;
    private float idleTimer = 0f;
    private float idleDestroyTime;
    private const float positionCheckRate = 0.2f; // how often to check position

    public void Initialize(ThrowableObjectManager mgr, Transform spawnPoint, float idleTime)
    {
        manager = mgr;
        assignedSpawnPoint = spawnPoint;
        idleDestroyTime = idleTime;
    }

    private void Start()
    {
        lastPosition = transform.position;
        StartCoroutine(ActivityMonitor());
    }

    private IEnumerator ActivityMonitor()
    {
        while (true)
        {
            yield return new WaitForSeconds(positionCheckRate);

            if (this == null) yield break;

            // Measure movement
            float moveDistance = (transform.position - lastPosition).sqrMagnitude;

            // If moved, reset idle timer
            if (moveDistance > 0.0005f)
            {
                idleTimer = 0f;
            }
            else
            {
                idleTimer += positionCheckRate;
            }

            // Destroy only if idle for too long
            if (idleTimer >= idleDestroyTime)
            {
                manager?.NotifyObjectReleased(assignedSpawnPoint);
                Destroy(gameObject);
                yield break;
            }

            lastPosition = transform.position;
        }
    }

    private void OnDestroy()
    {
        manager?.NotifyObjectReleased(assignedSpawnPoint);
    }
}
