using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGame : MonoBehaviour
{
    [Header("UI Reference")]
    public Button restartButton;

    private void Start()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartScene);
        else
            Debug.LogWarning("Restart button not assigned in Inspector!");
    }

    public void RestartScene()
    {
        // Optional: reset any persistent data if needed
        Time.timeScale = 1f; // Just in case the game was paused

        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
