using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Image timerBackground;
    public TextMeshProUGUI timerText;
    public Button continueButton;

    private bool isChallengeActive;
    private float startTime;
    private float elapsedTime;

    private void Start()
    {
        bool isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1;
        if (isPracticeMode)
        {
            timerText.gameObject.SetActive(true);
            timerBackground.gameObject.SetActive(true);
            startTime = Time.time;
            isChallengeActive = true;
        }
    }


    void Update()
    {
        if (isChallengeActive)
        {
            elapsedTime = Time.time - startTime;
            UpdateTimerText(elapsedTime);
            if (continueButton.isActiveAndEnabled)
            {
                isChallengeActive = false;
            }
        }
    }

    void UpdateTimerText(float elapsedTime)
    {
        // Format the time and update the Text object
        string formattedTime = FormatTime(elapsedTime);
        timerText.text = formattedTime;
    }

    string FormatTime(float timeInSeconds)
    {
        // Customize the time format as needed (e.g., minutes and seconds)
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    void publishTime()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetFloat("TimeElapsed_" + currentSceneName, elapsedTime);

    }
}
