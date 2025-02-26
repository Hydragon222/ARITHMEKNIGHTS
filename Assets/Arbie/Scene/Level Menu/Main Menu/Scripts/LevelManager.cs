using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Load the Practice Level (GameScene)
    public void LoadPracticeLevel()
    {
        Debug.Log("Loading Practice Level: GameScene");
        SceneManager.LoadScene("GameScene");
        AudioManager.instance.Play("Clicking Sound");
        AudioManager.instance.Stop("Menu");
        AudioManager.instance.Play("Practice Level Theme");
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioManager.instance.PlaySceneMusic(scene.name);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Load Level 1
    public void LoadLevel1()
    {
        Debug.Log("Loading Level 1...");
        SceneManager.LoadScene("Level 1");
        AudioManager.instance.Play("Clicking Sound");
        AudioManager.instance.Stop("Menu");
        AudioManager.instance.Play("Level 1 Theme");
    }

    // Load Level 2
    public void LoadLevel2()
    {
        Debug.Log("Loading Level 2...");
        SceneManager.LoadScene("Level 2");
        AudioManager.instance.Play("Clicking Sound");
        AudioManager.instance.Stop("Menu");
        AudioManager.instance.Play("Level 2 Theme");

    }

    // Return to the Main Menu
    public void BackToMainMenu()
    {
        Debug.Log("Returning to Main Menu...");
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.Play("Clicking Sound");

        // Stop all sounds to prevent duplicates
        AudioManager.instance.StopAll();

        // Only play the menu music if it's not already playing
        if (!AudioManager.instance.IsPlaying("Menu"))
        {
            AudioManager.instance.Play("Menu");
        }
    }
    
}
