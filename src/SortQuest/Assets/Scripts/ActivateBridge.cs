using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBridge : MonoBehaviour
{

    public GameObject bridge;
    // Start is called before the first frame update
    void Start()
    {
        // Check if the bridge should be active
        Debug.Log(("BridgeActive", 0));
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
