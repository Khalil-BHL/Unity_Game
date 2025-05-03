using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private Animator animator;
    private PlayerMovement playerMovement;
    private Player_Combat playerCombat;
    private Rigidbody2D rb;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<Player_Combat>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        
        isDead = true;
        
        // Use the correct animation parameter name for death
        if (animator != null)
        {
            animator.SetTrigger("Death");  // Changed from "exit" to "Death"
        }
        
        // Disable player movement and combat
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        if (playerCombat != null)
        {
            playerCombat.enabled = false;
        }
        
        // Stop any movement
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
