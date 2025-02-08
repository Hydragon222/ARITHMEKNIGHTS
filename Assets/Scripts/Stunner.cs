using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunner : MonoBehaviour
{
    private bool isStunned = false;
    private float stunDuration;
    private float stunTimer;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false;
                // Reset any effects (e.g., sprite color)
                spriteRenderer.color = Color.white;
            }
        }
    }

    public void Stun(float duration)
    {
        isStunned = true;
        stunDuration = duration;
        stunTimer = duration;
        // Apply any visual effects (e.g., change sprite color)
        spriteRenderer.color = Color.blue;

        // Stop all movement
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        Debug.Log("STUN");
    }

    public bool IsStunned()
    {
        return isStunned;
    }
}
