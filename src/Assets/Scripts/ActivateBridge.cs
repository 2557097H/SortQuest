using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBridge : MonoBehaviour
{
    // Reference to the bridge GameObject
    public GameObject bridge;

    void Start()
    {
        // Check if the game is in practice mode (retrieved from PlayerPrefs)
        bool isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1;

        // If not in practice mode
        if (!isPracticeMode)
        {
            // Check if the bridge should be active (retrieved from PlayerPrefs)
            if (PlayerPrefs.GetInt("BridgeActive", 0) == 1)
            {
                bridge.SetActive(true);
            }
            else
            {
                bridge.SetActive(false);
            }
        }
    }
}
