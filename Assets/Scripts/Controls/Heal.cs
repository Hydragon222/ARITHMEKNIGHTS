using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Heal : MonoBehaviour
{
    private Button button;
    [SerializeField] private Health health1;
    private bool canUseHeal = true;

    void Start()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canUseHeal)
        {
            UseHeal();
        }
    }

    public void OnButtonPressed()
    {
        if (canUseHeal)
        {
            UseHeal();
        }
    }

    private void UseHeal()
    {
        canUseHeal = false;
        if (button != null) button.interactable = false; // Disable button if it exists
        health1.health = health1.maxHealth;
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(30f);
        canUseHeal = true;
        if (button != null) button.interactable = true; // Re-enable button if it exists
    }
}
