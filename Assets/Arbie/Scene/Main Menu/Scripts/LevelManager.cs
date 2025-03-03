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

    // Load Level 1 to Level 12
    public void LoadLevel1() { LoadLevel(1); }
    public void LoadLevel2() { LoadLevel(2); }
    public void LoadLevel3() { LoadLevel(3); }
    public void LoadLevel4() { LoadLevel(4); }
    public void LoadLevel5() { LoadLevel(5); }
    public void LoadLevel6() { LoadLevel(6); }
    public void LoadLevel7() { LoadLevel(7); }
    public void LoadLevel8() { LoadLevel(8); }
    public void LoadLevel9() { LoadLevel(9); }
    public void LoadLevel10() { LoadLevel(10); }
    public void LoadLevel11() { LoadLevel(11); }
    public void LoadLevel12() { LoadLevel(12); }

    // Load any level dynamically from level1 to level12
    public void LoadLevel(int levelNumber)
    {
        if (levelNumber < 1 || levelNumber > 12)
        {
            Debug.LogError("Invalid level number! Level must be between 1 and 12.");
            return;
        }

        string levelName = "level" + levelNumber;
        Debug.Log("Loading " + levelName + "...");
        SceneManager.LoadScene(levelName);

        AudioManager.instance.Play("Clicking Sound");
        AudioManager.instance.Stop("Menu");
        AudioManager.instance.Play("Level " + levelNumber + " Theme");
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
