using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI enemyCountText;
    public GameObject tutorialPanel;
    
    [Header("Tutorial States")]
    private bool hasMoved = false;
    private bool hasAttacked = false;
    
    private void Start()
    {
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
            enemyCountText.text = $"Enemies Remaining: {EnemyManager.Instance.enemies.Count}";
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