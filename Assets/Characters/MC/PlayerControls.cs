using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private InputActionReference moveActionToUse;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = moveActionToUse.action.ReadValue<Vector2>();
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
