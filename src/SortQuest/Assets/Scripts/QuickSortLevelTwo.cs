using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class QuickSortLevelTwo : MonoBehaviour
{
    // Public variables
    public List<int> woodNumberArray; // Array to store wood numbers
    public TMP_Text stepText; // Text to display step information
    public TMP_Text resultText; // Text to display result (correct/wrong)
    public TMP_Text currentStep; // Text to display current step
    public Button continueButton; // Button to continue

    // Private variables
    private int[] numbers = { 2, 8, 5, 9, 4, 1, 3 }; // Initial array for sorting
    private int steps = 0; // Counter for steps
    private List<int[]> history = new System.Collections.Generic.List<int[]>(); // List to store sorting history
    private List<int[]> pivotHistoryList = new System.Collections.Generic.List<int[]>(); // List to store pivot history
    private List<int> woodNumberArrayCopy; // Copy of wood number array
    private List<int> pivotList = new System.Collections.Generic.List<int>(); // List to store pivots
    private int pivot; // Current pivot
    [SerializeField] AudioSource finishSound; // Sound played when sorting finishes

    void Start()
    {
        // Sort initial array
        QuickSort(numbers, 0, numbers.Length - 1);
        // Initialize step text
        stepText.text = "Step: " + "1/" + (history.Count + 1);
        // Copy wood number array
        woodNumberArrayCopy = woodNumberArray;
        // Display current step
        currentStep.text = "Current Step" + "[" + string.Join(", ", woodNumberArrayCopy) + "]";
    }

    // QuickSort function
    void QuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            // Partition array
            int pivotIndex = Partition(arr, low, high);

            // Recursive calls for sub-arrays
            QuickSort(arr, low, pivotIndex - 1);
            QuickSort(arr, pivotIndex + 1, high);
        }
    }

    // Partition function
    int Partition(int[] arr, int low, int high)
    {
        // Set pivot
        pivot = arr[high];
        int i = low - 1;

        // Iterate through array
        for (int j = low; j <= high - 1; j++)
        {
            // Swap elements if smaller than pivot
            if (arr[j] <= pivot)
            {
                i++;
                SwapOne(arr, i, j);
            }
        }
        // Swap pivot to correct position
        SwapTwo(arr, i + 1, high);

        return (i + 1);
    }

    // Swap function for smaller elements
    void SwapOne(int[] arr, int i, int j)
    {
        int temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
        // Add to history if elements are different
        if (arr[i] != arr[j])
        {
            history.Add((int[])arr.Clone());
        }
    }

    // Swap function for pivot
    void SwapTwo(int[] arr, int i, int j)
    {
        pivotList.Add(arr[j]);
        int temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
        // Add to history if elements are different
        if (arr[i] != arr[j])
        {
            history.Add((int[])arr.Clone());
        }
        // Add pivot history
        pivotHistoryList.Add((int[])arr.Clone());
    }

    // Function to update wood number array from UI
    public void UpdateDirectionArray()
    {
        // Get all child objects of the WoodManager and update wood number array
        GameObject gridObject = GameObject.Find("Grid");
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
                break;
            }
        }

        // Check if current step matches history
        if (steps < history.Count)
        {
            if (woodNumberArray.SequenceEqual(history[steps]))
            {
                // Increment steps
                steps++;
                resultText.text = "Correct!";
                // Update step text
                stepText.text = "Step: " + (steps + 1).ToString() + "/" + (history.Count + 1);
                // Update current step text
                currentStep.text = "Current Step" + "[" + string.Join(", ", woodNumberArrayCopy) + "]";

                // Check if pivot history matches
                if (woodNumberArray.SequenceEqual(pivotHistoryList[0]))
                {
                    // Access and process each pivotArray here
                    int index = Array.IndexOf(pivotHistoryList[0], pivotList[0]);
                    Debug.Log(index);
                    if (index != -1)
                    {
                        // The element was found in the array
                        // Use 'index' to perform some action
                        Debug.Log($"Index of element: {index}");

                        // Example: Change the color of the corresponding GameObject
                        Transform childTransform = gridObject.transform.GetChild(index);
                        GameObject slotObject = childTransform.gameObject;
                        slotObject.GetComponent<Image>().color = Color.green;
                        pivotList.RemoveAt(0);
                        pivotHistoryList.RemoveAt(0);
                    }
                }

                // Check if sorting is finished
                if (steps == history.Count)
                {
                    // Set PlayerPrefs
                    PlayerPrefs.SetInt("SignsActive", 1);
                    // Play finish sound
                    finishSound.Play();
                    // Activate continue button
                    continueButton.gameObject.SetActive(true);
                }
            }
            else
            {
                resultText.text = "Wrong!";
            }
        }
    }
}
