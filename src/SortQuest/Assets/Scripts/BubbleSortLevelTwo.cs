using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BubbleSortLevelTwo : MonoBehaviour
{

    public int[] woodNumberArray;
    public TMP_Text stepText;
    public TMP_Text resultText;
    public TMP_Text currentStep;
    public Button continueButton;
    

    private int steps = 0;
    private int[] woodNumberArrayCopy;
    private int[] numbers = { 2, 8, 5, 3, 9, 4, 1 };
    private List<int[]> history = new System.Collections.Generic.List<int[]>();


    void Start()
    {
        history = BubbleSort(numbers);
        stepText.text = "Step: " + "1/" + (history.Count+1);
        woodNumberArrayCopy = woodNumberArray;
        currentStep.text = "Current Step" + "[" + string.Join(", ", woodNumberArrayCopy) + "]";
    }

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
                  
                    swapHistory.Add((int[])arr.Clone());
                }
            }
        }
        return swapHistory;
    }


    public void UpdateWoodArray()
    {

        // Get all child objects of the WoodManager and sort them based on their position in the hierarchy
        GameObject gridObject = GameObject.Find("Grid");
        woodNumberArrayCopy = woodNumberArray;
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
