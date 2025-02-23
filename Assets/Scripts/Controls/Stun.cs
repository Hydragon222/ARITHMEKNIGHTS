using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Stun : MonoBehaviour
{
    public float stunDuration = 5f;
    private Button button;
    
    void Start()
    {
        
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StunAllEnemies();
        }
    }

    public void OnButtonPressed()
    {
        button.interactable = false;
        StartCoroutine(CooldownCoroutine());
        StunAllEnemies();
    }
    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(15f); // 10 seconds cooldown
        button.interactable = true; // Re-enable the button
    }
    public void StunAllEnemies()
    {
        // Find all enemies with the tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("stun");
       
        foreach (GameObject enemy in enemies)
        {
            EnemybehaviorM1 enemyBehavior = enemy.GetComponent<EnemybehaviorM1>();
            Damage enemyDamage = enemy.GetComponent<Damage>();
            SpriteRenderer enemySprite = enemy.GetComponent<SpriteRenderer>();
            
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
                // Reset the enemy's state to normal
                enemyBehavior.UnStun();
                // Make sure this method exists in EnemybehaviorM1
            }
        }
    }
}

