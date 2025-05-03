using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public float activationDistance = 5f;
    public bool requireProximity = true;
    [Header("Spawn Settings")]
    public int spawnCount = 1;  // How many enemies this spawner will create
    public float spawnDelay = 1f;  // Delay between spawns
    
    private bool hasSpawned = false;
    private Transform player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        // If we don't require proximity, spawn immediately
        if (!requireProximity)
        {
            SpawnEnemy();
        }
    }
    
    void Update()
    {
        // If player reference is lost, try to find it again
        if (player == null)
        {
            Debug.Log("Player reference lost, trying to find player again");
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null) 
            {
                Debug.Log("Still can't find player");
                return; // Exit if still can't find player
            }
            Debug.Log("Player found again!");
        }
        
        // Skip if already spawned or proximity not required
        if (hasSpawned || !requireProximity)
            return;
            
        // Check distance to player
        float distance = Vector2.Distance(player.position, transform.position);
        Debug.Log($"Distance to player: {distance}, Activation distance: {activationDistance}");
        if (distance <= activationDistance)
        {
            Debug.Log("Spawning enemy!");
            StartCoroutine(SpawnWithDelay());
        }
    }
    
    IEnumerator SpawnWithDelay()
    {
        hasSpawned = true;
        yield return new WaitForSeconds(spawnDelay);
        SpawnEnemy();
    }
    
    void SpawnEnemy()
    {
        StartCoroutine(SpawnEnemies());
    }
    
    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            if (enemyPrefab != null)
            {
                GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                
                // Connect to the enemy death event
                Enemy_Health enemyHealth = enemy.GetComponent<Enemy_Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.OnEnemyDeath += OnEnemyKilled;
                }
            }
            else
            {
                Debug.LogError("Enemy prefab not assigned to spawner: " + gameObject.name);
            }
            
            // Wait before spawning the next enemy
            if (i < spawnCount - 1)
            {
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
    
    void OnEnemyKilled()
    {
        // Notify the EnemyManager about the kill
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.RegisterKill();
        }
    }
    
    // Visualize the activation range in the editor
    void OnDrawGizmosSelected()
    {
        if (requireProximity)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, activationDistance);
        }
    }

    // Add this public method to reset the spawner
    public void ResetSpawner()
    {
        hasSpawned = false;
        Debug.Log($"Spawner {gameObject.name} has been reset");
    }
}