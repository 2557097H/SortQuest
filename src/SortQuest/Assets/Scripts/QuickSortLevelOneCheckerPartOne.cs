using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSortLevelOneCheckerPartOne : MonoBehaviour
{
    public TMP_Text checkText;
    public TMP_Text continueText;
    public Button continueButton;

    // Check the order of code blocks
    public void CheckCodeBlockOrder()
    {
        // Assuming you have a list of TextMeshPro components in the correct order
        List<TMP_Text> codeBlocks = new List<TMP_Text>();
        foreach (Transform child in transform)
        {
            TMP_Text codeBlockText = child.GetComponentInChildren<TMP_Text>();
            codeBlocks.Add(codeBlockText);
        }


        // Check the order
        for (int i = 0; i < codeBlocks.Count; i++)
        {
            if (codeBlocks[i].text.ToLower().Replace(" ", "") != GetExpectedCodeBlockText(i).ToLower().Replace(" ", ""))
            {
                checkText.text = "Not correct order - try again";
                return;
            }
        }

        checkText.text = "Correct Order - Well Done!";
        continueButton.gameObject.SetActive(true);
        continueText.text = "Continue to partition pseudocode re-arrangement";
    }

    // Get the expected text content of a code block based on its index
    private string GetExpectedCodeBlockText(int index)
    {
        // Define the expected order based on your requirements

        string[] expectedOrder =
        {
            "procedure quickSort(A: list, low: integer, high: integer)",
            "if low < high",
            "pivotIndex = partition(A, low, high)",
            "quickSort(A, low, pivotIndex - 1)",
            "quickSort(A, pivotIndex + 1, high)"
        };

        // Ensure the index is within bounds
        if (index >= 0 && index < expectedOrder.Length)
        {
            return expectedOrder[index];
        }

        return null;
    }
}
