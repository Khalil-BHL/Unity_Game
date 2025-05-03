using UnityEngine;

public class PortalTuto : MonoBehaviour
{
    public Transform targetPortal; // Reference to the destination portal
    public GameObject tutorialPanel;
    public GameObject tutorialCanvas; // Add reference to the entire canvas
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Disable tutorial panel
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(false);
            }
            
            // Disable the entire tutorial canvas
            if (tutorialCanvas != null)
            {
                tutorialCanvas.SetActive(false);
            }
            
            // Teleport player to the target portal
            if (targetPortal != null && collision.transform != null)
            {
                // Offset slightly to prevent immediate re-triggering
                Vector3 targetPosition = targetPortal.position + new Vector3(0, 1f, 0);
                collision.transform.position = targetPosition;
                
                // Reset all spawners in the scene
                EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
                foreach (EnemySpawner spawner in spawners)
                {
                    spawner.ResetSpawner();
                }
            }
            else
            {
                Debug.LogWarning("Target portal not assigned or player transform is null!");
            }
        }
    }
}