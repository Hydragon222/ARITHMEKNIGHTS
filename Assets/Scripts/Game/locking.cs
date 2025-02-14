using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class locking : MonoBehaviour
{
    public PlayerControls playerControls;
    public GameObject parentObject; // Assign the parent GameObject in the Inspector
    public Vector3 offset; // Set the desired relative position in the Inspector
    public Vector2 direction;
    public Vector3 flip;
    

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerControls = parentObject.GetComponent<PlayerControls>();
        transform.parent = parentObject.transform;
        transform.localPosition = offset;
        flip = -offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent = parentObject.transform;
        transform.localPosition = offset;
        if (playerControls.currentDirection.x < 0)
        {
            spriteRenderer.flipX = false;
            offset.x = flip.x;

        }
        else if (playerControls.currentDirection.x > 0)
        {
            spriteRenderer.flipX = true;
            offset.x = -flip.x;
        }
    }
}
