﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;
using TMPro;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private GameObject curvedSlashPrefab;
    [SerializeField] private InputActionReference moveAction;  // Existing move action reference
    [SerializeField] private InputActionReference tapAction;   // Tap action reference
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject popAns;
    [SerializeField] private TMP_Text popAnsText;
    public float answer;
    public PopupQuestion popupQuestion;
    public GameObject victorypanel;

    public Stun stun;
    [SerializeField] private Win win;
    [SerializeField] private EnemyCounter enemyCounter;
    [SerializeField] private mobileUI mobileui;

    private Transform mcSwordTransform;
    private SpriteRenderer spriteRenderer;
    
    private RaycastHit2D hit;
    private GameObject lastTappedEnemy;
    public Transform targetEnemy;

    public bool hasTappedEnemy = false;
    public bool isInvincible = false;
    private bool isDashing = false;
    public bool hasTappedDialogue = false;

    public float speed;
    public float dashSpeed = 30f; // Dash speed
    public float dashDuration = 0.2f; // Dash duration
    public float XY;
    public float YX;
    public Vector2 currentDirection;
    private float tapCooldown = 0.3f;  // Time between taps
    private float lastTapTime = 0f;

    public float kills;
    public float killsReqd;

    private void Start()
    {
        kills = 0;
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

        victorypanel.SetActive(false);
    }

    private void Update()
    {
        Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();

        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
        }

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
            FindAnyObjectByType<AudioManager>().Stop("Walking"); // Stop when the player stops moving
        }
        else
        {
            if (!FindAnyObjectByType<AudioManager>().IsPlaying("Walking")) // Play only if not already playing
            {
                FindAnyObjectByType<AudioManager>().Play("Walking");
            }
        }
        OnTap();

        if (mobileui.IsRunningOnPC()) 
        {
            RotateSword();
        }
    }
    private void RotateSword()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint
            (Input.mousePosition) - mcSwordTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        mcSwordTransform.rotation = rotation;
    }

    private void OnTap()
    {
        Vector2 worldPosition = Vector2.zero;
        bool isTap = false;

        // Handle Touch Input (Mobile)
        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            foreach (TouchControl touch in Touchscreen.current.touches)
            {
                if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    worldPosition = Camera.main.ScreenToWorldPoint(touch.position.ReadValue());
                    isTap = true;
                    break;
                }
            }
        }

        // Handle Mouse Clicks (PC)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            isTap = true;
        }

        if (!isTap) return;

        // Prevent clicking UI elements
        if (EventSystem.current != null)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = worldPosition };
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.CompareTag("JoystickUI"))
                {
                    Debug.Log("Tap detected on Joystick UI, ignoring.");
                    return;
                }
            }
        }

        // **Check for a tap on an object**
        Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition);

        if (hitCollider != null)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemybehaviorM1 enemyBehavior = hitCollider.GetComponent<EnemybehaviorM1>();
                
                if (enemyBehavior != null && enemyBehavior.isDying)
                {
                    Debug.Log("Tapped a dying enemy, ignoring.");
                    return; // Ignore if the enemy is dying
                }

                if (hasTappedEnemy)
                {
                    Debug.Log("Enemy already tapped, ignoring further taps.");
                    return;
                }
                if (hasTappedDialogue) return;

                targetEnemy = hitCollider.transform;
                Debug.Log("Enemy stored: " + targetEnemy.name);

                stun.StunAllEnemies();
                ActivateInvincibility();

                hasTappedEnemy = true;
                AudioManager.instance.Play("Question");
                popupQuestion.ShowQuestionUI();
                
            }
            else if (hitCollider.CompareTag("Wizzard")) // If tapped on the wizard
            {
                Debug.Log("Wizzard clicked! Triggering dialogue.");

                // ✅ Prevent multiple triggers using cooldown
                if (Time.time - lastTapTime < tapCooldown) return;
                lastTapTime = Time.time;
                if (hasTappedEnemy) return;

                hasTappedDialogue = true;

                // Find the DialogueTrigger component on the wizard and trigger dialogue
                DialogueTrigger wizzardDialogue = hitCollider.GetComponent<DialogueTrigger>();
                if (wizzardDialogue != null)
                {
                    wizzardDialogue.TriggerDialogue();
                }
                
                // ✅ Reset flag after a short delay
                StartCoroutine(ResetDialogueTap());
            }
        }
    }

    public void Correct()
    {
        if (targetEnemy == null)
        {
            Debug.LogError("Error: No enemy stored! Cannot dash.");
            return; // Exit if no valid target
        }
        Debug.Log("Dashing to enemy: " + targetEnemy.name);
        StartCoroutine(DashToPosition(targetEnemy.position));
        EnemybehaviorM1 enemyBehavior = targetEnemy.GetComponent<EnemybehaviorM1>();
        if (enemyBehavior != null)
        {
            // Trigger GetRekt() only for the tapped enemy
            enemyBehavior.GetRekt();
            kills += 1;
            enemyCounter.Score();
            if (kills == killsReqd)
            {
                StartCoroutine(Victory());
            }
        }
    }
    public void Incorrect()
    {
        StartCoroutine(Wrong());
    }
    private IEnumerator Wrong()
    {
        popAns.SetActive(true);
        popAnsText.text = $"The answer was {answer}!";
        
        yield return new WaitForSeconds(1.5f);
        popAns.SetActive(false);
    }

    private IEnumerator Victory()
    {
        yield return new WaitForSeconds(0.5f);
        victorypanel.SetActive(true);
        win.Winned();
    }

    private IEnumerator DashToPosition(Vector3 targetPosition) 
    {
        isDashing = true;

        GameObject slashEffect = Instantiate(slashPrefab, transform.position, Quaternion.identity);
        TrailRenderer trail = slashEffect.GetComponent<TrailRenderer>();
        slashEffect.transform.SetParent(transform);
        
        AudioManager.instance.Play("Dash");
        AudioManager.instance.Play("Slash");

        float dashTime = 0f;
        Vector3 startPosition = transform.position;

        Vector3 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        while (dashTime < dashDuration)
        {
            GameObject curvedSlash = Instantiate(curvedSlashPrefab, mcSwordTransform.position, Quaternion.Euler(0, 0, angle - 125));
            curvedSlash.transform.SetParent(transform);
            Destroy(curvedSlash, 0.4f);

            dashTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, dashTime / dashDuration);

            mcSwordTransform.rotation = Quaternion.Euler(0, 0, angle);
            yield return null;
        }

        transform.position = targetPosition;
        isDashing = false;
        Destroy(slashEffect, trail.time);
        slashEffect.transform.SetParent(null);

        yield return new WaitForSeconds(0.3f);
        
        mcSwordTransform.rotation = Quaternion.Euler(0, 0, 0);

        FindAnyObjectByType<AudioManager>().Play("Slash");

        hasTappedEnemy = false;
        targetEnemy = null;
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

    private IEnumerator ResetDialogueTap()
    {
        yield return new WaitForSeconds(tapCooldown);
    }
}
