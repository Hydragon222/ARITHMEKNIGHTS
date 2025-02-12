using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private Stun stun;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private InputActionReference moveAction;  // Existing move action reference
    [SerializeField] private InputActionReference tapAction;   // Tap action reference
    [SerializeField] private Animator animator;

    public Pop pop;
    private Transform mcSwordTransform; 
    private SpriteRenderer spriteRenderer;

    public bool isInvincible = false;
    private bool isDashing = false;
    public float speed;
    public float dashSpeed = 30f; // Dash speed
    public float dashDuration = 0.2f; // Dash duration
    public float XY;
    public float YX;
    public Vector2 currentDirection;
    private RaycastHit2D currentHit;
    private void Start()
    {
        mcSwordTransform = transform.GetChild(0);
        spriteRenderer = GetComponent<SpriteRenderer>();

        Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();
        transform.Translate(moveDirection * speed * Time.deltaTime);

        currentDirection = moveDirection;
        isDashing = false;
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
        Vector2 tapPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(tapPosition);

        currentHit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (currentHit.collider != null && currentHit.collider.CompareTag("Enemy"))
        {
            ActivateInvincibility();
            stun.StunAllEnemies();
            Debug.Log("Tapped");
            pop.ShowPopup();
        // !!!!!!!!ARBIEEE DIRI BUTANG ANG POP UP QUESTIONN!!!!!!!
        }
        
    }
    public void TriggerDashToPosition()
    {
    }

    private IEnumerator RotateWeapon(float targetAngle, float duration)
    {
        float startAngle = mcSwordTransform.rotation.eulerAngles.z;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float currentAngle = Mathf.LerpAngle(startAngle, targetAngle, elapsed / duration);
            mcSwordTransform.rotation = Quaternion.Euler(0, 0, currentAngle);
            yield return null;
        }

        mcSwordTransform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }
    private IEnumerator DashToPosition(Vector3 targetPosition)
    {
        isDashing = true;

        GameObject slashEffect = Instantiate(slashPrefab, transform.position, Quaternion.identity);
        TrailRenderer trail = slashEffect.GetComponent<TrailRenderer>();
        slashEffect.transform.SetParent(transform);

        float dashTime = 0f;
        Vector3 startPosition = transform.position;

        while (dashTime < dashDuration)
        {
            dashTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, dashTime / dashDuration);
            yield return null;
        }

        transform.position = targetPosition;
        isDashing = false;
        Destroy(slashEffect, trail.time);
        slashEffect.transform.SetParent(null);

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(RotateWeapon(0, 0.3f));
    }
    public IEnumerator Right()
    {
        StartCoroutine(DashToPosition(currentHit.collider.transform.position));
        yield return new WaitForSeconds(0.5f);
        Destroy(currentHit.collider.gameObject);
    }
    private void OnEnable()
    {
        moveAction.action.Enable();
        tapAction.action.Enable();
        shield.SetActive(false);
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        tapAction.action.Disable();
        shield.SetActive(false);
        
    }

    public void ShieldMode()
    {
        StartCoroutine(InvincibilityCoroutine());
        Debug.Log("Shield up");
        shield.SetActive(true);
    }

    public void ActivateInvincibility()
    {
        StartCoroutine(InvincibilityCoroutine());
        
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        Debug.Log("IMMORTALITY OR DEATH");

        yield return new WaitForSeconds(5f);  // Wait for 5 seconds

        isInvincible = false;
        Debug.Log("OVER");
        shield.SetActive(false);
    }
}
