using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    
    [Header("Tutorial Settings")]
    public int requiredKillsForPortal = 3;  // Number of kills required to activate portal
    public GameObject portalTuto;           // Reference to the portal
    private int enemiesKilled = 0;          // Track how many enemies have been killed
    
    [Header("Boss Settings")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public bool bossSpawned = false;
    
    // Event for enemy count changes
    public event System.Action OnEnemyCountChanged;
    
    private int enemiesRemaining;
    
    // Public property to access enemiesRemaining
    public int EnemiesRemaining 
    { 
        get { return enemiesRemaining; }
        private set 
        { 
            enemiesRemaining = value;
            OnEnemyCountChanged?.Invoke();
        }
    }
    
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
    
    void Start()
    {
        // Count all enemy spawners in the scene
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
        EnemiesRemaining = spawners.Length;
        
        // Disable portal at start
        if (portalTuto != null)
        {
            portalTuto.SetActive(false);
        }
    }
    
    // Called by individual spawners when their enemy is killed
    public void RegisterKill()
    {
        enemiesKilled++;
        EnemiesRemaining--;
        
        Debug.Log($"Enemy killed! Total: {enemiesKilled}, Remaining: {EnemiesRemaining}");
        
        // Check if we've killed enough enemies to activate the portal
        if (enemiesKilled >= requiredKillsForPortal && portalTuto != null)
        {
            portalTuto.SetActive(true);
        }
    }
    
    void SpawnBoss()
    {
        if (bossPrefab == null || bossSpawnPoint == null) return;
        
        bossSpawned = true;
        Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
    }
}