using Firebase.Database; // Import Firebase.Database namespace for Firebase functionality
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions; 
using System.Threading.Tasks;
using Unity.Services.Authentication; 
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class LoadStory : MonoBehaviour
{

    private string playerName; // Variable to store player name
    DatabaseReference dbRef; // Reference to the Firebase database
    private string currentSceneName; // Variable to store the current scene name
    public Button continueButton; // Reference to the continue button in the UI

    async void Start()
    {
        await UnityServices.InitializeAsync(); // Initialize Unity services asynchronously

        dbRef = FirebaseDatabase.DefaultInstance.RootReference; // Set the reference to the root of the Firebase database
        currentSceneName = SceneManager.GetActiveScene().name; // Get the name of the currently active scene

        // Check if a user is signed in
        if (AuthenticationService.Instance.IsSignedIn)
        {
            // Set the alpha channel of the continue button to fully opaque
            Color buttonColor = continueButton.GetComponent<Image>().color;
            buttonColor.a = 1.0f;
            continueButton.GetComponent<Image>().color = buttonColor;
        }
        else
        {
            continueButton.onClick.RemoveAllListeners(); // Remove any listeners attached to the continue button if no user is signed in
        }
    }

    // Method to load checkpoint asynchronously
    public async void LoadCheckpoint()
    {
        // Check if a user is signed in
        if (AuthenticationService.Instance.IsSignedIn)
        {
            // Get the player name asynchronously
            playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
            Debug.Log("Player Name: " + playerName);

            // Apply regex to remove numeric part from playerName
            string pattern = @"^(.*?)(?:#(\d+))?$"; // Regex pattern to match the player name and optional numeric part
            Match match = Regex.Match(playerName, pattern); // Match the pattern against the player name

            if (match.Success)
            {
                playerName = match.Groups[1].Value; // Extract the player name without the numeric part
            }

            Debug.Log("Loading checkpoint");

            // Retrieve checkpoint data from Firebase
            DataSnapshot dataSnapshot = await dbRef.Child("users").Child(playerName).Child("checkpoint").GetValueAsync();

            // Check if the checkpoint data exists
            if (dataSnapshot != null && dataSnapshot.Exists)
            {
                // Convert checkpoint data to dictionary
                Dictionary<string, object> checkpointData = dataSnapshot.Value as Dictionary<string, object>;

                if (checkpointData != null)
                {
                    // Get the name of the scene to be loaded from the checkpoint data
                    string loadedSceneName = checkpointData["scenename"].ToString();

                    // Check if the current scene is the main menu
                    if (currentSceneName == "MainMenu")
                    {
                        // Create a SceneSelector instance and switch to the loaded scene
                        SceneSelector sceneSelector = new SceneSelector();
                        sceneSelector.SwitchSceneStory(loadedSceneName);
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No user is currently signed in."); // Log a warning if no user is signed in
        }
    }
}
