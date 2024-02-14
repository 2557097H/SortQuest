using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BubbleSortVisualization : MonoBehaviour
{
    public GameObject[] numberObjects;
    public float swapDelay = 1.0f;

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

        if (!isPlaying)
        {
            isPaused = true; // Set the flag to pause the coroutine
        }
        else
        {
            if (count == 0)
            {
                StartCoroutine(BubbleSortAnimation());
                count++;
            }
            isPaused = false; // Set the flag to resume the coroutine
        }

        UpdateStateText();
    }

    void UpdateStateText()
    {
        Debug.Log(count);
        stateText.text = isPlaying ? "Playing" : (count == 2 ? "Finished" : "Paused");
        Debug.Log(stateText.text);
    }

    IEnumerator BubbleSortAnimation()
    {
        int n = numberObjects.Length;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (!isPlaying)
                {
                    while (isPaused)
                    {
                        yield return null; // Wait until resumed
                    }
                }

                if (GetNumber(numberObjects[j]) > GetNumber(numberObjects[j + 1]))
                {
                    yield return StartCoroutine(SwapObjects(numberObjects[j], numberObjects[j + 1]));
                }
            }

            yield return new WaitForSeconds(swapDelay);
        }

        count = 2;
        isPlaying = false;
        UpdateStateText();
    }

    IEnumerator SwapObjects(GameObject obj1, GameObject obj2)
    {
        Vector3 pos1 = obj1.transform.localPosition;
        Vector3 pos2 = obj2.transform.localPosition;

        StartCoroutine(MoveObject(obj1, pos2, swapDelay));
        StartCoroutine(MoveObject(obj2, pos1, swapDelay));

        yield return new WaitForSeconds(swapDelay);

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
}
