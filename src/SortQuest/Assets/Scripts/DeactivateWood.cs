using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateWood : MonoBehaviour
{
    public GameObject wood; // Reference to the GameObject named "wood"

    void Start()
    {
        // Check if the game is in practice mode based on PlayerPrefs
        bool isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1;

        // If not in practice mode, execute the following logic
        if (!isPracticeMode)
        {
            // Check if the "WoodActive" PlayerPrefs is set to 1 (meaning active)
            if (PlayerPrefs.GetInt("WoodActive", 0) == 1)
            {
                wood.SetActive(false);
            }
            else
            {
                wood.SetActive(true);
            }
        }
    }
}
