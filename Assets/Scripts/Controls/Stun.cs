using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Stun : MonoBehaviour
{
    public float stunDuration = 5f;
    private Button button;
    private bool canUseStun = true;
    [SerializeField] private stunCooldown stuncooldown;

    void Start()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canUseStun)
        {
            UseStun();
        }
    }

    public void OnButtonPressed()
    {
        if (canUseStun)
        {
            UseStun();
        }
    }

    private void UseStun()
    {
        canUseStun = false;
        if (button != null) button.interactable = false; // Disable button if it exists
        StunAllEnemies();
        AudioManager.instance.Play("Stun");
        StartCoroutine(CooldownCoroutine());
        stuncooldown.StartCooldown();
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(15f);
        canUseStun = true;
        if (button != null) button.interactable = true; // Re-enable button if it exists
    }

    public void StunAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("stun");

        foreach (GameObject enemy in enemies)
        {
            EnemybehaviorM1 enemyBehavior = enemy.GetComponent<EnemybehaviorM1>();
            if (enemyBehavior != null)
            {
                enemyBehavior.Stun(stunDuration);
            }
        }
    }

    public void UnstunAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("Unstunning all enemies.");

        foreach (GameObject enemy in enemies)
        {
            EnemybehaviorM1 enemyBehavior = enemy.GetComponent<EnemybehaviorM1>();
            if (enemyBehavior != null)
            {
                enemyBehavior.UnStun();
            }
        }
    }
}

