using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Complete UI")]
    public GameObject levelCompletePanel;
    public TextMeshProUGUI levelCompleteText;
    public float displayDuration = 3f;
    public string nextLevelName; // Leave empty if this is the final level
    
    private void OnEnable()
    {
        // Subscribe to the boss defeated event
        Debug.Log("LevelManager: Subscribing to OnBossDefeated event");
        Boss_Health.OnBossDefeated += HandleBossDefeated;
    }
    
    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        Boss_Health.OnBossDefeated -= HandleBossDefeated;
    }
    
    private void Start()
    {
        // Hide the level complete panel at start
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("LevelManager: levelCompletePanel reference is missing!");
        }
    }
    
    private void HandleBossDefeated()
    {
        Debug.Log("LevelManager: Boss defeated event received!");
        StartCoroutine(ShowLevelComplete());
    }
    
    private IEnumerator ShowLevelComplete()
    {
        Debug.Log("LevelManager: Showing level complete panel");
        // Show the level complete panel
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
            
            // Wait for the specified duration
            yield return new WaitForSeconds(displayDuration);
            
            // Load the next level or return to menu
            if (!string.IsNullOrEmpty(nextLevelName))
            {
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                // This is the final level, you might want to show credits or return to main menu
                SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
            }
        }
        else
        {
            Debug.LogError("LevelManager: levelCompletePanel is null!");
        }
    }
}