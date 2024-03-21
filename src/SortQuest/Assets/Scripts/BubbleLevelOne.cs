using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BubbleLevelOne : MonoBehaviour
{
    // Declare public variables for input fields and continue button
    public TMP_InputField inputOne;
    public TMP_InputField inputTwo;
    public TMP_InputField inputThree;
    public TMP_InputField inputFour;
    public Button continueButton;

    // Declare private variables to track correctness of input fields
    private bool inputOneCorrect;
    private bool inputTwoCorrect;
    private bool inputThreeCorrect;
    private bool inputFourCorrect;

    // Declare serialized field for the finish sound effect
    [SerializeField] AudioSource finishSound;

    private void Start()
    {
        // Subscribe to the "onValueChanged" events for each input field
        inputOne.onValueChanged.AddListener(delegate { CheckInputField(inputOne, "length"); });
        inputTwo.onValueChanged.AddListener(delegate { CheckInputField(inputTwo, "n-1"); });
        inputThree.onValueChanged.AddListener(delegate { CheckInputField(inputThree, "A[j] > A[j+1]"); });
        inputFour.onValueChanged.AddListener(delegate { CheckInputField(inputFour, "swap"); });
    }

    // Method to check the correctness of input fields
    void CheckInputField(TMP_InputField inputField, string correctValue)
    {
        // Convert both the input text and correct value to lowercase and remove spaces
        string inputText = inputField.text.ToLower().Replace(" ", "");
        string correctValueFormatted = correctValue.ToLower().Replace(" ", "");

        // Compare the formatted input text with the formatted correct value
        bool isCorrect = inputText == correctValueFormatted;

        // Set the background color based on correctness
        inputField.GetComponent<Image>().color = isCorrect ? Color.green : Color.white;

        // Update the correctness state based on the input field
        if (inputField == inputOne)
            inputOneCorrect = isCorrect;
        else if (inputField == inputTwo)
            inputTwoCorrect = isCorrect;
        else if (inputField == inputThree)
            inputThreeCorrect = isCorrect;
        else if (inputField == inputFour)
            inputFourCorrect = isCorrect;

        // Check if all inputs are correct
        if (inputOneCorrect && inputTwoCorrect && inputThreeCorrect && inputFourCorrect)
        {
            finishSound.Play();
            continueButton.gameObject.SetActive(true);
        }
    }
}
