using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject[] platforms;
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;

    void Start()
    {
        SpawnPlatforms();
        SpawnEnemies();
    }

    void SpawnPlatforms()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (Random.value > 0.5f)
            {
                int randomIndex = Random.Range(0, platforms.Length);
                Instantiate(platforms[randomIndex], spawnPoint.position, Quaternion.identity);
            }
        }
    }

    void SpawnEnemies()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (Random.value > 0.7f)
            {
                Instantiate(enemyPrefab, spawnPoint.position + new Vector3(0, 1, 0), Quaternion.identity);
            }
        }
    }
}

