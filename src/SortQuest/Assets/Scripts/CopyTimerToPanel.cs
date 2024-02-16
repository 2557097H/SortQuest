using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CopyTimerToPanel : MonoBehaviour
{
    public Image timerBackground;
    public TextMeshProUGUI timerText;

    public void onClickButton()
    {
        // Get the current scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check the scene name and move the timer objects accordingly
        if (currentSceneName == "QuickSortLevelOne")
        {
            // Find and move objects to PartitionPanel
            MoveObjectsToPanel("PartitionPanel");
        }
        else if (currentSceneName == "MergeSortLevelOne")
        {
            // Find and move objects to MergePanel
            MoveObjectsToPanel("MergePanel");
        }
        // Add more conditions as needed for other scenes
    }

    private void MoveObjectsToPanel(string panelName)
    {
        // Find the next panel in the hierarchy by name (active or inactive)
        GameObject nextPanelObject = GameObject.Find(panelName);

        if (nextPanelObject != null)
        {
            // Find the 'background' object under the 'Canvas'
            Transform background = nextPanelObject.transform.Find("Canvas/Background");

            if (background != null)
            {
                // Move the timer objects to the background
                timerBackground.transform.SetParent(background);
                timerText.transform.SetParent(timerBackground.transform); // Make timerText a child of timerBackground
            }
            else
            {
                Debug.LogWarning("'background' not found under 'Canvas' in panel " + panelName);
            }
        }
        else
        {
            Debug.LogWarning("Panel " + panelName + " not found in the scene.");
        }
    }
}
