using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damage;
    private bool isColliding = false;
    private Coroutine damageCoroutine;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerControls playerControls = collision.gameObject.GetComponent<PlayerControls>();
            Health health = collision.gameObject.GetComponent<Health>();

            if (playerControls != null && health != null)
            {
                if (!isColliding)
                {
                    isColliding = true;
                    damageCoroutine = StartCoroutine(ApplyDamageOverTime(playerControls, health));
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && isColliding)
        {
            isColliding = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
        }
    }

    private IEnumerator ApplyDamageOverTime(PlayerControls playerControls, Health health)
    {
        while (isColliding)
        {
            if (!playerControls.isInvincible)
            {
                health.TakeDamage(damage);
            }
            else
            {
                Debug.Log("Player is invincible and takes no damage!");
            }

            yield return new WaitForSeconds(0.4f);
        }
    }
}
