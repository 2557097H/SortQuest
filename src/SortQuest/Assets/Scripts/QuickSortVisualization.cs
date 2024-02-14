using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuickSortVisualization : MonoBehaviour
{
    public GameObject[] numberObjects;
    public float swapDelay = 1.0f;
    public float pivotDisplayDelay = 1.5f;

    public TextMeshProUGUI stateText; // Text element to display the state
    private bool isPlaying = false; // Flag to control play/pause
    private bool isPaused = false; // Flag to indicate if the coroutine is paused
    private int count = 0;

    void Start()
    {

    }

    void Update()
    {
        // No need for keyboard input checking here, as buttons will handle it
    }

    public void TogglePlayPause()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            isPaused = false;
            if(count == 0)
            {
                StartCoroutine(QuickSortAnimation(0, numberObjects.Length - 1));
                count++;

            }
            
        }
        else
        {
            isPaused = true;
        }

        UpdateStateText();
    }

    void UpdateStateText()
    {
        stateText.text = isPlaying ? "Playing" : (count == 2 ? "Finished" : "Paused");
    }

    IEnumerator QuickSortAnimation(int low, int high)
    {
        if (low < high)
        {
            bool swapsOccurred = false;
            yield return StartCoroutine(Partition(numberObjects, low, high, swapDelay, pivotDisplayDelay, (swaps) => swapsOccurred = swaps));

            int partitionIndex = PartitionIndex;

            if (swapsOccurred)
            {
                yield return StartCoroutine(QuickSortAnimation(low, partitionIndex - 1));
                yield return StartCoroutine(QuickSortAnimation(partitionIndex + 1, high));
            }

            if (partitionIndex == 5){
                count = 2;
                isPlaying = false;
            }
            UpdateStateText();
        }
    }

    private int PartitionIndex;

    IEnumerator Partition(GameObject[] array, int low, int high, float duration, float pivotDisplayDelay, System.Action<bool> setSwapsOccurred)
    {
        int pivot = GetNumber(array[high]);
        int i = low - 1;

        bool swapsOccurred = false;

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
                yield return StartCoroutine(SwapObjects(array[i], array[j], duration));
                swapsOccurred = true;
            }
        }

        yield return StartCoroutine(SwapObjects(array[i + 1], array[high], duration));
        PartitionIndex = i + 1;

        ChangePivotText(pivotTextObj, "Pivot Complete");

        setSwapsOccurred(swapsOccurred);
    }

    IEnumerator SwapObjects(GameObject obj1, GameObject obj2, float duration)
    {
        Vector3 pos1 = obj1.transform.localPosition;
        Vector3 pos2 = obj2.transform.localPosition;

        yield return StartCoroutine(MoveObject(obj1, pos2, duration));
        yield return StartCoroutine(MoveObject(obj2, pos1, duration));

        int index1 = System.Array.IndexOf(numberObjects, obj1);
        int index2 = System.Array.IndexOf(numberObjects, obj2);
        SwapArrayElements(numberObjects, index1, index2);
    }

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

    int GetNumber(GameObject obj)
    {
        TextMeshProUGUI tmpText = obj.GetComponentInChildren<TextMeshProUGUI>();

        if (tmpText != null && int.TryParse(tmpText.text, out int number))
        {
            return number;
        }

        return 0;
    }

    void SwapArrayElements<T>(T[] array, int index1, int index2)
    {
        T temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
    }

    GameObject CreateTextObject(string text, GameObject parent)
    {
        GameObject textObj = new GameObject("PivotText");
        textObj.transform.SetParent(parent.transform);
        textObj.transform.localPosition = new Vector3(18, -50, 0); // Adjust the position as needed

        TextMeshProUGUI textMesh = textObj.AddComponent<TextMeshProUGUI>();
        textMesh.text = text;
        textMesh.color = Color.black;
        textMesh.fontStyle = FontStyles.Bold;

        return textObj;
    }

    void ChangePivotText(GameObject pivotTextObj, string newText)
    {
        TextMeshProUGUI textMesh = pivotTextObj.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null)
        {
            textMesh.text = newText;
        }
    }
}
