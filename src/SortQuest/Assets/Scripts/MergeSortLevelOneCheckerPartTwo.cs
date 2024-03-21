using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MergeSortLevelOneCheckerPartTwo : MonoBehaviour
{
    public TMP_Text checkText;          // Text component to display check result
    public TMP_Text continueText;       // Text component to display continue option
    public Button continueButton;       // Button to continue
    [SerializeField] AudioSource finishSound;   // Sound played upon finishing

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
            if (codeBlocks[i].text.ToLower().Replace(" ", "").Trim() != GetExpectedCodeBlockText(i).ToLower().Replace(" ", "").Trim())
            {
                checkText.text = "Not correct order - try again";  // Display message for incorrect order
                return;
            }
        }

        checkText.text = "Correct Order - Well Done!";
        continueButton.gameObject.SetActive(true);
        finishSound.Play();
        PlayerPrefs.SetInt("WoodActive", 1);          // Set PlayerPrefs for wood activation
        continueText.text = "Continue";
    }
    // Get the expected text content of a code block based on its index
    private string GetExpectedCodeBlockText(int index)
    {
        

        string[] expectedOrder =
        {
            "procedure merge(A: list, l: list, r: list, left: integer, right: integer)",
            "i = 0, j = 0, k = 0",
            "while i < left and j < right",
            "if l[i] <= r[j]",
            "A[k] = l[i]\ni = i + 1",
            "else",
            "A[k] = r[j]\nj = j + 1",
            "k = k + 1",
            "while i < left",
            "A[k] = l[i]\ni = i + 1, k = k + 1",
            "while j < right",
            "A[k] = r[j]\nj = j + 1, k = k + 1"
        };

        // Ensure the index is within bounds
        if (index >= 0 && index < expectedOrder.Length)
        {
            return expectedOrder[index];
        }

        return null;
    }
}
