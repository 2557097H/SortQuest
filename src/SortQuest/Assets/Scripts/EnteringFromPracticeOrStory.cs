using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteringFromPracticeOrStory : MonoBehaviour
{
    //Sets PlayerPrefs depending on entering from practice mode or story mode
    public void setPracticeMode()
    {
        PlayerPrefs.SetInt("PracticeMode", 1);
    }

    public void setStoryMode()
    {
        PlayerPrefs.SetInt("PracticeMode", 0);
    }
}
