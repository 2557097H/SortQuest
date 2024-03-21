using UnityEngine.SceneManagement;
using UnityEngine;

public class PracticeModeHandlerMergeSort : MonoBehaviour
{
    // Reference to the SceneSelector component
    public SceneSelector sceneSelector;

    // Method triggered when the "Continue" button is clicked
    public void OnContinueButtonClick()
    {
        // Check if the practice mode flag is set
        bool isPracticeMode = PlayerPrefs.GetInt("PracticeMode", 0) == 1;

        if (isPracticeMode)
        {
            // If in practice mode, load the Main Menu scene
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            // If not in practice mode
            if (SceneManager.GetActiveScene().name == "MergeSortLevelTwo")
            {
                // If the current scene is MergeSortLevelTwo
                if (sceneSelector != null)
                {
                    // If the SceneSelector component is assigned, switch to the FinalStoryScene
                    sceneSelector.SwitchSceneStory("FinalStoryScene");
                }
                else
                {
                    // Log an error if the SceneSelector component is not assigned
                    Debug.LogError("sceneSelector is not assigned.");
                }
            }
            else
            {
                // If not in MergeSortLevelTwo scene, load the MergeSortStory scene
                SceneManager.LoadScene("MergeSortStory");
            }
        }
    }
}
