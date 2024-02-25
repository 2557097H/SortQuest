using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Database;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using System;
using System.Text.RegularExpressions;

public class Timer : MonoBehaviour
{
    public Image timerBackground;
    public TextMeshProUGUI timerText;
    public Button continueButton;
    public User user;

    private bool isChallengeActive;
    private float startTime;
    private float elapsedTime;

    private string playerName;
    DatabaseReference dbRef;

    async void Start()
    {
        await UnityServices.InitializeAsync();
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
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

    public async void publishTime()
    {
        elapsedTime = (float)Math.Round(elapsedTime, 2);

        string currentSceneName = SceneManager.GetActiveScene().name;
        playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
        string pattern = @"^(.*?)(?:#(\d+))?$";

        Match match = Regex.Match(playerName, pattern);

        if (match.Success)
        {
            playerName = match.Groups[1].Value;
        }

        if (!string.IsNullOrEmpty(playerName))
        {
            DatabaseReference userReference = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(playerName);

            if (currentSceneName == "BubbleSortLevelOne")
            {
                await SetScore(userReference, "bubbleSortLevelOne", elapsedTime);
            }
            else if (currentSceneName == "BubbleSortLevelTwo")
            {
                await SetScore(userReference, "bubbleSortLevelTwo", elapsedTime);
            }
            else if (currentSceneName == "QuickSortLevelOne")
            {
                await SetScore(userReference, "quickSortLevelOne", elapsedTime);
            }
            else if (currentSceneName == "QuickSortLevelTwo")
            {
                await SetScore(userReference, "quickSortLevelTwo", elapsedTime);
            }
            else if (currentSceneName == "MergeSortLevelOne")
            {
                await SetScore(userReference, "mergeSortLevelOne", elapsedTime);
            }
            else if (currentSceneName == "MergeSortLevelTwo")
            {
                await SetScore(userReference, "mergeSortLevelTwo", elapsedTime);
            }
            else
            {
                Debug.LogError("Player name is not available.");
            }
        }
    }

    private async Task SetScore(DatabaseReference userReference, string scoreField, float score)
    {
        try
        {
            // Retrieve existing score and previous high score from the database
            DataSnapshot snapshot = await userReference.Child(scoreField).GetValueAsync();
            float existingScore = snapshot.Exists ? Convert.ToSingle(snapshot.Value) : float.MaxValue;

            // Check if the current score is lower (better) than the existing score
            if (score < existingScore || existingScore == 0)
            {
                // Set the score for the specified field in the user's data
                Debug.Log(scoreField);
                await userReference.Child(scoreField).SetValueAsync(score);
                Debug.Log($"{scoreField} score updated successfully.");

                // Update the leaderboard only if it's a new high score
                await UpdateLeaderboard(scoreField, playerName, score);
            }
            else
            {
                Debug.Log($"Current score ({score}) is not lower than existing score ({existingScore}).");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error setting {scoreField} score: {ex.Message}");
        }
    }

    private async Task UpdateLeaderboard(string level, string playerName, float score)
    {
        try
        {
            // Retrieve the current leaderboard for the specified level
            DatabaseReference leaderboardReference = FirebaseDatabase.DefaultInstance.RootReference.Child("Leaderboard").Child(level);
            DataSnapshot leaderboardSnapshot = await leaderboardReference.GetValueAsync();

            // Check if the player is already in the leaderboard
            bool playerInLeaderboard = false;
            string playerToRemove = null;

            foreach (DataSnapshot entrySnapshot in leaderboardSnapshot.Children)
            {
                string entryPlayerName = entrySnapshot.Key;
                float entryScore = Convert.ToSingle(entrySnapshot.Value);

                if (entryPlayerName == playerName)
                {
                    playerInLeaderboard = true;

                    // Check if the new score is higher than the existing score
                    if (score < entryScore)
                    {
                        // Remove the existing entry for the player
                        playerToRemove = entryPlayerName;
                    }
                    else
                    {
                        // The new score is not higher, no need to update leaderboard
                        return;
                    }
                }
            }

            // Remove the existing entry for the player if needed
            if (playerInLeaderboard && !string.IsNullOrEmpty(playerToRemove))
            {
                await leaderboardReference.Child(playerToRemove).RemoveValueAsync();
                Debug.Log($"Removed previous entry for {playerToRemove} from the leaderboard.");
            }

            // Add the new score to the leaderboard
            await leaderboardReference.Child(playerName).SetValueAsync(score);
            Debug.Log($"Updated leaderboard for {playerName} with new score.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error updating leaderboard: {ex.Message}");
        }
    }
}
