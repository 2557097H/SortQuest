using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitiateLevel : MonoBehaviour
{
    // References to canvas objects and player
    public GameObject canvasLevelOne;
    public GameObject canvasLevelTwo;
    public GameObject canvasSort;
    public GameObject player;

    // Flags to track whether specific triggers have been activated
    private static bool chestTriggered = false;
    private static bool bridgeTriggered = false;
    private static bool foodTriggered = false;
    private static bool woodLogsTriggered = false;
    private static bool directionsTriggered = false;
    private static bool castleTriggered = false;

    // Trigger detection function
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        // Check which trigger has been activated and load appropriate level
        if (!chestTriggered && collider2D.gameObject.CompareTag("Chest"))
        {
            LoadLevelOne();
            chestTriggered = true;
        }
        else if (!bridgeTriggered && collider2D.gameObject.CompareTag("Bridge"))
        {
            LoadLevelTwo();
            bridgeTriggered = true;
        }
        else if (!foodTriggered && collider2D.gameObject.CompareTag("Food"))
        {
            LoadLevelOne();
            foodTriggered = true;
        }
        else if (!directionsTriggered && collider2D.gameObject.CompareTag("Directions"))
        {
            LoadLevelTwo();
            directionsTriggered = true;
        }
        else if (!castleTriggered && collider2D.gameObject.CompareTag("Castle"))
        {
            LoadLevelTwo();
            castleTriggered = true;
        }
        else if (!woodLogsTriggered && collider2D.gameObject.CompareTag("WoodLogs"))
        {
            LoadLevelOne();
            woodLogsTriggered = true;
        }
        // Activate canvas for when player enters sorting trigger area
        else if (collider2D.gameObject.CompareTag("ToSort") || collider2D.gameObject.CompareTag("Sword"))
        {
            canvasSort.SetActive(true);
            player.SetActive(false);
        }
    }

    private void LoadLevelOne()
    {
        canvasLevelOne.SetActive(true);
        player.SetActive(false);
    }

    private void LoadLevelTwo()
    {
        canvasLevelTwo.SetActive(true);
        player.SetActive(false);
    }
}
