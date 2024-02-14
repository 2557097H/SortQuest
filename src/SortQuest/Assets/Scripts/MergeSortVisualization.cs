using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading;

public class MergeSortVisualization : MonoBehaviour
{
    public GameObject content;
    public float activationDelay = 0.5f;

    public TextMeshProUGUI stateText; // Text element to display the state
    private bool isPlaying = false; // Flag to control play/pause
    private bool isPaused = false; // Flag to indicate if the coroutine is paused
    private int count = 0;

    private Coroutine mergeSortCoroutine;

    void Start()
    {
    }

    void Update()
    {
        // No need for keyboard input checking here, as buttons will handle it
    }

    public void TogglePlayPause()
    {
        isPlaying = !isPlaying;

        if (!isPlaying)
        {
            isPaused = true; // Set the flag to pause the coroutine
        }
        else
        {
            if (count == 0)
            {
                mergeSortCoroutine = StartCoroutine(MergeSortVisualizationCoroutine(content.transform));
                count++;
            }
            isPaused = false; // Set the flag to resume the coroutine
        }

        UpdateStateText();
    }

    void UpdateStateText()
    {
        stateText.text = isPlaying ? "Playing" : (count == 2 ? "Finished" : "Paused");
    }

    IEnumerator MergeSortVisualizationCoroutine(Transform parent)
    {
        if (parent.childCount > 0)
        {
            for (int i = 1; i < parent.childCount; i++)
            {
                Transform gameObjectContainer = parent.GetChild(i);

                if (!isPlaying)
                {
                    yield return null; // Pause the coroutine
                }

                if (gameObjectContainer.name.Contains("List"))
                {
                    if (gameObjectContainer.childCount > 0)
                    {
                        Transform grid = gameObjectContainer.Find("Grid");

                        if (grid != null && grid.childCount > 0)
                        {
                            for (int j = 0; j < grid.childCount; j++)
                            {
                                Transform slot = grid.GetChild(j);

                                if (slot.childCount > 0)
                                {
                                    slot.GetChild(0).gameObject.SetActive(true);

                                    // Wait for the specified delay before activating the next game object
                                    float timer = 0f;
                                    while (timer < activationDelay)
                                    {
                                        if (!isPaused)
                                        {
                                            timer += Time.deltaTime;
                                        }
                                        yield return null;
                                    }
                                }
                                else
                                {
                                    Debug.LogWarning("Slot at index " + j + " in grid of game object container at index " + i + " is empty.");
                                }
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Grid is missing or empty in game object container at index " + i + ".");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Game object container at index " + i + " is empty.");
                    }
                }
                else
                {
                    Debug.LogWarning("Game object container at index " + i + " does not contain 'List' in its name.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Content is empty.");
        }

        count = 2;
        isPlaying = false;
        UpdateStateText();
    }

    public void Reset()
    {
        StopAllCoroutines(); // Stop any running coroutines, including MergeSortVisualizationCoroutine
        ResetShieldStates(content.transform); // Reset shield states (set all shields to inactive)
        count = 0; // Reset the count
        isPlaying = false; // Reset the play/pause state
        isPaused = false; // Reset the pause state
        UpdateStateText(); // Update the state text
    }

    void ResetShieldStates(Transform parent)
    {
        var firstListCount = 0;
        foreach (Transform child in parent)
        {
            if (firstListCount == 0)
            {

                firstListCount++;
                continue;

            }
            if (child.name.Contains("List"))
            {
                // Reset shields within the slots
                Transform grid = child.Find("Grid");
                if (grid != null)
                {
                    for (int j = 0; j < grid.childCount; j++)
                    {
                        Transform slot = grid.GetChild(j);
                        if (slot.childCount > 0)
                        {
                            GameObject shield = slot.GetChild(0).gameObject;
                            if (shield != null)
                            {
                                shield.SetActive(false);
                            }
                        }
                    }
                }
            }
            else
            {
                ResetShieldStates(child);
            }
        }
    }
} 
