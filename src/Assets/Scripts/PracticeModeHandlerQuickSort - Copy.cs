using UnityEngine;
using UnityEngine.SceneManagement;

public class PracticeModeHandlerQuickSort : MonoBehaviour
{
    public void OnContinueButtonClick()
    {
        //Checks if in practice mode
        bool isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1;

        if (isPracticeMode)
        {
            // If in practice mode load main menu
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            //Otherwise continue woth story mode
            SceneManager.LoadScene("QuickSortStory");
        }
    }
}
