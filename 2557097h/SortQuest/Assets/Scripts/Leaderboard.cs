using UnityEngine;
using Firebase.Database;
using System;
using System.Collections.Generic;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public GameObject entryPrefab; // Prefab for leaderboard entry UI
    public Transform entryContainer; // Container for leaderboard entries
    public string level; // Level for which leaderboard is being displayed

    // Method called when leaderboard button is clicked
    public void LeaderboardClick()
    {
        // Call a method to load and display the leaderboard data for the specified level
        LoadAndDisplayLeaderboard(level);
    }

    // Method to load and display leaderboard data
    void LoadAndDisplayLeaderboard(string level)
    {
        // Get reference to the leaderboard node in the Firebase Realtime Database
        DatabaseReference leaderboardReference = FirebaseDatabase.DefaultInstance.RootReference.Child("Leaderboard").Child(level);

        // Attach a listener to the leaderboard reference to fetch data
        leaderboardReference.OrderByValue().LimitToFirst(20).ValueChanged += (sender, e) =>
        {
            // Check for errors
            if (e.DatabaseError != null)
            {
                Debug.LogError($"Error loading leaderboard: {e.DatabaseError.Message}");
                return;
            }

            // Parse and display the leaderboard data
            ParseAndDisplayLeaderboardData(e.Snapshot);
        };
    }

    // Method to parse and display leaderboard data
    void ParseAndDisplayLeaderboardData(DataSnapshot snapshot)
    {
        // Check if data exists
        if (snapshot != null && snapshot.Exists)
        {
            // Clear previous entries
            if (entryContainer.childCount > 0)
            {
                foreach (Transform child in entryContainer)
                {
                    Destroy(child.gameObject);
                }
            }

            // Iterate through the leaderboard entries
            List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
            int position = 1;

            foreach (var childSnapshot in snapshot.Children)
            {
                string playerName = childSnapshot.Key; // Get player name
                float score = Convert.ToSingle(childSnapshot.Value); // Get player score

                // Add entry to the list
                entries.Add(new LeaderboardEntry(playerName, score));
            }

            // Sort entries by score (ascending order)
            entries.Sort((a, b) => a.Score.CompareTo(b.Score));

            // Display the sorted leaderboard using prefabs and GridLayoutGroup
            foreach (var entry in entries)
            {
                GameObject entryObject = Instantiate(entryPrefab, entryContainer); // Instantiate entry prefab
                LeaderboardEntryUI entryUI = entryObject.GetComponent<LeaderboardEntryUI>(); // Get UI component
                entryUI.SetEntry(position, entry.PlayerName, entry.Score); // Set entry UI data

                position++; // Increment position for next entry
            }
        }
        else
        {
            Debug.LogWarning("No data found in the leaderboard.");
        }
    }
}

// Helper class to store leaderboard entries
public class LeaderboardEntry
{
    public string PlayerName { get; } 
    public float Score { get; } 

    public LeaderboardEntry(string playerName, float score)
    {
        PlayerName = playerName;
        Score = score;
    }
}
