using System.Collections;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("TMP text displaying remaining time.")]
    public TMP_Text timerText;

    [Header("Timer Settings")]
    [Tooltip("Set total time in seconds.")]
    public float timeLimit = 60f;

    [Header("Objects to Disable When Time Ends")]
    public GameObject[] objectsToDeactivate;

    [Header("Objects to Enable When Time Ends")]
    public GameObject[] objectsToActivate;

    [Header("Optional UI for End of Timer")]
    public GameObject gameOverUI;

    private float timeRemaining;
    private bool isTimerFinished;

    private void Start()
    {
        timeRemaining = timeLimit;
        isTimerFinished = false;
        if (gameOverUI) gameOverUI.SetActive(false);

        StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime; // Runs with real time (not affected by timescale)
            UpdateTimerUI();
            yield return null;
        }

        EndTimer();
    }

    private void UpdateTimerUI()
    {
        if (timerText)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            seconds = Mathf.Max(0, seconds);
            timerText.text = $"Time: {seconds}";
        }
    }

    private void EndTimer()
    {
        if (isTimerFinished) return;
        isTimerFinished = true;
        timeRemaining = 0f;
        UpdateTimerUI();

        // Activate/Deactivate relevant objects
        foreach (GameObject obj in objectsToDeactivate)
            if (obj) obj.SetActive(false);

        foreach (GameObject obj in objectsToActivate)
            if (obj) obj.SetActive(true);

        // Show GameOver UI if assigned
        if (gameOverUI) gameOverUI.SetActive(true);

        Debug.Log("⏱ Timer completed!");
    }

    public void ResetTimer(float newTimeLimit = -1f)
    {
        if (newTimeLimit > 0)
            timeLimit = newTimeLimit;

        StopAllCoroutines();
        timeRemaining = timeLimit;
        isTimerFinished = false;

        if (gameOverUI) gameOverUI.SetActive(false);
        foreach (GameObject obj in objectsToDeactivate)
            if (obj) obj.SetActive(true);
        foreach (GameObject obj in objectsToActivate)
            if (obj) obj.SetActive(false);

        StartCoroutine(TimerRoutine());
    }

    public bool IsFinished => isTimerFinished;
    public float TimeRemaining => timeRemaining;
}
