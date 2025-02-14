using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemybehaviorM1 : MonoBehaviour
{
    Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject mc;
    public float speed;
    private float distance;
    public float followRange;
    public Vector2 direction1;

    private SpriteRenderer childSpriteRenderer;
    private Transform childTransform;
    public Vector3 wpnoffset;

    private bool isStunned = false;
    private float stunDuration;
    private float stunTimer;
    private float originalSpeed;
    private int originalDamage;

    private IObjectPool<EnemybehaviorM1> enemyPool;
    public void SetPool(IObjectPool<EnemybehaviorM1> pool)
    {
        enemyPool = pool;
    }

    private Damage dScript;
    // Start is called before the first frame update
    void Start()
    {
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
                isStunned = false;
                // Reset any effects (e.g., sprite color)
                spriteRenderer.color = Color.white;
                speed = originalSpeed; // Restore the original speed
                dScript.damage = originalDamage;
            }
        }
        else
        {
            distance = Vector2.Distance(transform.position, mc.transform.position);
            Vector2 direction = mc.transform.position - transform.position;

            if (distance <= followRange)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, mc.transform.position, speed * Time.deltaTime);
                animator.SetTrigger("Run");
            }
            else
            {
                animator.SetTrigger("Stand");
            }

            if (direction.x < 0)
            {
                spriteRenderer.flipX = true; // Face left
                childSpriteRenderer.flipX = true;
                childTransform.localPosition = new Vector3(-wpnoffset.x, wpnoffset.y, wpnoffset.z);
            }
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = false; // Face right
                childSpriteRenderer.flipX = false;
                childTransform.localPosition = wpnoffset;
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

        // Stop all movement by setting speed to 0
        
        speed = 0f;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        dScript.damage = 0;
    }



    //void OnMouseDown()
    // {
    // This function is called when the enemy is clicked
    //Debug.Log("Enemy clicked!");
    // Add your desired action here
    //if (chargeCounter.counter == equationGenerator.expectedAnswer)
    //{
    //Debug.Log("CORRECT");
    //equationGenerator.equationText.text = equationGenerator.expectedAnswer.ToString();
    //animator.SetTrigger("Death");
    //speed = 0;
    //}
    //else
    //{
    //Debug.Log("WRONG");
    // }
    //}
    public void GetRekt()
    {
        enemyPool.Release(this);  
    }
}
