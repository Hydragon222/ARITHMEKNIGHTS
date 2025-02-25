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
    public float n2;
    private float n1, correctAnswer;
    public int limit; // Ensure a limit is set

    private void Start()
    { 
        questionUIPanel.SetActive(false);
    }
    public void GenerateQuestion()
    {
        n1 = Random.Range(1, 10);
        if (divisionMode == false)
        {
            correctAnswer = n1 * n2;
        }
        else if (divisionMode == true)
        {
            correctAnswer = n1 / n2;
        }

        if (questionText != null)
        {
            if (divisionMode == false)
            {
            questionText.text = $"{n1} × {n2}?";
            } else if (divisionMode == true)
            {
                questionText.text = $"{n1} ÷ {n2}?";
            }

        }

        // Generate wrong answers
        float wrongAnswer1, wrongAnswer2;
        do
        {
            wrongAnswer1 = n1 * Random.Range(1, limit);
        } while (wrongAnswer1 == correctAnswer);

        do
        {
            wrongAnswer2 = n2 * Random.Range(1, limit);
        } while (wrongAnswer2 == correctAnswer || wrongAnswer2 == wrongAnswer1);

        // Shuffle answers
        float[] answerOptions = { correctAnswer, wrongAnswer1, wrongAnswer2 };
        ShuffleArray(answerOptions);

        // Assign answers to buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerTexts[i].text = answerOptions[i].ToString();
            answerButtons[i].onClick.RemoveAllListeners(); // Clear old listeners

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
        float elapsedTime = 0f;

        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        questionUIPanel.SetActive(false); // Hide the question UI
        playerControls.targetEnemy = null;
        playerControls.hasTappedEnemy = false;
    }
    // Called when the correct answer is clicked
    public void CorrectAnswer()
    {
        Debug.Log("Answer tapped, checking correctness...");

        if (playerControls.targetEnemy == null)
        {
            Debug.LogError("Error: targetEnemy was lost before dashing!");
            return;
        }

        Debug.Log("Correct answer! Proceeding with dash...");
        playerControls.Correct();
        questionUIPanel.SetActive(false);
    }

    // Called when a wrong answer is clicked
    private void WrongAnswer()
    {
        Debug.Log("Wrong Answer! Try Again.");
        questionUIPanel.SetActive(false);
        playerControls.stun.UnstunAllEnemies();
        playerControls.targetEnemy = null;
        playerControls.hasTappedEnemy = false;
        AudioManager.instance.Play("Wrong");
    }
}

