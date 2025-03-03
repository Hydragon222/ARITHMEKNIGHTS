using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PopupQuestion : MonoBehaviour
{
    public PlayerControls playerControls;

    public GameObject questionUIPanel;
    public TMP_Text questionText; // Text for the question
    public Button[] answerButtons; // Array of buttons
    public TMP_Text[] answerTexts; // Texts for the buttons

    public bool divisionMode;
    public bool inverseMode;
    public int n2;
    private int n1;
    private float correctAnswer;
    public int limit; // Ensure a limit is set
    private int inverser;

    private void Start()
    { 
        questionUIPanel.SetActive(false);
    }
    public void GenerateQuestion()
    {
        if (!divisionMode) // Multiplication Mode
        {
            if (inverseMode)
            {
                int[] possibleNumbers = { 1, 2, 3, 4, 5, 10 };
                n2 = Random.Range(1, 11);
                n1 = possibleNumbers[Random.Range(0, possibleNumbers.Length)];
                inverser = n1 * n2;
                correctAnswer = inverser / n1;
            }
            else
            {
                n1 = Random.Range(1, 11);
                correctAnswer = n1 * n2;
            }
        }
        else // **Division Mode (Ensure Whole Number Answer)**
        {
            if (inverseMode)
            {
                int[] possibleNumbers = { 1, 2, 3, 4, 5, 10 };
                n2 = possibleNumbers[Random.Range(0, possibleNumbers.Length)];
                int multiplier = Random.Range(1, 11); // Random whole number result
                n1 = multiplier * n2; // Ensure n1 is a clean multiple of n2
                inverser = n1 / n2; // Exact division
                correctAnswer = n2 * inverser;
            }
            else
            {
                int multiplier = Random.Range(1, 11); // Random whole number result
                n1 = multiplier * n2; // Ensure n1 is a clean multiple of n2
                correctAnswer = (float)n1 / n2; // Exact division
            }
        }

        if (questionText != null)
        {
            if (!inverseMode)
            {
                questionText.text = divisionMode ? $"{n1} ÷ {n2}" : $"{n1} × {n2}";
            } 
            else
            {
                questionText.text = divisionMode ? $"? ÷ {n2} = {inverser}" : $"{n1} × ? = {inverser}";
            }

        }

        // Generate wrong answers (but make sure they are not correct)
        float wrongAnswer1, wrongAnswer2;
        do
        {
            wrongAnswer1 = correctAnswer + Random.Range(1, limit);
        } while (wrongAnswer1 == correctAnswer);

        do
        {
            wrongAnswer2 = Random.Range(1, limit);
        } while (wrongAnswer2 == correctAnswer || wrongAnswer2 == wrongAnswer1);

        // Shuffle answers
        float[] answerOptions = { correctAnswer, wrongAnswer1, wrongAnswer2 };
        ShuffleArray(answerOptions);

        // Assign answers to buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerTexts[i].text = answerOptions[i].ToString(); // Display correct or wrong answer
            answerButtons[i].onClick.RemoveAllListeners();

            if (Mathf.Approximately(answerOptions[i], correctAnswer))
            {
                answerButtons[i].onClick.AddListener(CorrectAnswer);
            }
            else
            {
                answerButtons[i].onClick.AddListener(WrongAnswer);
            }
        }
    }

    // Randomly shuffles an array
    private void ShuffleArray(float[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            float temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
    public void ShowQuestionUI()
    {
        GenerateQuestion();
        if (questionUIPanel != null)
        {
            questionUIPanel.SetActive(true); // Show the question UI

            StartCoroutine(CloseQuestionUIAfterDelay(5f));
        }
        else
        {
            Debug.LogError("Question UI Panel is not assigned in the Inspector!");
        }
    }

    public IEnumerator CloseQuestionUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        questionUIPanel.SetActive(false);
        playerControls.targetEnemy = null;
        playerControls.hasTappedEnemy = false;
    }
    
    public void CorrectAnswer()
    {
        Debug.Log("Answer tapped, checking correctness...");

        if (playerControls.targetEnemy == null)
        {
            Debug.LogError("Error: targetEnemy was lost before dashing!");
            return;
        }

        playerControls.Correct();
        questionUIPanel.SetActive(false);
    }

    // Called when a wrong answer is clicked
    private void WrongAnswer()
    {
        questionUIPanel.SetActive(false);
        playerControls.stun.UnstunAllEnemies();
        playerControls.targetEnemy = null;
        playerControls.hasTappedEnemy = false;
        AudioManager.instance.Play("Wrong");
    }
}

