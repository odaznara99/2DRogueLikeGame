using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // The maximum health the player can have
    private int currentHealth;  // The player's current health

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the player's health to the maximum at the start
        currentHealth = maxHealth;
        UpdateHealthUI(); // If you have a UI to display health, update it here
    }

    // Method to handle taking damage
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Make sure health doesn't go below zero
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        Debug.Log("Player took " + damageAmount + " damage. Current health: " + currentHealth);

        UpdateHealthUI(); // Update the UI to reflect the health change

        // Check if the player's health reaches zero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle healing the player (optional)
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        // Make sure health doesn't exceed the maximum
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        Debug.Log("Player healed " + healAmount + " points. Current health: " + currentHealth);

        UpdateHealthUI(); // Update the UI to reflect the health change
    }

    // Method called when the player's health reaches zero
    private void Die()
    {
        Debug.Log("Player died!");

        // Disable player controls, play a death animation, or trigger a respawn here
        // Example: Disable player movement
        GetComponent<HeroKnight>().enabled = false;

        // Optionally, you can trigger a death screen, restart the level, or respawn the player.
    }

    // Optional: Method to update the health UI
    private void UpdateHealthUI()
    {
        // Implement this method to update the player's health bar or any other UI element that displays health
        // For example, if using Unity UI:
        // healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}
