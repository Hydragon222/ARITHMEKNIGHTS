using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Pop : MonoBehaviour
{
    public PlayerControls playerControls;

    [SerializeField] private GameObject popupPanel; // Popup panel
    [SerializeField] private Button choiceButton1; // Button 1
    [SerializeField] private Button choiceButton2; // Button 2
    [SerializeField] private Button choiceButton3; // Button 3
    [SerializeField] private TMP_Text questionText; // TextMeshPro text for the question
    [SerializeField] private TMP_Text choiceText1; // Text for Button 1
    [SerializeField] private TMP_Text choiceText2; // Text for Button 2
    [SerializeField] private TMP_Text choiceText3; // Text for Button 3

    private float n1;
    private float ans;

    public float n2;
    public float limit;

    private void Start()
    {
        popupPanel.SetActive(false);
    }

    public void ShowPopup()
    {
        n1 = Random.Range(1, 10);
        ans = n1 * n2;

        // Set the question text
        questionText.text = $"{n1} X {n2} = ?";

        // Enable the panel
        popupPanel.SetActive(true);

        // Randomly assign the correct and incorrect answers to buttons
        RandomizeButtons();

        // Remove existing listeners to prevent duplication
        choiceButton1.onClick.RemoveAllListeners();
        choiceButton2.onClick.RemoveAllListeners();
        choiceButton3.onClick.RemoveAllListeners();

        // Add listeners for the buttons
        choiceButton1.onClick.AddListener(OnButton1Clicked);
        choiceButton2.onClick.AddListener(OnButton2Clicked);
        choiceButton3.onClick.AddListener(OnButton3Clicked);

        // Hide the popup after 5 seconds if no button is clicked
        StartCoroutine(HidePopupAfterDelay(5f));
    }

    private void RandomizeButtons()
    {
        // Shuffle the buttons and assign the correct/incorrect answers
        System.Random random = new System.Random();
        int correctIndex = random.Next(0, 3);

        // Assign correct answer to one of the buttons
        if (correctIndex == 0)
        {
            choiceText1.text = ans.ToString();
        }
        else if (correctIndex == 1)
        {
            choiceText2.text = ans.ToString();
        }
        else
        {
            choiceText3.text = ans.ToString();
        }

        // Assign incorrect answers to the remaining buttons
        if (correctIndex != 0)
        {
            int incorrectAnswer;
            do
            {
                incorrectAnswer = random.Next(1, (int)limit + 1); // Random number from 1 to limit
            } while (incorrectAnswer == (int)ans);

            choiceText1.text = incorrectAnswer.ToString();
        }
        if (correctIndex != 1)
        {
            int incorrectAnswer;
            do
            {
                incorrectAnswer = random.Next(1, (int)limit + 1); // Random number from 1 to limit
            } while (incorrectAnswer == (int)ans);

            choiceText2.text = incorrectAnswer.ToString();
        }
        if (correctIndex != 2)
        {
            int incorrectAnswer;
            do
            {
                incorrectAnswer = random.Next(1, (int)limit + 1); // Random number from 1 to limit
            } while (incorrectAnswer == (int)ans);

            choiceText3.text = incorrectAnswer.ToString();
        }
    }

    private void OnButton1Clicked()
    {
        HandleButtonClick(choiceText1.text);
    }

    private void OnButton2Clicked()
    {
        HandleButtonClick(choiceText2.text);
    }

    private void OnButton3Clicked()
    {
        HandleButtonClick(choiceText3.text);
    }

    private void HandleButtonClick(string buttonText)
    {
        // Check if the clicked button is correct
        if (buttonText == ans.ToString())
        {
            Correct();
        }
        else
        {
            Debug.Log("Incorrect choice!");
        }

        // Hide the popup
        popupPanel.SetActive(false);
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        popupPanel.SetActive(false);
    }

    public void Correct()
    {
        Debug.Log("Correct choice!");
        playerControls.TriggerDashToPosition();
    }
}
