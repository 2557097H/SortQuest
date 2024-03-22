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

// Serializable class to represent user data
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
    public Badges badges;
}

// Serializable class to represent badges
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
    // Input fields for sign-up and sign-in
    public TMP_InputField usernameSignInInput;
    public TMP_InputField passwordSignInInput;
    public TMP_InputField usernameSignUpInput;
    public TMP_InputField passwordSignUpInput;

    // Error text for sign-up and sign-in
    public TMP_Text errorTextSignIn;
    public TMP_Text errorTextSignUp;

    // Reference to Firebase database
    DatabaseReference dbRef;

    // User object to hold user data
    public User user;

    // User ID and player name
    public string userId;
    private string playerName;

    async void Start()
    {
        // Initialize Unity services
        await UnityServices.InitializeAsync();

        // Get reference to the Firebase database
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        // Set up censoring for password input fields
        StartCensoring(passwordSignUpInput);
        StartCensoring(passwordSignInInput);
    }

    // Function to set up censoring for password input fields
    private void StartCensoring(TMP_InputField inputField)
    {
        inputField.contentType = TMP_InputField.ContentType.Password;
    }

    // Function to handle sign-up button click
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

    // Function to handle sign-in button click
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

    // Function to sign up with username and password
    async Task SignUpWithUsernamePassword(string username, string password)
    {
        try
        {
            // Sign up the user
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful.");
            errorTextSignUp.text = "SignUp is successful - Please Wait";

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

    // Function to sign in with username and password
    async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            // Sign in the user
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("SignIn is successful.");
            errorTextSignIn.text = "SignIn is successful";

            // Load the main menu scene
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

    // Function to continue as a guest
    public void ContinueAsGuest()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
