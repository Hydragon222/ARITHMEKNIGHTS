using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Shield : MonoBehaviour
{
    private Button button;
    private PlayerControls playerControls;
    private bool canUseShield = true;
    [SerializeField] private shieldCooldown shieldcooldown;

    void Start()
    {
        button = GetComponent<Button>();
        playerControls = FindObjectOfType<PlayerControls>(); // Finds PlayerControls automatically
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canUseShield)
        {
            UseShield();
        }
    }

    public void OnButtonPressed()
    {
        if (canUseShield)
        {
            UseShield();
        }
    }

    private void UseShield()
    {
        canUseShield = false;
        if (button != null) button.interactable = false; // Disable button if it exists
        playerControls.ShieldMode();
        AudioManager.instance.Play("Shield");
        StartCoroutine(CooldownCoroutine());
        shieldcooldown.StartCooldown();
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(10f);
        canUseShield = true;
        if (button != null) button.interactable = true; // Re-enable button if it exists
    }
}
