using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Stun : MonoBehaviour
{
    public float stunDuration = 6f;
    private Button button;
    
    void Start()
    {
        
        button = GetComponent<Button>();
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
    void StunAllEnemies()
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
}

