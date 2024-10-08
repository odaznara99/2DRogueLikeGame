using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    public BoxCollider2D backgroundCollider;  // Reference to the background's BoxCollider2D
    private float backgroundWidth;             // Width of the background

    void Start()
    {
        // Get the BoxCollider2D component attached to the parent object
       // backgroundCollider = GameObject.Find("Background_Parent").GetComponent<BoxCollider2D>();

        // Calculate the width of the background based on the collider's size
        backgroundWidth = backgroundCollider.size.x;
    }

    void Update()
    {
        // Check if the background has moved beyond its own width
        if (transform.position.x < -backgroundWidth)
        {
            RepositionBackground();
        }
    }

    // Reposition the background to create a seamless repeat
    private void RepositionBackground()
    {
        Debug.Log("Reposition BG");
        Vector2 groundOffset = new Vector2(backgroundWidth * 2f, 0);
        transform.position = (Vector2)transform.position + groundOffset;
    }
}
