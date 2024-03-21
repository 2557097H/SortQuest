using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BubbleSortLevelTwo : MonoBehaviour
{
    // Variables declaration
    public int[] woodNumberArray; // Array to hold wood numbers
    public TMP_Text stepText; //  UI Text element to display step information
    public TMP_Text resultText; // UI Text element to display result information
    public TMP_Text currentStep; // UI Text element to display current step information
    public Button continueButton; // UI Button element to continue

    private int steps = 0; // Counter for steps
    private int[] woodNumberArrayCopy; // Copy of wood number array
    private int[] numbers = { 2, 8, 5, 3, 9, 4, 1 }; // Initial numbers for sorting
    private List<int[]> history = new System.Collections.Generic.List<int[]>(); // List to store sorting history

    [SerializeField] AudioSource finishSound; // Sound played when sorting is finished

    void Start()
    {
        // Initialize sorting history with bubble sort
        history = BubbleSort(numbers);
        // Set initial step text
        stepText.text = "Step: " + "1/" + (history.Count + 1);
  
        woodNumberArrayCopy = woodNumberArray;

        // Display current step
        currentStep.text = "Current Step" + "[" + string.Join(", ", woodNumberArrayCopy) + "]";
    }

    // Bubble sort algorithm
    System.Collections.Generic.List<int[]> BubbleSort(int[] arr)
    {
        int n = arr.Length;
        var swapHistory = new System.Collections.Generic.List<int[]>();

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                    swapHistory.Add((int[])arr.Clone()); // Add current state to swap history
                }
            }
        }
        return swapHistory;
    }

    // Update wood array based on user input
    public void UpdateWoodArray()
    {
        // Find the grid object
        GameObject gridObject = GameObject.Find("Grid");
        woodNumberArrayCopy = woodNumberArray; 
        // Update wood number array based on user input
        for (int i = 0; i < gridObject.transform.childCount; i++)
        {
            Transform childTransform = gridObject.transform.GetChild(i);
            GameObject slotObject = childTransform.gameObject;
            try
            {
                GameObject woodObject = slotObject.transform.GetChild(0).gameObject;
                TMP_Text numberTextTMP = woodObject.transform.GetChild(0).GetComponent<TMP_Text>();

                if (int.TryParse(numberTextTMP.text, out int intValue))
                {
                    woodNumberArray[i] = intValue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
                continue;
            }
        }

        // Check if sorting is complete
        if (steps < history.Count)
        {
            // If wood number array matches the sorted array
            if (woodNumberArray.SequenceEqual(history[steps]))
            {
                steps++; // Increment step counter
                resultText.text = "Correct!"; // Display correct result
                stepText.text = "Step: " + (steps + 1).ToString() + "/" + (history.Count + 1); // Update step text
                currentStep.text = "Current Step" + "[" + string.Join(", ", woodNumberArrayCopy) + "]"; // Display current step

                // Change color of slots
                for (int i = 0; i < gridObject.transform.childCount; i++)
                {
                    try
                    {
                        Transform childTransform = gridObject.transform.GetChild(i);
                        GameObject slotObject = childTransform.gameObject;
                        slotObject.GetComponent<Image>().color = Color.green;

                        // If sorting is complete, play finish sound and activate continue button
                        if (steps == history.Count)
                        {
                            finishSound.Play();
                            continueButton.gameObject.SetActive(true);
                            PlayerPrefs.SetInt("BridgeActive", 1); // Set BridgeActive player pref
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception caught: {ex.Message}");
                        continue;
                    }
                }
            }
            else // If wood number array doesn't match the sorted array
            {
                // Change color of slots
                for (int i = 0; i < gridObject.transform.childCount; i++)
                {
                    try
                    {
                        Transform childTransform = gridObject.transform.GetChild(i);
                        GameObject slotObject = childTransform.gameObject;
                        GameObject woodObject = slotObject.transform.GetChild(0).gameObject;
                        TMP_Text numberTextTMP = woodObject.transform.GetChild(0).GetComponent<TMP_Text>();
                        if (int.TryParse(numberTextTMP.text, out int number))
                        {
                            if (number == history[steps][i])
                            {
                                slotObject.GetComponent<Image>().color = Color.green; // Correctly sorted
                            }
                            else
                            {
                                slotObject.GetComponent<Image>().color = Color.red; // Incorrectly sorted
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception caught: {ex.Message}");
                        continue;
                    }
                }
                resultText.text = "Wrong!"; // Display wrong result
            }
        }
    }
}
