using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;  // Required for Button component

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private Stun stun;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference tapAction;
    [SerializeField] private Animator animator;
    [SerializeField] private Pop popupManager;
    [SerializeField] private EnemybehaviorM1 enemy;

    private Transform mcSwordTransform;
    private SpriteRenderer spriteRenderer;

    public bool isInvincible = false;
    private bool isDashing = false;
    public float speed;
    public float dashSpeed = 30f;
    public float dashDuration = 0.2f;
    public float XY;
    public float YX;
    public Vector2 currentDirection;
    private Vector3 targetPosition;
    private RaycastHit2D currentHit;

    private void Start()
    {
        mcSwordTransform = transform.GetChild(0);
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentDirection = Vector2.zero;
        isDashing = false;

        moveAction.action.started += ctx => Move(ctx.ReadValue<Vector2>());
        moveAction.action.performed += ctx => Move(ctx.ReadValue<Vector2>());
        moveAction.action.canceled += ctx => Move(Vector2.zero);

        tapAction.action.performed += OnTap;
        popupManager.gameObject.SetActive(false);
    }

    private void Move(Vector2 direction)
    {
        currentDirection = direction;
    }

    private void Update()
    {
        if (!isDashing)
        {
            transform.Translate(currentDirection * speed * Time.deltaTime);
            XY = currentDirection.x + currentDirection.y * speed;
            YX = currentDirection.x + currentDirection.y;
            animator.SetFloat("Speed", Mathf.Abs(XY));

            if (currentDirection.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (currentDirection.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    private void OnTap(InputAction.CallbackContext context)
    {
        Vector2 tapPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(tapPosition);

        currentHit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (currentHit.collider != null && currentHit.collider.CompareTag("Enemy"))
        {
            targetPosition = currentHit.collider.transform.position;
            popupManager.ShowPopup();
            stun.StunAllEnemies();
            ActivateInvincibility();
        }
    }

    public void TriggerDashToPosition()
    {
        if (currentHit.collider != null)
        {
            StartCoroutine(DashToPosition(targetPosition, currentHit.collider.gameObject)); // Pass the target position and the enemy GameObject
        }
    }

    private IEnumerator DashToPosition(Vector3 targetPosition, GameObject enemy)
    {
        if (enemy == null) yield break;

        isDashing = true;
        GameObject slashEffect = Instantiate(slashPrefab, transform.position, Quaternion.identity);
        TrailRenderer trail = slashEffect.GetComponent<TrailRenderer>();
        slashEffect.transform.SetParent(transform);

        float rotationTarget = spriteRenderer.flipX ? 65 : -65;
        StartCoroutine(RotateWeapon(rotationTarget, dashDuration));

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

        if (enemy != null)
        {
            Destroy(enemy); // Destroy the enemy after the dash is complete
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(RotateWeapon(0, 0.3f));
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
        shield.SetActive(true);
    }

    public void ActivateInvincibility()
    {
        StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(5f);
        isInvincible = false;
        shield.SetActive(false);
    }
}
