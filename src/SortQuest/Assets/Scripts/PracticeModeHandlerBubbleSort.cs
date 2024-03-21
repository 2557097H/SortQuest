using UnityEngine;
using UnityEngine.SceneManagement;

public class PracticeModeHandlerBubbleSort: MonoBehaviour
{
    public void OnContinueButtonClick()
    {
        // Check if the practice mode flag is set
        bool isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1;

        if (isPracticeMode)
        {
            // If in practice mode go to main menu
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            // Otherwise continue with story
            SceneManager.LoadScene("BubbleSortStory");
        }
    }
}
