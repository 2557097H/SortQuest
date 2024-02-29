using UnityEngine.SceneManagement;
using UnityEngine;

public class PracticeModeHandlerMergeSort : MonoBehaviour
{
    public SceneSelector sceneSelector; 

    public void OnContinueButtonClick()
    {
        bool isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1;

        if (isPracticeMode)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "MergeSortLevelTwo")
            {
                if (sceneSelector != null)
                {
                    sceneSelector.SwitchSceneStory("FinalStoryScene");
                }
                else
                {
                    Debug.LogError("sceneSelector is not assigned.");
                }
            }
            else
            {
                SceneManager.LoadScene("MergeSortStory");
            }
        }
    }
}
