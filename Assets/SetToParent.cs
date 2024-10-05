using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToParent : MonoBehaviour
{

    // for moving platform, attach this script so the player will follow
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player stepped onto the platform
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Make the player a child of the platform so it moves with it
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player left the platform
        if (other.CompareTag("Player") || other.CompareTag("Player"))
        {
            // Remove the player from being a child of the platform
            other.transform.SetParent(null);
        }
    }
}
