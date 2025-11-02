using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour
{
    [Header("UI Reference")]
    public Button startButton;

    [Header("Scene Settings")]
    [Tooltip("Name of the game scene to load")]
    public string gameSceneName = "_ThrowableHomeScene";

    private void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(LoadGame);
        else
            Debug.LogWarning("Start button is not assigned in the Inspector!");
    }

    public void LoadGame()
    {
        Time.timeScale = 1f;

        Resources.UnloadUnusedAssets();

        // Optional: destroy all persistent objects that shouldn’t carry over
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.scene.name != null && obj.scene.name != SceneManager.GetActiveScene().name)
                continue;
            if (obj.hideFlags == HideFlags.None)
                Destroy(obj);
        }

        // Load the target scene (completely replaces the current one)
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }
}
