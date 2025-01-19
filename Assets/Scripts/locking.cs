using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class locking : MonoBehaviour
{
    public GameObject parentObject; // Assign the parent GameObject in the Inspector
    public Vector3 offset; // Set the desired relative position in the Inspector

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = parentObject.transform;
        transform.localPosition = offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent = parentObject.transform;
        transform.localPosition = offset;
    }
}
