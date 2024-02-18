using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class MergeSortLevelTwo : MonoBehaviour
{
    private List<int> mergeHistory = new System.Collections.Generic.List<int>();
    private List<int> mergeHistoryChecker = new System.Collections.Generic.List<int>();
    public TMP_Text resultText;
    public Button continueButton;
    [SerializeField] AudioSource finishSound;

    void Start()
    {
        int[] array = { 5, 2, 6, 7, 3, 9, 8, 4 };
        Debug.Log("Original Array: " + string.Join(", ", array));

        MergeSort(array, array.Length, CaptureIntermediateList);
    }

    void MergeSort(int[] A, int n, Action<int[]> captureCallback)
    {
        if (n < 2)
        {
            captureCallback(A);
            return;
        }

        int mid = n / 2;
        int[] l = new int[mid];
        int[] r = new int[n - mid];

        for (int i = 0; i < mid; i++)
        {
            l[i] = A[i];
        }

        for (int i = mid; i < n; i++)
        {
            r[i - mid] = A[i];
        }

        captureCallback(A);

        MergeSort(l, mid, captureCallback);
        MergeSort(r, n - mid, captureCallback);

        Merge(A, l, r, mid, n - mid);
        captureCallback(A);
    }

    void Merge(int[] A, int[] l, int[] r, int left, int right)
    {
        int i = 0, j = 0, k = 0;

        while (i < left && j < right)
        {
            if (l[i] <= r[j])
            {
                A[k] = l[i];
                i++;
            }
            else
            {
                A[k] = r[j];
                j++;
            }
            k++;
        }

        while (i < left)
        {
            A[k] = l[i];
            i++;
            k++;
        }

        while (j < right)
        {
            A[k] = r[j];
            j++;
            k++;
        }
    }

    void CaptureIntermediateList(int[] intermediateList)
    {
        foreach (int i in intermediateList)
        {
            mergeHistory.Add(i);
        }
    }
    public void KeyCheckUpdate()
    {
        // Get all child objects of the WoodManager and sort them based on their position in the hierarchy
        GameObject contentObject = GameObject.Find("Content");
        mergeHistoryChecker.Clear();
        
        foreach (Transform list in contentObject.transform)
        {
            if (!list.name.Contains("List"))
            {
                continue;
            }
            Transform grid = list.transform.GetChild(0);

            foreach (Transform slot in grid)
                try
            {
                GameObject slotObject = slot.gameObject;
                GameObject keyObject = slotObject.transform.GetChild(0).gameObject;
                TMP_Text numberTextTMP = keyObject.transform.GetChild(0).GetComponent<TMP_Text>();
                mergeHistoryChecker.Add(int.Parse(numberTextTMP.text));

                }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
                mergeHistoryChecker.Add(-1);
                continue;
            }
        
        }

        if (mergeHistoryChecker.SequenceEqual(mergeHistory))
        {
            resultText.text = "Correct - Well Done";
            finishSound.Play();
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            resultText.text = "Wrong";
            int i = 0;
                foreach (Transform list in contentObject.transform)
                {
                if (!list.name.Contains("List")) { continue; }
                    Transform grid = list.transform.GetChild(0);

                foreach (Transform slot in grid)
                {
                    GameObject slotObject = slot.gameObject;
                    
                    if (mergeHistory[i] != mergeHistoryChecker[i])
                    {
                        
                        slotObject.GetComponent<Image>().color = Color.red;
                        i++;
                    }
                    else
                    {
                        slotObject.GetComponent<Image>().color = Color.green;
                        i++;
                    }
                            
                            

                        }


                }

            
        }
    }
}
