using UnityEngine;
using TMPro;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HighScoreData
{
    public int highestScore;
}

public class HighScoreDisplay : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Assign all TMP_Text fields that should display the high score.")]
    public List<TMP_Text> highScoreTexts = new List<TMP_Text>();

    private string persistentFilePath;
    private int lastScore = -1;

    private void Start()
    {
        persistentFilePath = Path.Combine(Application.persistentDataPath, "Level1Score.json");
        StartCoroutine(UpdateHighScoreRoutine());
    }

    private IEnumerator UpdateHighScoreRoutine()
    {
        while (true)
        {
            DisplayHighScore();
            yield return new WaitForSeconds(1f); // Check every second
        }
    }

    private void DisplayHighScore()
    {
        if (highScoreTexts == null || highScoreTexts.Count == 0)
        {
            Debug.LogWarning("[HighScoreDisplay] No TMP_Text references assigned!");
            return;
        }

        if (!File.Exists(persistentFilePath))
        {
            foreach (TMP_Text text in highScoreTexts)
                if (text != null) text.text = "Highest Score: 0";
            return;
        }

        try
        {
            string json = File.ReadAllText(persistentFilePath);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);

            if (data != null && data.highestScore != lastScore)
            {
                lastScore = data.highestScore;
                string display = "High Score: " + data.highestScore;

                foreach (TMP_Text text in highScoreTexts)
                {
                    if (text != null)
                        text.text = display;
                }

                Debug.Log($"[HighScoreDisplay] Updated display: {data.highestScore}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("[HighScoreDisplay] Error reading JSON: " + e.Message);
        }
    }
}
