using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System;
using System.Text.RegularExpressions;


[Serializable]
public class User
{
    public string userName;
    public float bubbleSortLevelOne;
    public float bubbleSortLevelTwo;
    public float quickSortLevelOne;
    public float quickSortLevelTwo;
    public float mergeSortLevelOne;
    public float mergeSortLevelTwo;
    public Badges badges;  // Add a property for badges
}

[Serializable]
public class Badges
{
    public float bubbleBadge;
    public float quickBadge;
    public float mergeBadge;
    public float storyBadge;
    public float practiceBadge;
    public float learnBadge;
}


public class AuthManager : MonoBehaviour
{
    public TMP_InputField usernameSignInInput;
    public TMP_InputField passwordSignInInput;
    public TMP_InputField usernameSignUpInput;
    public TMP_InputField passwordSignUpInput;
    public TMP_Text errorTextSignIn;
    public TMP_Text errorTextSignUp;
    DatabaseReference dbRef;
    public User user;
    public string userId;
    private string playerName;
    // Start is called before the first frame update
    async void Start()
    {
    
        await UnityServices.InitializeAsync();
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        // Set up censoring for password input fields
        StartCensoring(passwordSignUpInput);
        StartCensoring(passwordSignInInput);
    }

    private void StartCensoring(TMP_InputField inputField)
    {
        inputField.contentType = TMP_InputField.ContentType.Password;
    }

    public async void OnSignUpButtonClick()
    {
        string username = usernameSignUpInput.text;
        string password = passwordSignUpInput.text;
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            await SignUpWithUsernamePassword(username, password);
        }
        else
        {
            errorTextSignUp.text = "Username or password field is empty.";
            Debug.Log("Username or password field is empty.");
        }
    }

    public async void OnSignInButtonClick()
    {
        string username = usernameSignInInput.text;
        string password = passwordSignInInput.text;
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            await SignInWithUsernamePasswordAsync(username, password);
        }
        else
        {
            errorTextSignIn.text = "Username or password field is empty.";
            Debug.LogError("Username or password field is empty.");
        }
    }


    async Task SignUpWithUsernamePassword(string username, string password)
    {
        try
        {
            // Sign up the user
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful.");
            errorTextSignUp.text = "SignUp is successful";

            // Initialize the user object
            user.userName = username;
            user.bubbleSortLevelOne = 0;
            user.bubbleSortLevelTwo = 0;
            user.quickSortLevelOne = 0;
            user.quickSortLevelTwo = 0;
            user.mergeSortLevelOne = 0;
            user.mergeSortLevelTwo = 0;

            // Set up badges information
            user.badges = new Badges
            {
                bubbleBadge = 0,
                quickBadge = 0,
                mergeBadge = 0,
                storyBadge = 0,
                practiceBadge = 0,
                learnBadge = 0
            };

            // Convert user object to JSON
            string json = JsonUtility.ToJson(user);

            // Update player name and store user data in the database
            await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            await dbRef.Child("users").Child(username).SetRawJsonValueAsync(json);

            // Load the main menu scene
            SceneManager.LoadScene("MainMenu");
        }
        catch (AuthenticationException ex)
        {
            errorTextSignUp.text = "Username has been taken or username or password does not meet requirements";
            Debug.Log(ex);
        }
        catch (RequestFailedException ex)
        {
            errorTextSignUp.text = "Username has been taken or username or password does not meet requirements";
            Debug.Log(ex);
        }
    }

    async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("SignIn is successful.");
            errorTextSignIn.text = "SignIn is successful";
            SceneManager.LoadScene("MainMenu");
        }
        catch (AuthenticationException ex)
        {
            errorTextSignIn.text = "Username or password incorrect";
            Debug.Log(ex);
        }
        catch (RequestFailedException ex)
        {
            errorTextSignIn.text = "Username or password incorrect";
            Debug.Log(ex);
        }
    }

    public void ContinueAsGuest()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
