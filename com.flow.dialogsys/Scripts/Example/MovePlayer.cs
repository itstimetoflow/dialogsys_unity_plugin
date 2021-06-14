using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovePlayer : MonoBehaviour
{

    public float unitsToMove = 8f;
    public float unitsToMove2 = 14f;
    public float unitsToRotate = 250f;
    //public GameObject image_holder;
    //public GameObject cameraToLookAt;

    public GameObject avatar;
    public GameObject cameraToAvatarToLookAt;
    public bool fixedPoint = true;
    public Vector3 fixedPointVector = new Vector3(11, 12, -18); // Position of the main camera (screen) from 0,0,0 point
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey("up"))
        {
            // Move the object forward along its z axis 1 unit/second.
            transform.Translate(Vector3.forward * unitsToMove * Time.deltaTime);
        }else 
        if (Input.GetKey("down"))
        {
            // Move the object forward along its z axis 1 unit/second.
            transform.Translate(Vector3.back * unitsToMove * Time.deltaTime);
        }

        if (Input.GetKey("left"))
        {
            transform.Rotate( new Vector3(0,-1,0) * unitsToRotate * Time.deltaTime, Space.Self);
        }else
        if (Input.GetKey("right"))
        {
            transform.Rotate(new Vector3(0, 1, 0) * unitsToRotate * Time.deltaTime, Space.Self);
        }


        ///////////////////////////////////////////
        //Using W,A,S and D to move..
        //To the movement match the camera position, the direction of the player
        // rotation must be relative to the camera, that is turned -30o on Y  
        float cameraAngle = -30;

        //float diagonalUnitsToMove = (float)(unitsToMove / (Math.Sqrt(2) / 2)); //Calculating by cosseno

        if (Input.GetKey("w"))
        {
            if (Input.GetKey("a"))
            {
                //transform.LookAt(new Vector3(-1, 0, 1));
                transform.rotation = Quaternion.Euler(0, -45 + cameraAngle, 0);
                //transform.Rotate(new Vector3(-1, 0, 1));
                //transform.Translate(new Vector3(-1, 0, 1).normalized * unitsToMove * Time.deltaTime);
            }
            else if (Input.GetKey("d"))
            {
                //transform.LookAt(new Vector3(1, 0, 1));
                //transform.Rotate(new Vector3(1, 0, 1));
                transform.rotation = Quaternion.Euler(0, 45 + cameraAngle, 0);
                //transform.Translate(new Vector3(1, 0, 1).normalized * unitsToMove * Time.deltaTime);
            }
            else
            {
                //transform.LookAt(Vector3.forward);
                //transform.Rotate(Vector3.forward);
                transform.rotation = Quaternion.Euler(0, 0 + cameraAngle, 0);
                                //transform.Translate(Vector3.forward * unitsToMove * Time.deltaTime);
            }
            //transform.Translate(Vector3.forward * unitsToMove * Time.deltaTime);
        }
        else if (Input.GetKey("s"))
        {
            if (Input.GetKey("a"))
            {
                transform.rotation = Quaternion.Euler(0, -135 + cameraAngle, 0);
            }
            else if (Input.GetKey("d"))
            {
                transform.rotation = Quaternion.Euler(0, 135 + cameraAngle, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180 + cameraAngle, 0);
            }
            
        }
        else if (Input.GetKey("a"))
        {
            transform.rotation = Quaternion.Euler(0, -90 + cameraAngle, 0);
            //transform.Translate(Vector3.left * unitsToMove * Time.deltaTime);
        }
        else if (Input.GetKey("d"))
        {
            transform.rotation = Quaternion.Euler(0, 90 + cameraAngle, 0);
            //transform.Translate(Vector3.right * unitsToMove * Time.deltaTime);
        }

        if(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            transform.Translate(Vector3.forward * unitsToMove * Time.deltaTime);
        }



        ////////////////////////////////////////////
        ////Get controller input to move..
        ////////////////////////////////////////////
        /////
        //float horizontalInput = Input.GetAxis("Horizontal");
        ////Get the value of the Horizontal input axis.
        //float verticalInput = Input.GetAxis("Vertical");
        ////Get the value of the Vertical input axis.

        //Debug.Log("hInput: " + horizontalInput + " / vInput: " + verticalInput);

        //if (verticalInput > 0)
        //{
        //    if (horizontalInput < 0)
        //        transform.rotation = Quaternion.Euler(0, -45 + cameraAngle, 0);
        //    else 
        //    if (horizontalInput > 0)
        //        transform.rotation = Quaternion.Euler(0, 45 + cameraAngle, 0);
        //    else
        //        transform.rotation = Quaternion.Euler(0, 0 + cameraAngle, 0);
        //}else
        //if (verticalInput < 0)
        //{
        //    if (horizontalInput < 0)
        //        transform.rotation = Quaternion.Euler(0, -135 + cameraAngle, 0);
        //    else
        //    if (horizontalInput > 0)
        //        transform.rotation = Quaternion.Euler(0, 135 + cameraAngle, 0);
        //    else
        //        transform.rotation = Quaternion.Euler(0, 180 + cameraAngle, 0);
        //}

        //if (horizontalInput != 0 || verticalInput != 0)
        //{
        //    transform.Translate(Vector3.forward * unitsToMove * Time.deltaTime);
        //}



        //Make the image holder (avatar) of the entity look at the camera
        //image_holder.transform.LookAt(cameraToLookAt.transform);
        if (!fixedPoint)
        {
            //Make the image holder (avatar) of the entity look at the camera
            avatar.transform.LookAt(cameraToAvatarToLookAt.transform);
        }
        else
        {
            avatar.transform.LookAt(fixedPointVector + avatar.transform.position);
        }
    }
}
