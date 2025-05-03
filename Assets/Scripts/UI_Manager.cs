using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [Header("Health Display")]
    public TextMeshProUGUI healthText;  // Drag your HP text here in the inspector
    
    private void Start()
    {
        // Find the player and subscribe to health changes
        Player_Health playerHealth = FindObjectOfType<Player_Health>();
        
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthDisplay;
        }
        else
        {
            Debug.LogError("Player_Health component not found!");
        }
    }
    
    // Update the health display
    public void UpdateHealthDisplay(int currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth.ToString();
            Debug.Log("Updated health display: " + currentHealth);
        }
    }
}