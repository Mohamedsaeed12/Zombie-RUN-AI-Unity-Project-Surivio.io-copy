using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int maxZombies;
    public float spawnInterval;
    public float spawnRadius;
    private int currentZombieCount;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    private IEnumerator SpawnZombies()
    {
        while (true)  // Run this loop forever
        {
            if (GameObject.FindGameObjectsWithTag("Zombie").Length < maxZombies)  // Count zombies by tag
            {
                Vector3 spawnPosition = GetRandomNavMeshLocation();
                Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            }
            yield return new WaitForSeconds(spawnInterval);  // Wait for defined interval
        }
    }

    public Vector3 GetRandomNavMeshLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius + transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    // Call this method from the Zombie's Die method
    public void RegisterZombieDeath()
    {
        // No need to decrement count because we're using GameObject.FindGameObjectsWithTag to count zombies
    }


public void ZombieDied()
    {
        // Ensure the count doesn't go negative
        if (currentZombieCount > 0)
        {
            currentZombieCount--;
        }
    }

  
}
