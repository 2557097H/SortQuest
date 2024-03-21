using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuickSortVisualization : MonoBehaviour
{
    // Public variables
    public GameObject[] numberObjects;
    public float swapDelay = 1.0f;
    public float pivotDisplayDelay = 1.5f;

    // Private variables
    private GameObject[] originalPositions = new GameObject[7];
    private Vector3[] originalPositionTransforms = new Vector3[7];

    // UI Text element to display the state
    public TextMeshProUGUI stateText;

    // Flags to control play/pause
    private bool isPlaying = false;
    private bool isPaused = false;
    private int count = 0;

    void Start()
    {
        // Save original positions of number objects
        SaveOriginalPositions();
    }

    // Function to toggle play/pause
    public void TogglePlayPause()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            isPaused = false;
            if (count == 0)
            {
                // Start QuickSortAnimation coroutine
                StartCoroutine(QuickSortAnimation(0, numberObjects.Length - 1));
                count++;
            }
        }
        else
        {
            isPaused = true;
        }

        // Update state text
        UpdateStateText();
    }

    void UpdateStateText()
    {
        // Update state text based on play/pause state and count
        stateText.text = isPlaying ? "Playing" : (count == 2 ? "Finished" : "Paused");
    }

    // Coroutine for Quick Sort animation
    IEnumerator QuickSortAnimation(int low, int high)
    {
        if (low < high)
        {
            bool swapsOccurred = false;
            // Call Partition coroutine
            yield return StartCoroutine(Partition(numberObjects, low, high, swapDelay, pivotDisplayDelay, (swaps) => swapsOccurred = swaps));

            int partitionIndex = PartitionIndex;

            if (swapsOccurred)
            {
                // Recursive calls for left and right partitions
                yield return StartCoroutine(QuickSortAnimation(low, partitionIndex - 1));
                yield return StartCoroutine(QuickSortAnimation(partitionIndex + 1, high));
            }

            if (partitionIndex == 5)
            {
                count = 2;
                isPlaying = false;
            }
            // Update state text
            UpdateStateText();
        }
    }

    private int PartitionIndex;

    // Coroutine for partitioning step
    IEnumerator Partition(GameObject[] array, int low, int high, float duration, float pivotDisplayDelay, System.Action<bool> setSwapsOccurred)
    {
        int pivot = GetNumber(array[high]);
        int i = low - 1;

        bool swapsOccurred = false;

        // Create text object to display pivot
        GameObject pivotTextObj = CreateTextObject("Pivot: " + pivot.ToString(), array[high]);
        yield return new WaitForSeconds(pivotDisplayDelay);

        for (int j = low; j < high; j++)
        {
            if (!isPlaying)
            {
                while (isPaused)
                {
                    yield return null; // Wait until resumed
                }
            }

            if (GetNumber(array[j]) < pivot)
            {
                i++;
                // Swap objects
                yield return StartCoroutine(SwapObjects(array[i], array[j], duration));
                swapsOccurred = true;
            }
        }

        // Final swap and update pivot index
        yield return StartCoroutine(SwapObjects(array[i + 1], array[high], duration));
        PartitionIndex = i + 1;

        // Change pivot text
        ChangePivotText(pivotTextObj, "Pivot Complete");

        // Set swaps occurred
        setSwapsOccurred(swapsOccurred);
    }

    // Coroutine to swap two objects
    IEnumerator SwapObjects(GameObject obj1, GameObject obj2, float duration)
    {
        Vector3 pos1 = obj1.transform.localPosition;
        Vector3 pos2 = obj2.transform.localPosition;

        // Move objects
        yield return StartCoroutine(MoveObject(obj1, pos2, duration));
        yield return StartCoroutine(MoveObject(obj2, pos1, duration));

        // Swap elements in array
        int index1 = System.Array.IndexOf(numberObjects, obj1);
        int index2 = System.Array.IndexOf(numberObjects, obj2);
        SwapArrayElements(numberObjects, index1, index2);
    }

    // Coroutine to move an object
    IEnumerator MoveObject(GameObject obj, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;
        Vector3 startingPos = obj.transform.localPosition;

        while (elapsedTime < duration)
        {
            obj.transform.localPosition = Vector3.Lerp(startingPos, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;

            if (!isPlaying)
            {
                isPaused = true; // Pause the coroutine
            }

            yield return null;
        }

        obj.transform.localPosition = targetPosition;

        if (!isPlaying)
        {
            while (isPaused)
            {
                yield return null; // Wait until resumed
            }
        }
    }

    // Get the number from text object
    int GetNumber(GameObject obj)
    {
        TextMeshProUGUI tmpText = obj.GetComponentInChildren<TextMeshProUGUI>();

        if (tmpText != null && int.TryParse(tmpText.text, out int number))
        {
            return number;
        }

        return 0;
    }

    // Swap elements in an array
    void SwapArrayElements<T>(T[] array, int index1, int index2)
    {
        T temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
    }

    // Create text object to display pivot
    GameObject CreateTextObject(string text, GameObject parent)
    {
        GameObject textObj = new GameObject("PivotText");
        textObj.tag = "PivotText"; // Set the tag
        textObj.transform.SetParent(parent.transform);
        textObj.transform.localPosition = new Vector3(18, -50, 0); // Adjust the position as needed

        TextMeshProUGUI textMesh = textObj.AddComponent<TextMeshProUGUI>();
        textMesh.text = text;
        textMesh.color = Color.black;
        textMesh.fontStyle = FontStyles.Bold;

        return textObj;
    }

    // Change pivot text
    void ChangePivotText(GameObject pivotTextObj, string newText)
    {
        TextMeshProUGUI textMesh = pivotTextObj.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null)
        {
            textMesh.text = newText;
        }
    }

    // Save original positions of number objects
    void SaveOriginalPositions()
    {
        int objectCount = numberObjects.Length;

        for (int i = 0; i < objectCount; i++)
        {
            originalPositions[i] = numberObjects[i];
            originalPositionTransforms[i] = numberObjects[i].transform.position;
        }
    }

    // Reset object positions
    void ResetObjectPositions()
    {
        int objectCount = numberObjects.Length;

        for (int i = 0; i < objectCount; i++)
        {
            numberObjects[i] = originalPositions[i];
            numberObjects[i].transform.position = originalPositionTransforms[i];
        }
    }

    // Reset the visualization
    public void Reset()
    {
        StopAllCoroutines(); // Stop any running coroutines
        DestroyPivotTextObjects(); // Destroy pivot text objects
        ResetObjectPositions(); // Reset object positions
        count = 0; // Reset the count
        isPlaying = false; // Reset the play/pause state
        isPaused = false; // Reset the pause state
        UpdateStateText(); // Update the state text
    }

    // Destroy pivot text objects
    void DestroyPivotTextObjects()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PivotText"))
        {
            Destroy(obj);
        }
    }
}

