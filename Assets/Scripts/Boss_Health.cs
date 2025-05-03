using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Health : MonoBehaviour
{
    // Add this static event
    public static event System.Action OnBossDefeated;
    
    public int currentHealth;
    public int maxHealth = 500;
    
    private void Start()
    {
        currentHealth = maxHealth;
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
            // Boss is defeated
            Die();
        }
    }
    
    private void Die()
    {
        Debug.Log("Boss defeated! Triggering event.");
        
        // Trigger the boss defeated event
        OnBossDefeated?.Invoke();
        
        // Destroy the boss
        Destroy(gameObject);
    }
}