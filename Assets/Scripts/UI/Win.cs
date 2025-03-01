using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField] private GameTimer timer;
    [SerializeField] private TMP_Text victoryTimeText;
    
    private float victoryTime;

    public void Winned()
    {
        Time.timeScale = 0f;
        victoryTime = timer.gameTime;
        if (victoryTimeText != null)
        {
            int minutes = Mathf.FloorToInt(victoryTime / 60);
            int seconds = Mathf.FloorToInt(victoryTime % 60);
            victoryTimeText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }
}
