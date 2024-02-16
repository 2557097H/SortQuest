using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteringFromPracticeOrStory : MonoBehaviour
{

    public void setPracticeMode()
    {
        PlayerPrefs.SetInt("PracticeMode", 1);
    }

    public void setStoryMode()
    {
        PlayerPrefs.SetInt("PracticeMode", 0);
    }
}
