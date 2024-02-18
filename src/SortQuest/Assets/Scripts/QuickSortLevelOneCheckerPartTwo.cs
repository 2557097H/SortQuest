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
    [SerializeField] AudioSource finishSound;

    // Check the order of code blocks
    public void CheckCodeBlockOrder()
    {
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
                Debug.Log(codeBlocks[i].text.ToLower().Replace(" ", "").Trim());
                Debug.Log(GetExpectedCodeBlockText(i).ToLower().Replace(" ", "").Trim());
                checkText.text = "Not correct order - try again";
                return;
            }
        }

        checkText.text = "Correct Order - Well Done!";
        continueButton.gameObject.SetActive(true);
        finishSound.Play();
        continueText.text = "Continue with story mode";
    }

    private string GetExpectedCodeBlockText(int index)
    {

        string[] expectedOrder =
        {
            "procedure partition(A: list, low: integer, high: integer) returns integer",
            "pivot = A[high]\ni = low - 1",
            "for j from low to high - 1",
            "if A[j] <= pivot",
            "i = i + 1\nswap(A[i], A[j])",
            "swap(A[i + 1], A[high])\nreturn i + 1"
        };

        // Ensure the index is within bounds
        if (index >= 0 && index < expectedOrder.Length)
        {
            return expectedOrder[index];
        }

        return null;
    }
}
