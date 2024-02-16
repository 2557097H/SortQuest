using UnityEngine;
using UnityEngine.SceneManagement;

public class PracticeModeHandlerMergeSort : MonoBehaviour
{
    public void OnContinueButtonClick()
    {
        bool isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1;

        if (isPracticeMode)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("MergeSortStory");
        }
    }
}
