using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombies : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int maxZombies = 10;
    public float spawnTimeGap = 5f;
    public Transform[] spawnPoints;

    private List<GameObject> spawnedZombies = new List<GameObject>();
    private float lastSpawnTime = 0f;

    private void Update()
    {
        float currentTime = Time.time;

        float calcSpawnTimeGap = currentTime - lastSpawnTime;//reduce amount of zombies spawning at same time.

        if (spawnedZombies.Count < maxZombies && calcSpawnTimeGap >= spawnTimeGap)
        {
            lastSpawnTime = currentTime;
            SpawnZombie(); 
        }
    }

    private void SpawnZombie()
    {
        int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomSpawnPoint];

        // Instantiate a new zombie at the spawn point
        GameObject newZombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
        spawnedZombies.Add(newZombie);
    }
}