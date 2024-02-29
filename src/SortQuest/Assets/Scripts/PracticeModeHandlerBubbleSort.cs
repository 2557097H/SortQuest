using UnityEngine;
using UnityEngine.SceneManagement;

public class PracticeModeHandlerBubbleSort: MonoBehaviour
{
    public void OnContinueButtonClick()
    {
        bool isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1;

        Debug.Log(isPracticeMode + " CLickidy click mate");

        if (isPracticeMode)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("BubbleSortStory");
        }
    }
}
