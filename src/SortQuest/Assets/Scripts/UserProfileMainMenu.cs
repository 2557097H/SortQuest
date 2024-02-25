using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Firebase.Database;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine.UI;

public class UserProfileMainMenu : MonoBehaviour
{
    private string playerName;

    public TMP_Text userText;

    public TMP_Text bubbleLevelOneTime;
    public TMP_Text bubbleLevelTwoTime;
    public TMP_Text quickLevelOneTime;
    public TMP_Text quickLevelTwoTime;
    public TMP_Text mergeLevelOneTime;
    public TMP_Text mergeLevelTwoTime;

    public GameObject bubbleBadge;
    public GameObject quickBadge;
    public GameObject mergeBadge;
    public GameObject storyBadge;
    public GameObject practiceBadge;
    public GameObject learnBadge;

    private float elapsedTime;

    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();
        DisplayPlayerName();
    }

    // Update is called once per frame
    void Update()
    {
        // Your update logic here
    }

    async void DisplayPlayerName()
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
            Debug.Log("Player Name: " + playerName);

            // Apply regex to remove numeric part from playerName
            string pattern = @"^(.*?)(?:#(\d+))?$";
            Match match = Regex.Match(playerName, pattern);

            if (match.Success)
            {
                playerName = match.Groups[1].Value;
            }

            userText.text = "Hi " + playerName;
            DisplayUserScores();
            FetchAndDisplayBadges();
        }
        else
        {
            userText.text = "Hi Guest";
            Debug.LogWarning("No user is currently signed in.");
        }
    }

    async void DisplayUserScores()
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            DatabaseReference userReference = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(playerName);
            DataSnapshot snapshot = await userReference.GetValueAsync();

            if (snapshot.Exists)
            {
                // Retrieve user scores from the database
                float bubbleLevelOneScore = Convert.ToSingle(snapshot.Child("bubbleSortLevelOne").Value);
                float bubbleLevelTwoScore = Convert.ToSingle(snapshot.Child("bubbleSortLevelTwo").Value);
                float quickLevelOneScore = Convert.ToSingle(snapshot.Child("quickSortLevelOne").Value);
                float quickLevelTwoScore = Convert.ToSingle(snapshot.Child("quickSortLevelTwo").Value);
                float mergeLevelOneScore = Convert.ToSingle(snapshot.Child("mergeSortLevelOne").Value);
                float mergeLevelTwoScore = Convert.ToSingle(snapshot.Child("mergeSortLevelTwo").Value);

                // Update UI text fields
                bubbleLevelOneTime.text = FormatTime(bubbleLevelOneScore);
                bubbleLevelTwoTime.text = FormatTime(bubbleLevelTwoScore);
                quickLevelOneTime.text = FormatTime(quickLevelOneScore);
                quickLevelTwoTime.text = FormatTime(quickLevelTwoScore);
                mergeLevelOneTime.text = FormatTime(mergeLevelOneScore);
                mergeLevelTwoTime.text = FormatTime(mergeLevelTwoScore);
            }
            else
            {
                Debug.LogWarning($"User data for {playerName} not found in the database.");
            }
        }
        else
        {
            Debug.LogWarning("No user is currently signed in.");
        }
    }

    async void FetchAndDisplayBadges()
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            DatabaseReference userReference = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(playerName);
            DataSnapshot snapshot = await userReference.GetValueAsync();

            if (snapshot.Exists)
            {
                // Retrieve badge values from the database
                int bubbleBadgeValue = Convert.ToInt32(snapshot.Child("badges").Child("bubbleBadge").Value);
                int quickBadgeValue = Convert.ToInt32(snapshot.Child("badges").Child("quickBadge").Value);
                int mergeBadgeValue = Convert.ToInt32(snapshot.Child("badges").Child("mergeBadge").Value);
                int storyBadgeValue = Convert.ToInt32(snapshot.Child("badges").Child("storyBadge").Value);
                int practiceBadgeValue = Convert.ToInt32(snapshot.Child("badges").Child("practiceBadge").Value);
                int learnBadgeValue = Convert.ToInt32(snapshot.Child("badges").Child("learnBadge").Value);

                // Update badge images transparency based on badge values
                SetBadgeTransparency(bubbleBadge, bubbleBadgeValue);
                SetBadgeTransparency(quickBadge, quickBadgeValue);
                SetBadgeTransparency(mergeBadge, mergeBadgeValue);
                SetBadgeTransparency(storyBadge, storyBadgeValue);
                SetBadgeTransparency(practiceBadge, practiceBadgeValue);
                SetBadgeTransparency(learnBadge, learnBadgeValue);
            }
            else
            {
                Debug.LogWarning($"User data for {playerName} not found in the database.");
            }
        }
        else
        {
            Debug.LogWarning("No user is currently signed in.");
        }
    }

    void SetBadgeTransparency(GameObject badge, int badgeValue)
    {
        // Assuming badgeValue is either 0 or 1
        Image badgeImage = badge.GetComponent<Image>();
        Color badgeColor = badgeImage.color;
        badgeColor.a = badgeValue == 1 ? 1f : 0.4f; // Set transparency to 255 if badgeValue is 1, else 40%
        badgeImage.color = badgeColor;
    }

    string FormatTime(float time)
    {
        // If the time is 0, display "N/A", otherwise format the time as desired
        return time == 0f ? "N/A" : time.ToString("F2") + "s";
    }
}
