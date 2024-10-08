using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Transform player;           // Reference to the player transform
    public Vector2 parallaxFactor;     // Determines how much each layer should move in relation to the player
    private Vector3 lastPlayerPosition; // The player's position in the previous frame

    void Start()
    {
        //Get reference from the player transform
        player = GameObject.Find("HeroKnight").GetComponent<Transform>();

        // Initialize lastPlayerPosition with the player's starting position
        lastPlayerPosition = player.position;

        // Assuming the background has a Renderer with a Sprite, calculate the size of the texture
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
      
    }

    void Update()
    {
        // Calculate how much the player has moved since the last frame
        Vector3 deltaMovement = player.position - lastPlayerPosition;

        // Move the background based on the player's movement, reduced by the parallaxFactor
        transform.position += new Vector3(deltaMovement.x * parallaxFactor.x, deltaMovement.y * parallaxFactor.y, 0);

        // Update lastPlayerPosition for the next frame
        lastPlayerPosition = player.position;

    }
}
