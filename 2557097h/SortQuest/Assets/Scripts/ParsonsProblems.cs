using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParsonsProblems : MonoBehaviour
{
    private Button upArrowButton;
    private Button downArrowButton;

    void Start()
    {
        // Find buttons dynamically within the row
        Button[] buttons = GetComponentsInChildren<Button>();
        upArrowButton = buttons[0];
        downArrowButton = buttons[1];

        // Attach click events to the up and down arrow buttons
        upArrowButton.onClick.AddListener(SwapUp);
        downArrowButton.onClick.AddListener(SwapDown);
    }

    // Swap code block with the one above
    public void SwapUp()
    {
        // Get the index of the current row
        int rowIndex = transform.GetSiblingIndex();

        // Ensure there is a row above
        if (rowIndex > 0)
        {
            // Get the row above
            GameObject rowAbove = transform.parent.GetChild(rowIndex - 1).gameObject;

            // Swap the code block content
            SwapCodeBlockContent(rowAbove, transform.gameObject);
        }
    }

    // Swap code block with the one below
    public void SwapDown()
    {
        // Get the index of the current row
        int rowIndex = transform.GetSiblingIndex();

        // Ensure there is a row below
        if (rowIndex < transform.parent.childCount - 1)
        {
            // Get the row below
            GameObject rowBelow = transform.parent.GetChild(rowIndex + 1).gameObject;

            // Swap the code block content
            SwapCodeBlockContent(rowBelow, transform.gameObject);
        }
    }

    // Swap code block content between two row GameObjects
    private void SwapCodeBlockContent(GameObject row1, GameObject row2)
    {
        // Get the code block TextMeshPro components within each row
        TMP_Text codeBlockText1 = row1.GetComponentInChildren<TMP_Text>();
        TMP_Text codeBlockText2 = row2.GetComponentInChildren<TMP_Text>();

        // Swap the code block content
        string temp = codeBlockText1.text;
        codeBlockText1.text = codeBlockText2.text;
        codeBlockText2.text = temp;
    }
}
