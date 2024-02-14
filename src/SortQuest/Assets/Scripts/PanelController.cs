using UnityEngine;

public class PanelController : MonoBehaviour
{
    private bool isFirstOpen = true;
    private Vector3[] initialPositions;

    // This function is called when the object becomes enabled and active.
    private void OnEnable()
    {
        if (!isFirstOpen)
        {
            ResetPanel();
        }
        else
        {
            // Set up initial state for the first time.
            isFirstOpen = false;
            SaveInitialPositions();
        }
    }

    private void SaveInitialPositions()
    {
        // Save the initial positions of game objects when the panel is first opened.
        // For example:
        // initialPositions[0] = gameObj1.transform.position;
        // initialPositions[1] = gameObj2.transform.position;
        // Save positions for other game objects as needed.


    }

    private void ResetPanel()
    {
        // Reset the positions of game objects to their initial values.
        // For example:
        // gameObj1.transform.position = initialPositions[0];
        // gameObj2.transform.position = initialPositions[1];
        // Reset positions for other game objects as needed.
    }
}
