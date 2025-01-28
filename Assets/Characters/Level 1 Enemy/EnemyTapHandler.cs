using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyTapHandler : MonoBehaviour
{
    private GameControls controls;
    
    private void Awake()
    {
        contols = new GameControls();
        controls.Player.Tap.performed += ctx => OnTap();
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable() 
    {
        controls.Player.Disable();
    }
    void OnTap()
    {
        Vector2 worldPosition = controls.Player.Tap.ReadValue<Vector2>();

        Vector2 worldPositon = Camera.main.ScreenToWorldPoint(touchPosition);

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            Debug.Log("Tapped");
        }
    }
}
