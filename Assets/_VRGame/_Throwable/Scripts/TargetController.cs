using System.Collections;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;
    public float targetLifetime = 5f;

    private void Start()
    {
        StartCoroutine(SpawnTargets());
    }

    private IEnumerator SpawnTargets()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnTarget();
        }
    }

    private void SpawnTarget()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        GameObject target = Instantiate(targetPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        Destroy(target, targetLifetime);
    }
}
