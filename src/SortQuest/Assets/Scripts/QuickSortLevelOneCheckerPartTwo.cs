using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSortLevelOneCheckerPartTwo : MonoBehaviour
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
        continueText.text = "Continue with story mode";
    }

    // Get the expected text content of a code block based on its index
    private string GetExpectedCodeBlockText(int index)
    {
        // Define the expected order based on your requirements

        string[] expectedOrder =
        {
            "procedure partition(A: list, low: integer, high: integer) returns integer",
            "pivot = A[high]\r\ni = low - 1",
            "for j from low to high - 1",
            "if A[j] <= pivot",
            "i = i + 1\r\nswap(A[i], A[j])",
            "swap(A[i + 1], A[high])\r\nreturn i + 1"
        };

        // Ensure the index is within bounds
        if (index >= 0 && index < expectedOrder.Length)
        {
            return expectedOrder[index];
        }

        return null;
    }
}
