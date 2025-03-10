﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemybehaviorM1 : MonoBehaviour
{
    Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    private GameObject mc;
    private SpriteRenderer mcSpriteRenderer;
    public float speed;
    private float distance;
    public float followRange;
    public Vector2 direction1;

    private SpriteRenderer childSpriteRenderer;
    private Transform childTransform;
    private GameObject child;
    public Vector3 wpnoffset;

    public bool isStunned = false;
    public bool isDying = false;
    private float stunDuration;
    private float stunTimer;
    private float originalSpeed;
    private int originalDamage;

    private IObjectPool<EnemybehaviorM1> enemyPool;
    public void SetPool(IObjectPool<EnemybehaviorM1> pool)
    {
        enemyPool = pool;
    }

    public void SetTarget(GameObject player)
    {
        mc = player;
        mcSpriteRenderer = mc.GetComponent<SpriteRenderer>();
        Damage damageScript = GetComponent<Damage>();
        if (damageScript != null)
        {
            damageScript.SetPlayerSprite(mcSpriteRenderer);
        }
    }


    private Damage dScript;

    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0).gameObject;

        dScript = GetComponent<Damage>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (transform.childCount > 0) // ✅ Check if the object has children
        {
            childTransform = transform.GetChild(0);
            
            childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();

            if (childSpriteRenderer == null)
            {
                Debug.LogError("No SpriteRenderer found on " + childTransform.name);
            }
        }
        else
        {
            Debug.LogError("No child object found on " + gameObject.name + "! childTransform is NULL.");
            childTransform = null; // Prevent further errors
        }

        wpnoffset = childTransform != null ? childTransform.localPosition : Vector3.zero;
        originalSpeed = speed;
        if (dScript != null)
            originalDamage = dScript.damage;


    }
    void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                UnStun();
            }

            // Prevent running while stunned
            animator.ResetTrigger("Run");
            animator.SetTrigger("Stand");
            rb.velocity = Vector2.zero; // Stop movement
            return; // Stop further processing
        }

        if (mc != null)
        {
            distance = Vector2.Distance(transform.position, mc.transform.position);
            Vector2 direction = mc.transform.position - transform.position;

            if (distance <= followRange)
            {
                // 💡 Ensure enemy STARTS running immediately
                animator.ResetTrigger("Stand"); // Remove Stand state immediately
                animator.SetTrigger("Run"); // Ensure Run animation starts

                transform.position = Vector2.MoveTowards(this.transform.position, mc.transform.position, speed * Time.deltaTime);
            }
            else
            {
                animator.ResetTrigger("Run"); // Remove Run state
                animator.SetTrigger("Stand"); // Set Stand animation
            }

            if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
                childSpriteRenderer.flipX = true;
                childTransform.localPosition = new Vector3(-wpnoffset.x, wpnoffset.y, wpnoffset.z);
            }
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
                childSpriteRenderer.flipX = false;
                childTransform.localPosition = wpnoffset;
            }
        }
        else
        {
            Debug.LogWarning("No player assigned to follow!");
        }
    }



    public void Stun(float duration)
    {
        isStunned = true;
        stunDuration = duration;
        stunTimer = duration;
        // Apply any visual effects (e.g., change sprite color)
        spriteRenderer.color = Color.blue;

        // Stop all movement by setting speed to 0

        speed = 0f;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        dScript.damage = 0;
        childSpriteRenderer.color = Color.blue;

        animator.SetTrigger("Stand");
    }

    public void UnStun()
    {
        isStunned = false;
        // Reset any effects (e.g., sprite color)
        spriteRenderer.color = Color.white;
        childSpriteRenderer.color = Color.white;
        speed = originalSpeed; // Restore the original speed
        dScript.damage = originalDamage;
        if (Vector2.Distance(transform.position, mc.transform.position) <= followRange)
        {
            animator.ResetTrigger("Stand");
            animator.SetTrigger("Run"); // Start running instantly
        }
    }

    public void GetRekt()
    {
        isDying = true;

        animator.SetTrigger("Death");
        speed = 0;
        dScript.damage = 0;

        child.SetActive(false);
        rb.simulated = false;
        
        childSpriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        StartCoroutine(WaitAndRelease());
    }

    private IEnumerator WaitAndRelease()
    {
        speed = 0;
        dScript.damage = 0;
        // Wait for 4 seconds before releasing
        yield return new WaitForSeconds(2f);
        child.SetActive(true);
        
        rb.simulated = true;
        childSpriteRenderer.color = Color.white;
        spriteRenderer.color = Color.white;
        // Release the enemy back into the pool
        enemyPool.Release(this);
        isDying = false;
    }
}
