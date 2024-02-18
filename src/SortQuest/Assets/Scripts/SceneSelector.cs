using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    public void SwitchSceneStory(string sceneName)
    {
        PlayerPrefs.SetInt("PracticeMode", 0);
        if(sceneName == "QuickSortStory")
        {
            PlayerPrefs.SetInt("BridgeActive", 0);
        }
        else if(sceneName == "MergeSortStory" && SceneManager.GetActiveScene().name == "QuickSortStory")
        {
            PlayerPrefs.SetInt("SignsActive", 0);
        }
        else if (sceneName == "MergeSortStory" && SceneManager.GetActiveScene().name == "MergeSortStory")
        {
            PlayerPrefs.SetInt("WoodActive", 0);
        }
        SceneManager.LoadScene(sceneName);

    }
}
