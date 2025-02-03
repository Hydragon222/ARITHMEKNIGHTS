using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Add this for scene management
using TMPro; 

public class SettingsMenuManager : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown; 

    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value); 
    }

    // Function to return to the Main Menu
    public void BackToMainMenu()
    {
        Debug.Log("Back Button Clicked! Returning to Main Menu...");
        SceneManager.LoadScene("MainMenu"); // Ensure "MainMenu" is the correct scene name
    }
}
