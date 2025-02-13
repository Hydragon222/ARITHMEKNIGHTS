using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadPracticeLevel()
    {
        Debug.Log("Loading Practice Level: GameScene");
        SceneManager.LoadScene("GameScene");
    }

    public void LoadLevel1()
    {
        Debug.Log("Loading Level 1...");
        SceneManager.LoadScene("Level 1");
    }

    public void LoadLevel2()
    {
        Debug.Log("Loading Level 2...");
        SceneManager.LoadScene("Level 2");
    }

    public void BackToMainMenu()
    {
        Debug.Log("Returning to Main Menu...");
        SceneManager.LoadScene("MainMenu");
    }
}
