using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject player;
    public GameObject controls;
    private PlayerControls playerControls;
    [SerializeField] private Animator animator;
    public int maxHealth = 100;
    public int health;
    private Transform tr;
    [SerializeField] private GameObject sword;

    private GameOverManager gameOverManager;

    void Start()
    {
        tr = player.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        health = maxHealth;
        playerControls = GetComponent<PlayerControls>();

        // Find the GameOverManager in the scene
        gameOverManager = FindObjectOfType<GameOverManager>();

        if (gameOverManager == null)
        {
            Debug.LogError("GameOverManager not found in the scene!");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Dead();
            Destroy(controls);
        }
    }

    public void Dead()
    {
        animator.SetTrigger("Death");
        AudioManager.instance.StopAll();
        AudioManager.instance.Play("Death");
        sword.SetActive(false);
        playerControls.speed = 0;

        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver(); // Call Game Over Screen
        }
        else
        {
            Debug.LogError("GameOverManager reference is missing!");
        }
    }
}
