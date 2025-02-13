using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign in Inspector

    public void OpenPauseMenu()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true); // Show Pause Menu
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
        }
    }

    public void QuitToLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection"); // Load Level Selection Scene
    }
}
