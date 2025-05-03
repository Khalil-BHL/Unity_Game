using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Add this for scene management

public class Player_Health : MonoBehaviour
{
    // Add this event
    public event System.Action<int> OnHealthChanged;
    
    public int currentHealth;
    public int maxHealth = 100;
    
    [Header("Game Over Settings")]
    public float restartDelay = 2f; // Delay before restarting
    public GameObject gameOverPanel; // Optional: Game over UI panel
    
    private void Start()
    {
        currentHealth = maxHealth;
        // Trigger initial health update
        OnHealthChanged?.Invoke(currentHealth);
        
        // Hide game over panel at start if it exists
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }
    
    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
            // Handle player death
            Die();
        }
        
        // Notify listeners about health change
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    private void Die()
    {
        // Handle player death
        Debug.Log("Player died!");
        
        // Show game over panel if it exists
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        // Disable player movement and combat
        var playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null) playerMovement.enabled = false;
        var playerCombat = GetComponent<Player_Combat>();
        if (playerCombat != null) playerCombat.enabled = false;
        
        // Start the restart coroutine
        StartCoroutine(RestartGame());
    }
    
    private IEnumerator RestartGame()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(restartDelay);
        
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    
    // Optional: Add a public method to restart immediately (for UI buttons)
    public void RestartGameImmediate()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}