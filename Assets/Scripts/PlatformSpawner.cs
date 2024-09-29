using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject[] platformsPrefabs;    
    public Transform[]  platformSpawnPoints;
    [SerializeField, Range(0f, 1f), Tooltip("Platform spawn rate as a percentage. Value between 0 (0%) and 1 (100%).")] 
    float platformRate;

    public GameObject[]   enemyPrefabs;
    public Transform[]    enemySpawnPoints;
    [SerializeField, Range(0f, 1f), Tooltip("Enemy spawn rate as a percentage. Value between 0 (0%) and 1 (100%).")] 
    float enemyRate;
    

    void Start()
    {
        //SpawnPlatforms();
        SpawnEnemies();
    }

    void SpawnPlatforms()
    {
        foreach (Transform spawnPoint in platformSpawnPoints)
        {
            if (Random.value <= platformRate)
            {
                int randomIndex = Random.Range(0, platformsPrefabs.Length);
                Instantiate(platformsPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
            }
        }
    }

    void SpawnEnemies()
    {
        foreach (Transform spawnPoint in enemySpawnPoints)
        {
            if (Random.value <= enemyRate)
            {
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                Instantiate(enemyPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
            }
        }
    }
}

