using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPoint.position, 0.5f);  // Start point of the segment
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endPoint.position, 0.5f);    // End point of the segment
    }
}
