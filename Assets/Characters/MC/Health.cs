using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private Animator animator;
    public int maxHealth = 100;
    public int health;
    private Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        tr = player.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health == 0)
        {
            Dead();
        }
        else if (health > 0)
        {
            Debug.Log("i live");
        }
    }

    public void Dead()
    {
        Debug.Log("GAME OVER");
        animator.SetTrigger("Death");
    }
}
