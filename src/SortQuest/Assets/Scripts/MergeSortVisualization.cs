using System.Collections;
using UnityEngine;
using TMPro;

public class MergeSortVisualization : MonoBehaviour
{
    public GameObject[] numberObjects;
    public float swapDelay = 1.0f;

    private Vector3[] originalPositions;

    private void Start()
    {
        originalPositions = new Vector3[numberObjects.Length];
        for (int i = 0; i < numberObjects.Length; i++)
        {
            originalPositions[i] = numberObjects[i].transform.localPosition;
        }

        StartCoroutine(MergeSortAnimation(numberObjects, 0, numberObjects.Length - 1));
    }

    IEnumerator MergeSortAnimation(GameObject[] array, int left, int right)
    {
        if (left < right)
        {
            int middle = (left + right) / 2;

            yield return StartCoroutine(MergeSortAnimation(array, left, middle));
            yield return StartCoroutine(MergeSortAnimation(array, middle + 1, right));

            yield return StartCoroutine(Merge(array, left, middle, right));
        }
    }

    IEnumerator Merge(GameObject[] array, int left, int middle, int right)
    {
        int leftSize = middle - left + 1;
        int rightSize = right - middle;

        GameObject[] leftArray = new GameObject[leftSize];
        GameObject[] rightArray = new GameObject[rightSize];

        // Temporarily hide original objects
        for (int i = left; i <= right; i++)
        {
            array[i].SetActive(false);
        }

        // Move original objects to new positions
        for (int i = 0; i < leftSize; i++)
        {
            array[left + i].transform.localPosition = new Vector3(originalPositions[left + i].x - 5, 10, 0);
            leftArray[i] = array[left + i];
            leftArray[i].SetActive(true);
            yield return new WaitForSeconds(swapDelay);
        }

        for (int j = 0; j < rightSize; j++)
        {
            array[middle + 1 + j].transform.localPosition = new Vector3(originalPositions[middle + 1 + j].x + 5, 5, 0);
            rightArray[j] = array[middle + 1 + j];
            rightArray[j].SetActive(true);
            yield return new WaitForSeconds(swapDelay);
        }

        int leftIndex = 0;
        int rightIndex = 0;
        int mergedIndex = left;

        // Merge the temporary arrays back into the original array
        while (leftIndex < leftSize && rightIndex < rightSize)
        {
            if (GetNumber(leftArray[leftIndex]) <= GetNumber(rightArray[rightIndex]))
            {
                array[mergedIndex] = leftArray[leftIndex];
                leftIndex++;
            }
            else
            {
                array[mergedIndex] = rightArray[rightIndex];
                rightIndex++;
            }

            yield return new WaitForSeconds(swapDelay);
            mergedIndex++;
        }

        // Copy remaining elements if any
        while (leftIndex < leftSize)
        {
            array[mergedIndex] = leftArray[leftIndex];
            leftIndex++;
            mergedIndex++;
            yield return new WaitForSeconds(swapDelay);
        }

        while (rightIndex < rightSize)
        {
            array[mergedIndex] = rightArray[rightIndex];
            rightIndex++;
            mergedIndex++;
            yield return new WaitForSeconds(swapDelay);
        }

        // Reset the original positions of the merged elements
        for (int i = left; i <= right; i++)
        {
            array[i].transform.localPosition = originalPositions[i];
            array[i].SetActive(true);
            yield return new WaitForSeconds(swapDelay);
        }
    }

    int GetNumber(GameObject obj)
    {
        // Assuming the TMP text is a direct child of the GameObject
        TextMeshProUGUI tmpText = obj.GetComponentInChildren<TextMeshProUGUI>();

        if (tmpText != null && int.TryParse(tmpText.text, out int number))
        {
            return number;
        }

        // Return a default value if the number couldn't be retrieved
        return 0;
    }
}
