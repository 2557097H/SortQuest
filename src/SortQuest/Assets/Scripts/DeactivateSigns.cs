using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateSigns : MonoBehaviour
{

    public GameObject signs;
    // Start is called si the first frame update
    void Start()
    {
        bool isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1;

        if (!isPracticeMode)
        {
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
