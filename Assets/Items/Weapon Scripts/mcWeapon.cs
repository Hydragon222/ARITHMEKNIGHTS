using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mcWeapon : MonoBehaviour
{
    Rigidbody2D rb;
    //public Camera cam;

    Vector2 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
   {
       //mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
   }
    //private void FixedUpdate()
    //{

        //Vector2 look = mousePos - rb.position;
        //float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg - 90f;
        //rb.rotation = angle;
    //}
}
