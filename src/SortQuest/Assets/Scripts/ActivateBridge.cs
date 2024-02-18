using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBridge : MonoBehaviour
{

    public GameObject bridge;
    // Start is called before the first frame update
    void Start()
    {
        bool isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1;

        if (!isPracticeMode)
        {

            if (PlayerPrefs.GetInt("BridgeActive", 0) == 1)
            {
                // Set the bridge GameObject active
                bridge.SetActive(true);
            }
            else
            {
                // Set the bridge GameObject inactive if not already
                bridge.SetActive(false);
            }
        }
    }

}
