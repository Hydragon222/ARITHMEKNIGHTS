using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Add this for scene management
using TMPro; 

public class SettingsMenuManager : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;


    private void Start()
    {
        if (!AudioManager.instance.IsPlaying("Menu"))
        {
            AudioManager.instance.Play("Menu");
        }
    }
    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
        AudioManager.instance.Play("Clicking Sound");
    }

    // Function to return to the Main Menu
    public void BackToMainMenu()
    {
        Debug.Log("Back Button Clicked! Returning to Main Menu...");
        SceneManager.LoadScene("MainMenu"); // Ensure "MainMenu" is the correct scene name
        AudioManager.instance.Play("Clicking Sound");
    }
}
