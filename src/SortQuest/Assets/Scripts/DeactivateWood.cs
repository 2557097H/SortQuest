using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateWood : MonoBehaviour
{
    public GameObject wood;
    // Start is called before the first frame update
    void Start()
    {
        bool isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1;

        if (!isPracticeMode)
        {
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
