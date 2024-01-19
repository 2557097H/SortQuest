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
    public List<int> woodNumberArray;
    public TMP_Text stepText;
    public TMP_Text resultText;
    public TMP_Text currentStep;
    public Button continueButton;



    private int[] numbers = { 2, 8, 5, 9, 4, 1, 3 };

    private int steps = 0;
    private List<int[]> history = new System.Collections.Generic.List<int[]>();
    private List<int> pivotHistory = new System.Collections.Generic.List<int>();
    private List<int[]> pivotHistoryList = new System.Collections.Generic.List<int[]>();
    private List<int> woodNumberArrayCopy;
    private List<int> pivotList = new System.Collections.Generic.List<int>();
    private int pivotIndex;
    // Start is called before the first frame update
    void Start()
    {
        QuickSort(numbers,0, numbers.Length-1);
        stepText.text = "Step: " + "1/" + (history.Count + 1);
        woodNumberArrayCopy = woodNumberArray;
        currentStep.text = "Current Step" + "[" + string.Join(", ", woodNumberArrayCopy) + "]";
    }

    // Update is called once per frame
    // QuickSort function
    void QuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            pivotIndex = Partition(arr, low, high);

            QuickSort(arr, low, pivotIndex - 1);
            QuickSort(arr, pivotIndex + 1, high);
 
        }
    }

    // Partition function
    int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        pivotList.Add(pivot);
        int i = low - 1;

        for (int j = low; j <= high - 1; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;
                Swap(arr, i, j);
            }
        }
        Swap(arr, i + 1, high);
        
        return (i + 1);
    }

    // Swap function
    void Swap(int[] arr, int i, int j)
    {
        Debug.Log(arr[i]);
        Debug.Log(arr[j]);
        int temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
        if (arr[i] != arr[j])
        {
            history.Add((int[])arr.Clone());
        }

        
    }

    public void UpdateWoodArray()
    {
        Debug.Log("[" + string.Join(", ", pivotList) + "]");
        // Get all child objects of the WoodManager and sort them based on their position in the hierarchy
        GameObject gridObject = GameObject.Find("Grid");
        //woodNumberArrayCopy = woodNumberArray;
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

        if (steps < history.Count)
        {
            
            

            if (woodNumberArray.SequenceEqual(history[steps]))
            {
                steps++;
                resultText.text = "Correct!";
                stepText.text = "Step: " + (steps + 1).ToString() + "/" + (history.Count + 1);
                currentStep.text = "Current Step" + "[" + string.Join(", ", woodNumberArrayCopy) + "]";
                /*
                                if (woodNumberArray.SequenceEqual(pivotHistoryList[0]))
                                {
                                    // Access and process each pivotArray here
                                    int index = Array.IndexOf(pivotHistoryList[0], pivotHistory[0]);
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
                                        pivotHistory.RemoveAt(0);
                                        pivotHistoryList.RemoveAt(0);
                                    }
                                }*/

                if (steps == history.Count)
                {
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
