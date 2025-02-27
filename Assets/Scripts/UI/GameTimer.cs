using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    private TMP_Text timerText; // Assign in Inspector
    public float gameTime = 300f; // Default: 5 minutes (300 sec)
    [SerializeField] private bool countDown = true; // True = countdown, False = count up
    private bool isRunning = true;
    
    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        if (countDown)
        {
            UpdateTimerDisplay(gameTime); // Start at max time
        }
        else
        {
            UpdateTimerDisplay(0); // Start from 0
        }
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second

            if (countDown)
            {
                gameTime--;
                if (gameTime <= 0)
                {
                    gameTime = 0;
                    isRunning = false;
                    TimerEnded();
                }
            }
            else
            {
                gameTime++;
            }

            UpdateTimerDisplay(gameTime);
        }
    }

    private void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; // Format MM:SS
    }

    private void TimerEnded()
    {
        Debug.Log("Time's up!");
        // You can add anything here (Game Over, Scene Change, etc.)
    }
}
