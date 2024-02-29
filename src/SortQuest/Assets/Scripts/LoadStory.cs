using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadStory : MonoBehaviour { 

    private string playerName;
    DatabaseReference dbRef;
    private string currentSceneName;
    public Button continueButton;

    async void Start()
    {
        await UnityServices.InitializeAsync();
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        currentSceneName = SceneManager.GetActiveScene().name;

        if (AuthenticationService.Instance.IsSignedIn)
        {
            Color buttonColor = continueButton.GetComponent<Image>().color;
            buttonColor.a = 1.0f;  // Set alpha channel to 1.0 (fully opaque)
            continueButton.GetComponent<Image>().color = buttonColor;
        }
        else
        {
            continueButton.onClick.RemoveAllListeners();
        }

    }

    public async void LoadCheckpoint()
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

            Debug.Log("Loading checkpoint");

            // Retrieve checkpoint data from Firebase
            DataSnapshot dataSnapshot = await dbRef.Child("users").Child(playerName).Child("checkpoint").GetValueAsync();

            if (dataSnapshot != null && dataSnapshot.Exists)
            {
                Dictionary<string, object> checkpointData = dataSnapshot.Value as Dictionary<string, object>;

                if (checkpointData != null)
                {
                    string loadedSceneName = checkpointData["scenename"].ToString();
                    if (currentSceneName == "MainMenu")
                    {
                        SceneSelector sceneSelector = new SceneSelector();
                        sceneSelector.SwitchSceneStory(loadedSceneName);
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No user is currently signed in.");
        }
    }


}
