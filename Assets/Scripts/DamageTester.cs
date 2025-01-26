using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DamageTester : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    public AttributesManager playerAtm;
    public AttributesManager enemyAtm;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerAtm.DealDamage(enemyAtm.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            enemyAtm.DealDamage(playerAtm.gameObject);
        }
   
    }
    //public void OnAttackTap()
    //{
        //playerAtm.DealDamage(enemyAtm.gameObject);
        //Debug.Log("PLSS PLSSS PLSS WORK");
    //}
}
