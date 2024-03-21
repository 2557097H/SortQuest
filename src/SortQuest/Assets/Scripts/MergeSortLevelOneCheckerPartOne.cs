using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MergeSortLevelOneCheckerPartOne : MonoBehaviour
{
    // TextMeshPro components for displaying messages
    public TMP_Text checkText; // Text for displaying checking message
    public TMP_Text continueText; // Text for displaying continue message
    public Button continueButton; // Button for continuing to the next step
    [SerializeField] AudioSource finishSound; // Sound played when the correct order is achieved

    // Check the order of code blocks
    public void CheckCodeBlockOrder()
    {
        // List to hold TextMeshPro components representing code blocks
        List<TMP_Text> codeBlocks = new List<TMP_Text>();

        // Iterate through child objects to find code blocks
        foreach (Transform child in transform)
        {
            TMP_Text codeBlockText = child.GetComponentInChildren<TMP_Text>();
            codeBlocks.Add(codeBlockText); // Add code block text to the list
        }

        // Check the order of code blocks
        for (int i = 0; i < codeBlocks.Count; i++)
        {
            // Compare the actual text of the code block with the expected text
            if (codeBlocks[i].text.ToLower().Replace(" ", "").Trim() !=
                GetExpectedCodeBlockText(i).ToLower().Replace(" ", "").Trim())
            {
                Debug.Log(codeBlocks[i].text.ToLower().Replace(" ", "").Trim());
                Debug.Log(GetExpectedCodeBlockText(i).ToLower().Replace(" ", "").Trim());
                checkText.text = "Not correct order - try again"; // Display message for incorrect order
                return;
            }
        }

        // If the correct order is achieved
        checkText.text = "Correct Order - Well Done!";
        continueButton.gameObject.SetActive(true);
        finishSound.Play();
        continueText.text = "Continue to merge pseudocode re-arrangement";
    }

    // Get the expected text content of a code block based on its index
    private string GetExpectedCodeBlockText(int index)
    {

        string[] expectedOrder =
        {
            "procedure mergeSort(A: list, n: integer)",
            "if n < 2\nreturn",
            "mid = n / 2\nl = new list[mid], r = new list[n - mid]",
            "for i from 0 to mid - 1\nl[i] = A[i]",
            "for i from mid to n - 1\nr[i - mid] = A[i]",
            "mergeSort(l, mid)",
            "mergeSort(r, n - mid)",
            "merge(A, l, r, mid, n - mid)"
        };

        // Ensure the index is within bounds
        if (index >= 0 && index < expectedOrder.Length)
        {
            return expectedOrder[index];
        }

        return null;
    }
}
