using System.Collections;
using UnityEngine;
using TMPro;

public class QuickSortVisualization : MonoBehaviour
{
    public GameObject[] numberObjects;
    public float swapDelay = 1.0f;
    public float pivotDisplayDelay = 1.5f;

    private void Start()
    {
        StartCoroutine(QuickSortAnimation(0, numberObjects.Length - 1));
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
        }
    }

    private int PartitionIndex;

    IEnumerator Partition(GameObject[] array, int low, int high, float duration, float pivotDisplayDelay, System.Action<bool> setSwapsOccurred)
    {
        int pivot = GetNumber(array[high]);
        int i = low - 1;

        bool swapsOccurred = false;

        // Create and show the pivot text
        GameObject pivotTextObj = CreateTextObject("Pivot: " + pivot.ToString(), array[high]);
        yield return new WaitForSeconds(pivotDisplayDelay);

        for (int j = low; j < high; j++)
        {
            if (GetNumber(array[j]) < pivot)
            {
                i++;
                yield return StartCoroutine(SwapObjects(array[i], array[j], duration));
                swapsOccurred = true;
            }
        }

        yield return StartCoroutine(SwapObjects(array[i + 1], array[high], duration));
        PartitionIndex = i + 1;

        // Change the text of the pivot object
        ChangePivotText(pivotTextObj, "Pivot Complete");

        setSwapsOccurred(swapsOccurred);
    }

    IEnumerator SwapObjects(GameObject obj1, GameObject obj2, float duration)
    {
        Vector3 pos1 = obj1.transform.localPosition;
        Vector3 pos2 = obj2.transform.localPosition;

        // Move objects to swap positions
        yield return StartCoroutine(MoveObject(obj1, pos2, duration));
        yield return StartCoroutine(MoveObject(obj2, pos1, duration));

        // Swap positions in the array
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
            yield return null;
        }

        obj.transform.localPosition = targetPosition;
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
