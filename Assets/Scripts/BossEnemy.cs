using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [Header("Boss Stats")]
    public float maxHealth = 200f;
    public float currentHealth;
    public float moveSpeed = 3f;
    public float attackDamage = 25f;
    public float attackRange = 2f;
    public float detectionRange = 10f;

    [Header("Attack Settings")]
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Initialize animator parameters
        animator.SetBool("isWalking", false);
        animator.ResetTrigger("attack");
        animator.ResetTrigger("hurt");
        animator.ResetTrigger("death");
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
    
        // Add this debug line
        Debug.DrawLine(transform.position, player.position, Color.red);
    
        // Move towards player when in detection range but outside attack range
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }

        // Attack when in range
        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
        }

        // Flip sprite based on player position
        if (player.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        animator.SetBool("isWalking", true);
    }

    void Attack()
    {
        // Reset previous attack animation
        animator.ResetTrigger("attack");
        
        // Set the new attack trigger
        animator.SetTrigger("attack");
        nextAttackTime = Time.time + attackCooldown;

        // Create a larger area for attack detection
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Player"));
        foreach (Collider2D player in hitPlayers)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    // Add this method to reset attack animation
    public void OnAttackAnimationEnd()
    {
        animator.ResetTrigger("attack");
    }

    // Add this method to visualize the attack range in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("death");
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
        Destroy(gameObject, 2f);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}