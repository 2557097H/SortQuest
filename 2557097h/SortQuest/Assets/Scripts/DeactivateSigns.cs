using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateSigns : MonoBehaviour
{
    // Reference to the GameObject containing the signs
    public GameObject signs;

    void Start()
    {
        // Check if the game is in practice mode
        bool isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1;

        // If not in practice mode
        if (!isPracticeMode)
        {
            // Check if signs should be active or inactive based on player preferences
            if (PlayerPrefs.GetInt("SignsActive", 0) == 1)
            {
                signs.SetActive(false);
            }
            else
            {
                signs.SetActive(true);
            }
        }
    }
}
