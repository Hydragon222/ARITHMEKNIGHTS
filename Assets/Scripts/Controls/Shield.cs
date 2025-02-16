using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Shield : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnButtonPressed()
    { 
        // Disable the button and start the cooldown coroutine
        button.interactable = false;
        StartCoroutine(CooldownCoroutine());
        AudioManager.instance.Play("Shield");
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(10f); // 10 seconds cooldown
        button.interactable = true; // Re-enable the button
    }
}
