using System.Collections;
using UnityEngine;
using TMPro;

public class BubbleSortVisualization : MonoBehaviour
{
    public GameObject[] numberObjects;
    public float swapDelay = 1.0f;

    private void Start()
    {
        StartCoroutine(BubbleSortAnimation());
    }

    IEnumerator BubbleSortAnimation()
    {
        int n = numberObjects.Length;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                // Compare and swap if needed
                if (GetNumber(numberObjects[j]) > GetNumber(numberObjects[j + 1]))
                {
                    yield return StartCoroutine(SwapObjects(numberObjects[j], numberObjects[j + 1]));
                }
            }
        }
    }

    IEnumerator SwapObjects(GameObject obj1, GameObject obj2)
    {
        Vector3 pos1 = obj1.transform.localPosition;
        Vector3 pos2 = obj2.transform.localPosition;

        // Move objects to swap positions
        StartCoroutine(MoveObject(obj1, pos2, swapDelay));
        StartCoroutine(MoveObject(obj2, pos1, swapDelay));

        // Wait for the swap to complete
        yield return new WaitForSeconds(swapDelay);

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
}
