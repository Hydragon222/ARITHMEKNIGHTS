using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmmount;

    public GameObject mc;
    public AttributesManager attributes;
    public DamageTester damageTester;
    // Start is called before the first frame update
    void Start()
    {
        attributes = mc.GetComponent<AttributesManager>();
        healthAmmount = attributes.health;
        GetComponent<DamageTester>();
    }

    // Update is called once per frame
    void Update()
    {
        healthAmmount = attributes.health;
        healthBar.fillAmount = healthAmmount / 100f;
    }
}

   //public void OnDealDamage(int damage)
    //{
        //healthAmmount -= damage;
        //healthBar.fillAmount = healthAmmount / 100f;
    //}

