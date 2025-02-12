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

    private void Start()
    {
        mcSwordTransform = transform.GetChild(0);
        spriteRenderer = GetComponent<SpriteRenderer>();

        Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();
        transform.Translate(moveDirection * speed * Time.deltaTime);

        currentDirection = moveDirection;
        isDashing = false;

        tapAction.action.performed += OnTap;
        popupManager.gameObject.SetActive(false);
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

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            targetPosition = hit.collider.transform.position;
            popupManager.ShowPopup();
            stun.StunAllEnemies();
            ActivateInvincibility();

        }
    }
    public void TriggerDashToPosition()
    {
        StartCoroutine(DashToPosition(targetPosition));
        StartCoroutine(enemy.GetRektCoroutine());
    }

    private IEnumerator DashToPosition(Vector3 targetPosition)
    {
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
