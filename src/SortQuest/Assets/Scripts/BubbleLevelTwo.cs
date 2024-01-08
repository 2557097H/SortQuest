using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class BubbleLevelTwo : MonoBehaviour
{
    public GameObject[] numberImages;
    public TMP_Text stepText;
    public TMP_Text resultText;

    private int currentStep = 0;
    private bool isSorting = false;
    private int[] numbers = { 2, 8, 5, 3, 9, 4, 1 };
    private List<int[]> history = new System.Collections.Generic.List<int[]>();  


    void Start()
    {

        history = BubbleSort(numbers);
        InitializeNumberImages();

    }

    void InitializeNumberImages()
    {
        // Your existing code remains unchanged 
    }

    public void OnDrop(NumberImage droppedImage)
    {
        if (isSorting)
        {
            if (CheckSwap(droppedImage))
            {
                PerformSwap(droppedImage);
                
            }
        }
    }



    private bool CheckSwap(NumberImage droppedImage)
    {
        // Your existing logic to check if the swap is correct
        // You might compare the indices of the droppedImage and its neighbor

        return true; // Replace with your actual condition
    }

    private void PerformSwap(NumberImage droppedImage)
    {
        // Update the NumberImages array after the swap
        for (int i = 0; i < numberImages.Length; i++)
        {
            NumberImage numberImage = numberImages[i].GetComponent<NumberImage>();
            numbers[i] = numberImage.GetIndex() + 1;
        }
    }

    System.Collections.Generic.List<int[]> BubbleSort(int[] arr)
    {
        int n = arr.Length;

        // Create a list to store the order of numbers after each swap
        var swapHistory = new System.Collections.Generic.List<int[]>();

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    // Swap elements
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;

                    // Save a copy of the array after each swap
                    swapHistory.Add((int[])arr.Clone());
                }
            }
        }
        return swapHistory;
    }
}
