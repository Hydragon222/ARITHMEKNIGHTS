using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject player;
    public GameObject controls;
    [SerializeField] private Animator animator;
    public int maxHealth = 100;
    public int health;
    private Transform tr;
    [SerializeField] private GameObject sword;
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
            Destroy(controls);
        }
    }

    public void Dead()
    {
        animator.SetTrigger("Death");
        AudioManager.instance.StopAll();
        AudioManager.instance.Play("Death");
        sword.SetActive(false);
        

    }
}
