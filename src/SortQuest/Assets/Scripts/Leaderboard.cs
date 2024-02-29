using UnityEngine;
using Firebase.Database;
using System;
using System.Collections.Generic;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform entryContainer;
    public string level;

    public void LeaderboardClick()
    {
        // Call a method to load and display the leaderboard data for the specified level
        LoadAndDisplayLeaderboard(level);
    }

    void LoadAndDisplayLeaderboard(string level)
    {
        DatabaseReference leaderboardReference = FirebaseDatabase.DefaultInstance.RootReference.Child("Leaderboard").Child(level);

        // Attach a listener to the leaderboard reference
        leaderboardReference.OrderByValue().LimitToFirst(10).ValueChanged += (sender, e) =>
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError($"Error loading leaderboard: {e.DatabaseError.Message}");
                return;
            }

            // Parse and display the leaderboard data
            ParseAndDisplayLeaderboardData(e.Snapshot);
        };
    }

    void ParseAndDisplayLeaderboardData(DataSnapshot snapshot)
    {
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
                string playerName = childSnapshot.Key;
                float score = Convert.ToSingle(childSnapshot.Value);

                // Add entry to the list
                entries.Add(new LeaderboardEntry(playerName, score));
            }

            // Sort entries by score (ascending order)
            entries.Sort((a, b) => a.Score.CompareTo(b.Score));

            // Display the sorted leaderboard using prefabs and GridLayoutGroup
            foreach (var entry in entries)
            {
                GameObject entryObject = Instantiate(entryPrefab, entryContainer);
                LeaderboardEntryUI entryUI = entryObject.GetComponent<LeaderboardEntryUI>();
                entryUI.SetEntry(position, entry.PlayerName, entry.Score);

                position++;
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
