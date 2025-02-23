using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Heal : MonoBehaviour
{
    private Button button;
    [SerializeField] private Health health1;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }
    public void OnButtonPressed()
    {
        button.interactable = false;
        StartCoroutine(CooldownCoroutine());
        heal();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            heal();
        }
    }

    private void heal()
    {
        health1.health = health1.maxHealth;
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(30f);
        button.interactable = true; // Re-enable the button
    }
}
