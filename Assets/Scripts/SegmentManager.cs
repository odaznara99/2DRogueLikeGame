using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentManager : MonoBehaviour
{
    public GameObject[] segmentPrefabs;
    public Transform player;
    public int maxSegments = 5;
    private List<GameObject> activeSegments = new List<GameObject>();
    private Vector3 nextSpawnPoint;

    void Start()
    {
        // Spawn initial segments
        for (int i = 0; i < maxSegments; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        if (PlayerReachedNextSegment())
        {
            SpawnSegment();
            RemoveOldSegment();
        }
    }

    void SpawnSegment()
    {
        int randomIndex = Random.Range(0, segmentPrefabs.Length);
        GameObject newSegment = Instantiate(segmentPrefabs[randomIndex], nextSpawnPoint, Quaternion.identity);
        activeSegments.Add(newSegment);

        // Update next spawn point based on the end point of the new segment
        nextSpawnPoint = newSegment.GetComponent<Segment>().endPoint.position;
    }

    void RemoveOldSegment()
    {
        if (activeSegments.Count > maxSegments)
        {
            Destroy(activeSegments[0]);
            activeSegments.RemoveAt(0);
        }
    }

    bool PlayerReachedNextSegment()
    {
        // Define how close the player must be to the end point to spawn the next segment
        return player.position.x > nextSpawnPoint.x - 10f;
    }
}
    

