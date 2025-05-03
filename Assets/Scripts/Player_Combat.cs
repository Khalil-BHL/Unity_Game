using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    public static Player_Combat Instance { get; private set; }
    
    // Add event for player attack
    public event System.Action OnPlayerAttacked;
    
    public Transform attackPoint;
    public float weaponRange = 1;
    public float knockbackForce = 50;
    public float knockbackTime = .15f;
    public float stunTime = .3f;
    public LayerMask enemyLayer;
    public int damage = 1;

    public Animator anim;
    public float coooldown = 2;
    private float timer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        // Trigger attack event
        OnPlayerAttacked?.Invoke();
        
        if (timer <= 0)
        {
            anim.SetBool("isAttacking", true);
            
            timer = coooldown;
        }
    }

    void DealDamage()
    {
        // First check if attackPoint exists
        if (attackPoint == null)
        {
            Debug.LogError("Attack Point is not assigned in Player_Combat!");
            return;
        }

        // Get all colliders in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);

        // Debug log to check if we're detecting anything
        if (hitEnemies.Length > 0)
        {
            Debug.Log($"Hit {hitEnemies.Length} enemies");
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy == null) continue;

            // Try to get BossEnemy component
            BossEnemy bossEnemy = enemy.GetComponent<BossEnemy>();
            if (bossEnemy != null)
            {
                bossEnemy.TakeDamage(damage);
                continue;
            }

            // Try to get regular Enemy_Health component
            Enemy_Health enemyHealth = enemy.GetComponent<Enemy_Health>();
            if (enemyHealth != null)
            {
                enemyHealth.ChangeHealth(-damage);  // Changed from TakeDamage to ChangeHealth with negative damage
            }
        }
    }

    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false);
    }
}
