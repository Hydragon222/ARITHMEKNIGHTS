using UnityEngine;
using UnityEngine.SceneManagement; // For scene loading
public class MainMenuController : MonoBehaviour
{
    // Method to start the game
    private void Start()
    {
        if (!AudioManager.instance.IsPlaying("Menu"))
        {
            AudioManager.instance.Play("Menu");
        }
    }
    public void StartGame()
    {
        Debug.Log("Start Button Clicked!");
        SceneManager.LoadScene("LevelSelection"); // Load the game scene
        AudioManager.instance.Play("Clicking Sound");
    }

    // Method to open the settings menu
    public void OpenSettings()
    {
        Debug.Log("Settings Button Clicked!");
        SceneManager.LoadScene("SettingsScene");
        AudioManager.instance.Play("Clicking Sound");
    }
    public void OpenCredits()
    {
        Debug.Log("Credits Scene Click");
        SceneManager.LoadScene("CreditsScene");
        AudioManager.instance.Play("Clicking Sound");
    }
    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("Quit Button Clicked!");
        Application.Quit();
        AudioManager.instance.Play("Clicking Sound");
        AudioManager.instance.Stop("Menu");



        // This ensures quitting works in the Unity editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void CreditsMenu()
    {
        Debug.Log("Credits Menu Click");
        SceneManager.LoadScene("CreditsMenu");
        AudioManager.instance.Play("Clicking Sound");
    }





}
