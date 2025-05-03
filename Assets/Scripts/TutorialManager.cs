using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI enemyCountText;
    public GameObject tutorialPanel;
    
    [Header("Portal References")]
    public GameObject portalTuto;
    
    [Header("Tutorial States")]
    private bool hasMoved = false;
    private bool hasAttacked = false;
    private bool enemiesDefeated = false;
    private int requiredEnemyCount = 3;
    
    private void Start()
    {
        // Disable portal at start
        if (portalTuto != null)
        {
            portalTuto.SetActive(false);
        }
        
        // Show initial movement tutorial
        ShowTutorial("Use WASD to move");
        
        // Subscribe to player movement
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.OnPlayerMoved += HandlePlayerMoved;
        }
        
        // Subscribe to player attack
        if (Player_Combat.Instance != null)
        {
            Player_Combat.Instance.OnPlayerAttacked += HandlePlayerAttacked;
        }
        
        // Subscribe to enemy count changes
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.OnEnemyCountChanged += UpdateEnemyCount;
        }
    }
    
    private void HandlePlayerMoved()
    {
        if (!hasMoved)
        {
            hasMoved = true;
            ShowTutorial("Press K to attack");
        }
    }
    
    private void HandlePlayerAttacked()
    {
        if (!hasAttacked)
        {
            hasAttacked = true;
            ShowTutorial("Defeat all enemies to proceed");
            ShowEnemyCount();
        }
    }
    
    private void ShowTutorial(string message)
    {
        if (tutorialText != null)
        {
            tutorialText.text = message;
            tutorialPanel.SetActive(true);
        }
    }
    
    private void ShowEnemyCount()
    {
        if (enemyCountText != null)
        {
            enemyCountText.gameObject.SetActive(true);
            UpdateEnemyCount();
        }
    }
    
    private void UpdateEnemyCount()
    {
        if (enemyCountText != null && EnemyManager.Instance != null)
        {
            int remainingEnemies = EnemyManager.Instance.EnemiesRemaining;
            enemyCountText.text = $"Enemies Remaining: {remainingEnemies}";
            
            // Check if all required enemies are defeated
            if (remainingEnemies <= 0 && !enemiesDefeated)
            {
                enemiesDefeated = true;
                ActivatePortal();
                ShowTutorial("Portal activated! Enter to proceed.");
            }
        }
    }
    
    private void ActivatePortal()
    {
        if (portalTuto != null)
        {
            portalTuto.SetActive(true);
        }
    }
    
    private void OnDestroy()
    {
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.OnPlayerMoved -= HandlePlayerMoved;
        }
        if (Player_Combat.Instance != null)
        {
            Player_Combat.Instance.OnPlayerAttacked -= HandlePlayerAttacked;
        }
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.OnEnemyCountChanged -= UpdateEnemyCount;
        }
    }
}