using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class ScoreData
{
    public int highestScore;
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI")]
    public List<TMP_Text> scoreTexts;

    [Header("Audio & Effects")]
    public AudioSource hitAudioSource;
    public ParticleSystem hitEffect;

    [Header("Settings")]
    public int pointsPerHit = 10;

    private int score = 0;
    private int highestScore = 0;
    private string persistentFilePath;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        persistentFilePath = Path.Combine(Application.persistentDataPath, "Level1Score.json");

        // First-time setup: copy from StreamingAssets if not found
        if (!File.Exists(persistentFilePath))
        {
            string sourcePath = Path.Combine(Application.streamingAssetsPath, "Level1Score.json");
            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, persistentFilePath);
                Debug.Log($"[ScoreManager] Copied JSON from StreamingAssets to {persistentFilePath}");
            }
            else
            {
                File.WriteAllText(persistentFilePath, JsonUtility.ToJson(new ScoreData { highestScore = 0 }, true));
                Debug.LogWarning("[ScoreManager] Created new default Level1Score.json");
            }
        }

        LoadHighScore();
        UpdateScoreText();
    }

    public void AddScore(Vector3 hitPosition)
    {
        score += pointsPerHit;

        // Particle + Audio
        if (hitEffect != null)
        {
            var fx = Instantiate(hitEffect, hitPosition, Quaternion.identity);
            fx.Play();
            Destroy(fx.gameObject, 3f);
        }
        if (hitAudioSource != null && hitAudioSource.clip != null)
            hitAudioSource.Play();

        // Update high score if beaten
        if (score > highestScore)
        {
            highestScore = score;
            SaveHighScore();
        }

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreTexts == null) return;

        string display = $"Score: {score}";
        foreach (var txt in scoreTexts)
            if (txt != null) txt.text = display;
    }

    private void LoadHighScore()
    {
        try
        {
            string json = File.ReadAllText(persistentFilePath);
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);
            highestScore = data.highestScore;
            Debug.Log($"[ScoreManager] Loaded Highest Score: {highestScore}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("[ScoreManager] Error loading: " + e.Message);
        }
    }

    private void SaveHighScore()
    {
        try
        {
            string json = JsonUtility.ToJson(new ScoreData { highestScore = highestScore }, true);
            File.WriteAllText(persistentFilePath, json);
            Debug.Log($"[ScoreManager] Saved new High Score: {highestScore}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("[ScoreManager] Error saving: " + e.Message);
        }
    }
}
