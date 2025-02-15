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
    }
    private void Start()
    {
        if (!AudioManager.instance.IsPlaying("Menu"))
        {
            AudioManager.instance.Play("Menu");
        }
    }

    // Load Level 1
    public void LoadLevel1()
    {
        Debug.Log("Loading Level 1...");
        SceneManager.LoadScene("Level 1");
        AudioManager.instance.Play("Clicking Sound");
        AudioManager.instance.Stop("Menu");
    }

    // Load Level 2
    public void LoadLevel2()
    {
        Debug.Log("Loading Level 2...");
        SceneManager.LoadScene("Level 2");
        AudioManager.instance.Play("Clicking Sound");
        AudioManager.instance.Stop("Menu");

    }

    // Return to the Main Menu
    public void BackToMainMenu()
    {
        Debug.Log("Returning to Main Menu...");
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.Play("Clicking Sound");
    }

    // Return to the Level Selection Menu
    public void BackToLevelSelection()
    {
        Debug.Log("Returning to Level Selection...");
        SceneManager.LoadScene("LevelSelection");
    }
}
