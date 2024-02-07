using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitiateLevel : MonoBehaviour
{
    public GameObject canvasLevelOne;
    public GameObject canvasLevelTwo;
    public GameObject canvasSort;
    public GameObject player;

    private static bool chestTriggered = false;
    private static bool bridgeTriggered = false;
    private static bool foodTriggered = false;
    private static bool woodLogsTriggered = false;

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
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
        else if (!woodLogsTriggered && collider2D.gameObject.CompareTag("WoodLogs"))
        {
            LoadLevelTwo();
            woodLogsTriggered = true;
        }
        else if (collider2D.gameObject.CompareTag("ToSort"))
        {
            canvasSort.SetActive(true);
            player.SetActive(false);
        }
        /*
        else if (!mergeLevelOneTriggered && collider2D.gameObject.CompareTag("n/a"))
        {
            LoadMergeLevelOne();
            mergeLevelOneTriggered = true;
        }
        else if (!mergeLevelTwoTriggered && collider2D.gameObject.CompareTag("n/a"))
        {
            LoadMergeLevelTwo();
            mergeLevelTwoTriggered = true;
        }*/
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
