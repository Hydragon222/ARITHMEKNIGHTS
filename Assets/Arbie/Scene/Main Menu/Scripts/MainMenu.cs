using UnityEngine;
using UnityEngine.SceneManagement; // For scene loading
public class MainMenuController : MonoBehaviour
{
    // Method to start the game
    public void StartGame()
    {
        Debug.Log("Start Button Clicked!");
        SceneManager.LoadScene("LevelSelection"); // Load the game scene
    }

    // Method to open the settings menu
    public void OpenSettings()
    {
        Debug.Log("Settings Button Clicked!");
        SceneManager.LoadScene("SettingsScene");
    }

    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("Quit Button Clicked!");
        Application.Quit();

        // This ensures quitting works in the Unity editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
