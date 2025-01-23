using UnityEngine;
using UnityEngine.SceneManagement; // For scene loading

public class MainMenuController : MonoBehaviour
{
    // Method to start the game
    public void StartGame()
    {
        Debug.Log("Start Button Clicked!");
        // Load the game scene (replace "GameScene" with your scene name)
        SceneManager.LoadScene("GameScene");
    }

    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("Quit Button Clicked!");
        // Quit the application
        Application.Quit();

        // NOTE: Application.Quit() does not work in the Unity editor.
        // Use this line in the editor for testing:
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
