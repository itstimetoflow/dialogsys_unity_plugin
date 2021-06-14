using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{

    public float turnSpeed = 25;
    public bool x = false;
    public bool y = false;
    public bool z = true;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);

        if(x)
            transform.Rotate(Vector3.right, turnSpeed * Time.deltaTime);
        else if(y)
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        else 
            transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime);
    }
}
