using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MergeSortLevelOneCheckerPartOne : MonoBehaviour
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
            if (codeBlocks[i].text != GetExpectedCodeBlockText(i))
            {
                checkText.text = "Not correct order - try again";
                return;
            }
        }

        checkText.text = "Correct Order - Well Done!";
        continueButton.gameObject.SetActive(true);
        continueText.text = "Continue to merge pseudocode re-arrangement";
    }

    // Get the expected text content of a code block based on its index
    private string GetExpectedCodeBlockText(int index)
    {
        // Define the expected order based on your requirements

        string[] expectedOrder =
        {
            "procedure mergeSort(A: list, n: integer)",
            "if n < 2\r\n    return",
            "mid = n / 2\r\nl = new list[mid], r = new list[n - mid]",
            "for i from 0 to mid - 1\r\n    l[i] = A[i]",
            "for i from mid to n - 1\r\n    r[i - mid] = A[i]",
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
