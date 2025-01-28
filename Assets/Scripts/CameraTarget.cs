using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraTarget : MonoBehaviour
{
    public float Followspeed = 2f;
    [SerializeField] private Transform target;
    public float yOffset = 1f;
    //[SerializeField] Camera cam;
    //[SerializeField] Transform MC;
    //[SerializeField] float threshold;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        //Vector3 targetPos = (MC.position + mousePos) / 2f;

        //targetPos.x = Mathf.Clamp(targetPos.x, -threshold + MC.position.x, threshold + MC.position.x);
        //targetPos.y = Mathf.Clamp(targetPos.y, -threshold + MC.position.y, threshold + MC.position.y);

        //this.transform.position = targetPos;

        Vector3 newPos = new Vector3(target.position.x,target.position.y + yOffset,-10f);
        transform.position = Vector3.Slerp(transform.position,newPos,Followspeed*Time.deltaTime);
    }
}
