using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    public GameObject cameraToLookAt;
    public bool fixedPoint = false;
    public  Vector3 fixedPointVector = new Vector3(10,12,-18); // Position of the main camera (screen) from 0,0,0 point
    

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //perfomLookAt(); //Execute on MoveNpc
    }


    public void perfomLookAt()
    {
        if (!fixedPoint)
        {
            //Make the image holder (avatar) of the entity look at the camera
            transform.LookAt(cameraToLookAt.transform);
        }
        else
        {
            transform.LookAt(fixedPointVector + transform.position);
        }
    }
}
