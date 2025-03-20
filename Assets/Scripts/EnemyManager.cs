using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    
    [Header("Enemy Settings")]
    public List<GameObject> enemies = new List<GameObject>();
    
    [Header("Portal Settings")]
    public GameObject portal;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Disable portal at start
        if (portal != null)
        {
            portal.SetActive(false);
        }
    }

    public void EnemyDied(GameObject enemy)
    {
        enemies.Remove(enemy);
        
        // Check if all enemies are defeated
        if (enemies.Count == 0 && portal != null)
        {
            portal.SetActive(true);
            Debug.Log("All enemies defeated! Portal is now accessible.");
        }
    }
} 