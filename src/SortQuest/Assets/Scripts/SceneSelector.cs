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
    public async void SwitchSceneStory(string sceneName)
    {
        PlayerPrefs.SetInt("PracticeMode", 0);

        if (sceneName == "QuickSortStory")
        {
            PlayerPrefs.SetInt("BridgeActive", 0);

            await SetBadgeValue("bubbleBadge", 1);
        }
        else if (sceneName == "MergeSortStory" && SceneManager.GetActiveScene().name == "QuickSortStory")
        {
            PlayerPrefs.SetInt("SignsActive", 0);

   
            await SetBadgeValue("quickBadge", 1);
        }
        else if (sceneName == "FinalStoryScene" && SceneManager.GetActiveScene().name == "MergeSortLevelTwo")
        {
            PlayerPrefs.SetInt("WoodActive", 0);

            
            await SetBadgeValue("mergeBadge", 1);
        }
        else if(sceneName == "MainMenu" && SceneManager.GetActiveScene().name == "FinalStoryScene")
        {
            await SetBadgeValue("storyBadge", 1);
        }

        SceneManager.LoadScene(sceneName);
    }

    private async Task SetBadgeValue(string badgeName, int value)
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            string playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
            string pattern = @"^(.*?)(?:#(\d+))?$";
            Match match = Regex.Match(playerName, pattern);

            if (match.Success)
            {
                playerName = match.Groups[1].Value;
            }
            DatabaseReference userReference = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(playerName);

            try
            {
;
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
            Debug.LogWarning("No user is currently signed in.");
        }
    }
}
