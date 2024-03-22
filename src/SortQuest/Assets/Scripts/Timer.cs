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
    public Image timerBackground; // Reference to the timer's background image
    public TextMeshProUGUI timerText; // Reference to the timer's text component
    public Button continueButton; // Reference to the continue button
    public User user; // Reference to the User class

    private bool isChallengeActive; // Flag indicating whether the challenge is active
    private float startTime; // Time when the challenge started
    private float elapsedTime; // Elapsed time since the challenge started

    private string playerName; // Player's name
    DatabaseReference dbRef; // Reference to the Firebase Database
    private bool isPracticeMode; // Flag indicating whether it's practice mode

    async void Start()
    {
        await UnityServices.InitializeAsync(); // Initialize Unity Services
        dbRef = FirebaseDatabase.DefaultInstance.RootReference; // Reference to the root of the Firebase Database
        isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1; // Check if it's practice mode
        if (isPracticeMode)
        {
            timerText.gameObject.SetActive(true); // Activate the timer text
            timerBackground.gameObject.SetActive(true); // Activate the timer background
            startTime = Time.time; // Record the start time
            isChallengeActive = true; // Set the challenge as active
        }
    }

    void Update()
    {
        if (isChallengeActive)
        {
            elapsedTime = Time.time - startTime; // Calculate elapsed time
            UpdateTimerText(elapsedTime); // Update the timer text
            if (continueButton.isActiveAndEnabled)
            {
                isChallengeActive = false; // If the continue button is active, end the challenge
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
        if (AuthenticationService.Instance.IsSignedIn) { 
            if (isPracticeMode)
            {
                elapsedTime = (float)Math.Round(elapsedTime, 2);

                string currentSceneName = SceneManager.GetActiveScene().name; // Get the current scene name
                playerName = await AuthenticationService.Instance.GetPlayerNameAsync(); // Get the player's name
                string pattern = @"^(.*?)(?:#(\d+))?$";

                Match match = Regex.Match(playerName, pattern); // Match the player's name pattern

                if (match.Success)
                {
                    playerName = match.Groups[1].Value; // Extract the player's name
                }

                if (!string.IsNullOrEmpty(playerName))
                {
                    DatabaseReference userReference = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(playerName); // Reference to the player's data in the database

                    // Check the current scene name and set the score accordingly
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
                        Debug.LogError("Player name is not available."); // Log an error if the player's name is not available
                    }
                }
            } }
    }

    // Set the score in the database
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

    // Update the leaderboard with the new score
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
