using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobileUI : MonoBehaviour
{
    
[SerializeField] private GameObject mobileUICanvas; // Assign your mobile UI Canvas in Inspector

    private void Start()
    {
        // Check if the platform is PC or mobile
        if (IsRunningOnPC())
        {
            Debug.Log("PC detected – Hiding Mobile UI");
            mobileUICanvas.SetActive(false); // Disable Mobile UI on PC
        }
        else
        {
            Debug.Log("Mobile detected – Showing Mobile UI");
            mobileUICanvas.SetActive(true); // Keep Mobile UI on Mobile
        }
    }

    private bool IsRunningOnPC()
    {
        // Returns true if running on Windows, Mac, or Linux
        return Application.platform == RuntimePlatform.WindowsPlayer ||
               Application.platform == RuntimePlatform.OSXPlayer ||
               Application.platform == RuntimePlatform.LinuxPlayer ||
               Application.platform == RuntimePlatform.WindowsEditor; // Also applies in Play Mode
    }
}

