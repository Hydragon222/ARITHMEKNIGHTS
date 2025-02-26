using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign this in the Inspector

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Hide GameOver panel at the start
        }
        else
        {
            Debug.LogError("GameOverPanel is not assigned in GameOverManager!");
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Debug.LogError("GameOverPanel reference is missing!");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset game speed
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the scene
    }

   
}


