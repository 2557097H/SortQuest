using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.Threading.Tasks;
using System;
using Unity.Services.Authentication; 
using System.Text.RegularExpressions; 

public class SceneSelector : MonoBehaviour
{
    // Method to switch scenes based on the provided sceneName
    public async void SwitchSceneStory(string sceneName)
    {
        // Set PracticeMode to 0
        PlayerPrefs.SetInt("PracticeMode", 0);

        // Logic for updating badge values based on scene transitions
        if (sceneName == "QuickSortStory")
        {
            // Deactivate Bridge if transitioning to QuickSortStory scene
            PlayerPrefs.SetInt("BridgeActive", 0);

            // Set bubbleBadge value to 1
            await SetBadgeValue("bubbleBadge", 1);
        }
        else if (sceneName == "MergeSortStory" && SceneManager.GetActiveScene().name == "QuickSortStory")
        {
            // Deactivate Signs if transitioning from QuickSortStory to MergeSortStory
            PlayerPrefs.SetInt("SignsActive", 0);

            // Set quickBadge value to 1
            await SetBadgeValue("quickBadge", 1);
        }
        else if (sceneName == "FinalStoryScene" && SceneManager.GetActiveScene().name == "MergeSortLevelTwo")
        {
            // Deactivate Wood if transitioning from MergeSortLevelTwo to FinalStoryScene
            PlayerPrefs.SetInt("WoodActive", 0);

            // Set mergeBadge value to 1
            await SetBadgeValue("mergeBadge", 1);
        }
        else if (sceneName == "MainMenu" && SceneManager.GetActiveScene().name == "FinalStoryScene")
        {
            // Set storyBadge value to 1 when returning to MainMenu from FinalStoryScene
            await SetBadgeValue("storyBadge", 1);
        }

        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }

    // Method to set badge value in the Firebase Realtime Database
    private async Task SetBadgeValue(string badgeName, int value)
    {
        // Check if a user is signed in
        if (AuthenticationService.Instance.IsSignedIn)
        {
            // Get the player's name
            string playerName = await AuthenticationService.Instance.GetPlayerNameAsync();

            // Regular expression pattern to extract player name without the appended number
            string pattern = @"^(.*?)(?:#(\d+))?$";
            Match match = Regex.Match(playerName, pattern);

            // If match is found, extract player name
            if (match.Success)
            {
                playerName = match.Groups[1].Value;
            }

            // Reference to the user's data in the Firebase Realtime Database
            DatabaseReference userReference = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(playerName);

            try
            {
                // Set the badge value in the user's data
                await userReference.Child("badges").Child(badgeName).SetValueAsync(value);
                Debug.Log($"{badgeName} badge value updated successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error setting {badgeName} badge value: {ex.Message}");
            }
        }
        else
        {
            // Log warning if no user is signed in
            Debug.LogWarning("No user is currently signed in.");
        }
    }
}
