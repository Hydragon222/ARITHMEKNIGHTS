using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damage;
    private Transform weaponTransform;
    private bool isColliding = false;
    private Coroutine damageCoroutine;
    private SpriteRenderer spriteRenderer;
    private Stun stun;
    public SpriteRenderer player;
    private EnemybehaviorM1 enemybehaviorm1;
    private void Start()
    {
        weaponTransform = transform.GetChild(0);
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemybehaviorm1 = GetComponent<EnemybehaviorM1>();
        stun = GetComponent<Stun>();
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                SpriteRenderer playerSprite = playerObject.GetComponent<SpriteRenderer>();
                SetPlayerSprite(playerSprite);
            }
            else
            {
                Debug.LogError("Player GameObject with tag 'Player' not found!");
            }
        }

    }
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
    public void SetPlayerSprite(SpriteRenderer playerSprite)
    {
        player = playerSprite;
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
            weaponTransform.rotation = Quaternion.Euler(0, 0, 0);
            player.color = Color.white;
        }
    }

    private IEnumerator ApplyDamageOverTime(PlayerControls playerControls, Health health)
    {
        while (isColliding)
        {
            if (!playerControls.isInvincible && !enemybehaviorm1.isStunned)
            {
                health.TakeDamage(damage);
                player.color = Color.red;
                // Rotate the weapon based on the enemy's facing direction
                if (spriteRenderer.flipX)
                {
                    // Counter-clockwise when facing left
                    weaponTransform.rotation = Quaternion.Euler(0, 0, 65);
                }
                else
                {
                    // Clockwise when facing right
                    weaponTransform.rotation = Quaternion.Euler(0, 0, -65);
                }

                yield return new WaitForSeconds(0.2f); // Small delay to make the rotation noticeable
                player.color = Color.white;
                // Reset the weapon rotation
                weaponTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                Debug.Log("Player is invincible and takes no damage!");
            }
            
            yield return new WaitForSeconds(0.4f);
        }
    }
    private void Update()
    {
   
    }
}

