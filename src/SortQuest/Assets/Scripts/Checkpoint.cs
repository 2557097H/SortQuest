using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    // Static variable to hold the checkpoint position
    public static Vector2 checkpoint = new Vector2(-28.04f, -7.78f);

    // Static variable to hold the name of the scene
    private static string sceneName = "BubbleSortStory";

    private void Awake()
    {
        // Get the name of the currently active scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check if the current scene is different from the saved scene name
        if (currentSceneName != sceneName)
        {
            // If different, reset the checkpoint position to default
            checkpoint = new Vector2(-28.04f, -7.78f);
            // Update the saved scene name to the current scene name
            sceneName = currentSceneName;
        }

        // Find the GameObject with the "Player" tag and set its position to the checkpoint
        GameObject.FindGameObjectWithTag("Player").transform.position = checkpoint;
    }

    // Called when another Collider2D enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider has the tag "Player"
        if (collision.transform.tag == "Player")
        {
            // If yes, update the checkpoint position to the player's position
            checkpoint = collision.transform.position;
        }
    }
}
