using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DamageTester : MonoBehaviour
{
    //[SerializeField] private InputActionAsset inputActions;
    [SerializeField] private AttributesManager playerAtm;
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
    
}
