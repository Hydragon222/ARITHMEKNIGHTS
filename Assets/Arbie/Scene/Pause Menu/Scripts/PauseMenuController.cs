using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign in Inspector
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Press ESC to toggle pause
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                OpenPauseMenu();
            }
        }
    }

    public void OpenPauseMenu()
    {
        AudioManager.instance.Play("Clicking Sound");
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true); // Show Pause Menu
            Time.timeScale = 0f; // Pause the game
            isPaused = true;
            AudioManager.instance.PauseLevelMusic();
        }
        else
        {
            Debug.LogError("Pause Menu UI is not assigned in the Inspector!");
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // Hide Pause Menu
            Time.timeScale = 1f; // Resume game
            isPaused = false;
            AudioManager.instance.Play("Clicking Sound");
            AudioManager.instance.ResumeLevelMusic();
        }
    }

    public void QuitToLevelSelection()
    {
        Time.timeScale = 1f; // Ensure time scale is reset before loading a new scene
        SceneManager.LoadScene("LevelSelection"); // Load Level Selection Scene
        AudioManager.instance.Play("Clicking Sound");
    }
}
