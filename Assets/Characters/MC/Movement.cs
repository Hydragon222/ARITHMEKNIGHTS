using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float movSpeed;
    float XY;
    Vector2 movement;
    Rigidbody2D rb;
    public Animator animator;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        XY = movement.x + movement.y * movSpeed;
        animator.SetFloat("Speed", Mathf.Abs(XY));

        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }

    }

    private void FixedUpdate() 
    {
        movement.Normalize();
        rb.velocity = new Vector2(movement.x * movSpeed * Time.fixedDeltaTime, movement.y *movSpeed * Time.fixedDeltaTime);
    }
}
