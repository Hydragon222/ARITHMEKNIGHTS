using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmmount;

    [SerializeField] private GameObject mc;
    public Health playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = mc.GetComponent<Health>();
        healthAmmount = playerHealth.maxHealth;
        healthBar.fillAmount = healthAmmount / 100f;
    }

    // Update is called once per frame
    void Update()
    {
        healthAmmount = playerHealth.health;    
        healthBar.fillAmount = healthAmmount / 100f;
    }
}

