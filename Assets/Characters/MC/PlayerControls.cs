using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;  // Existing move action reference
    [SerializeField] private InputActionReference tapAction;   // Tap action reference
    public float speed;
    public float XY;
    public float YX;
    public Vector2 currentDirection;
    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();
        transform.Translate(moveDirection * speed * Time.deltaTime);

        currentDirection = moveDirection;

        // Enable the tap action
        tapAction.action.performed += OnTap;
    }

    private void Update()
    {
        Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();
        transform.Translate(moveDirection * speed * Time.deltaTime);
        currentDirection = moveDirection;
        XY = moveDirection.x + moveDirection.y * speed;
        YX = moveDirection.x + moveDirection.y;
        animator.SetFloat("Speed", Mathf.Abs(XY));

        if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnTap(InputAction.CallbackContext context)
    {
        Debug.Log("OnTap method called");

        Vector2 tapPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(tapPosition);

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            Debug.Log("Tapped");
        }
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        tapAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        tapAction.action.Disable();
    }
}
