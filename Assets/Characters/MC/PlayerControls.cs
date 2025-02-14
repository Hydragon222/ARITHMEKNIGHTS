using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private Stun stun;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private InputActionReference moveAction;  // Existing move action reference
    [SerializeField] private InputActionReference tapAction;   // Tap action reference
    [SerializeField] private Animator animator;
    public PopupQuestion popupQuestion;

    private Transform mcSwordTransform;
    private SpriteRenderer spriteRenderer;
    private RaycastHit2D hit;
    private GameObject lastTappedEnemy;
    public Transform targetEnemy;

    public bool isInvincible = false;
    private bool isDashing = false;
    public float speed;
    public float dashSpeed = 30f; // Dash speed
    public float dashDuration = 0.2f; // Dash duration
    public float XY;
    public float YX;
    public Vector2 currentDirection;

    private void Start()
    {
        targetEnemy = GameObject.FindWithTag("Enemy")?.transform;
        if (targetEnemy != null)
        {
            Debug.Log("TEST: Enemy found at start: " + targetEnemy.name);
        }
        else
        {
            Debug.LogWarning("TEST: No enemy found at start.");
        }
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
        if (XY == 0) // If the player is moving
        {
            return;
        }
        if (XY == 0) // If the player is moving
        {
            FindAnyObjectByType<AudioManager>().Stop("Walking"); // Stop when the player stops moving
        }
        else
        {
            if (!FindAnyObjectByType<AudioManager>().IsPlaying("Walking")) // Play only if not already playing
            {
                FindAnyObjectByType<AudioManager>().Play("Walking");
            }
        }
    }
<<<<<<< HEAD

    

    


=======
>>>>>>> 2e3bf10d66722eb6ad84fca1daa0dab3e0d224bf
    private void OnTap(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Tap detected on UI, ignoring.");
            return;
        }
        Vector2 tapPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(tapPosition);

        hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        { 
            targetEnemy = hit.collider.transform;
            Debug.Log("Enemy stored: " + targetEnemy.name);

            stun.StunAllEnemies();
            ActivateInvincibility();
            if (spriteRenderer.flipX)
            {
                mcSwordTransform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                mcSwordTransform.rotation = Quaternion.Euler(0, 0, -90);
            }
            
            popupQuestion.ShowQuestionUI();
        }
        else
        {
            Debug.LogWarning("No enemy detected on tap.");
        }
    }



    public void Correct()
    {
        if (hit.collider == null)
        {
            Debug.LogError("Error: No enemy stored! Cannot dash.");
            return; // Exit if no valid target
        }
        Debug.Log("Dashing to enemy: " + targetEnemy.name);
        StartCoroutine(DashToPosition(targetEnemy.position));
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

        yield return new WaitForSeconds(0.3f);
        Destroy(targetEnemy.gameObject);
        mcSwordTransform.rotation = Quaternion.Euler(0, 0, 0);

        FindAnyObjectByType<AudioManager>().Play("Slash");
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

        yield return new WaitForSeconds(5.8f);  // Wait for 5 seconds

        isInvincible = false;
        Debug.Log("OVER");
        shield.SetActive(false);
    }
}
