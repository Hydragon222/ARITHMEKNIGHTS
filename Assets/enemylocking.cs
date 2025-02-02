using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemylocking : MonoBehaviour
{
    public EnemybehaviorM1 enemyControls;
    public GameObject parentObject; // Assign the parent GameObject in the Inspector
    public Vector3 offset; // Set the desired relative position in the Inspector
    public Vector3 flip;
    
    // Start is called before the first frame update
    void Start()
    {
        
        enemyControls = parentObject.GetComponent<EnemybehaviorM1>();
        transform.parent = parentObject.transform;
        transform.localPosition = offset;
        flip = -offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = offset;
        if (enemyControls.direction1.x < 0)
        {
            offset.x = flip.x;
        }
        else if (enemyControls.direction1.x > 0)
        {
            offset.x = -flip.x;
        }
    }
}
