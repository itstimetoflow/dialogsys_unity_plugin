using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class MoveNpc : MonoBehaviour
{

    public float speed = 12f;
    public float unitsToRotate = 150f;
    public GameObject pointA;
    public GameObject pointB;

    [HideInInspector]
    public bool isLooking = false;
    [HideInInspector]
    public bool stopMoving = false;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public GameObject goingTo;

    public GameObject CubeLookAt;
    public GameObject Player;
    public float smoothRotation = 10f;
    public Quaternion angleToRotateWhenTalk;
    //public LookAtCam lookAtCam;


    // For use by the avatar
    public GameObject avatar;
    [Tooltip("If Fixed Point is not marked, the avatar image will look at this camera")]
    public GameObject cameraToAvatarToLookAt;
    [Tooltip("If marked, the avatar image will look at the Fixed Point Vector")]
    public bool fixedPoint = true;
    [Tooltip("Vector point that the avatar image will look at (Fixed Point must be marked)")]
    public Vector3 fixedPointVector = new Vector3(10, 12, -18); // Position of the main camera (screen) from 0,0,0 point


    // Its necessary to put Editors classes on a separated script on a folder called 'Editors'
    //// Shows infos on Inspector..
    //#if UNITY_EDITOR
    //[CustomEditor(typeof(MoveNpc))]
    //public class MoveNpc_Editor : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        //DrawDefaultInspector();
    //        //MoveNpc myScript = (MoveNpc)target;

            ////Showing off the values of the vars on Inspector..
            //GUILayout.Label("Variable values: ");
            //GUILayout.Label("Is Looking: " + myScript.isLooking);
            //GUILayout.Label("Stop Moving: " + myScript.stopMoving);

            //if (myScript.rb != null)
            //    GUILayout.Label("Rigid Body: " + myScript.rb.name);
            //else
            //    GUILayout.Label("Rigid Body: null");

    //    }
    //}
    //#endif



    public void perfomAvatarLookAt()
    {
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


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        goingTo = pointA;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!stopMoving)
        {

            //Getting the angle to look at..

            CubeLookAt.transform.LookAt(Player.transform);
            angleToRotateWhenTalk = new Quaternion(0, CubeLookAt.transform.rotation.y, 0, CubeLookAt.transform.rotation.w);


            if (!isLooking)
            {

                transform.LookAt(goingTo.transform);
                perfomAvatarLookAt();
                //isLooking = true;

                //transform.Translate(Vector3.forward * unitsToMove * Time.deltaTime);

                //float moveHorizontal = Input.GetAxis("Horizontal");
                //float moveVertical = Input.GetAxis("Vertical");
                //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
                Vector3 movement = new Vector3(1, 0.0f, 0);
                rb.AddForce(transform.forward * speed);
            }
            else
            {
                rb.velocity = new Vector3(0, 0, 0);

                Quaternion myRotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
                //get rotation
                //Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, rcHit.normal);
                Quaternion targetRotation = Quaternion.FromToRotation(transform.rotation.eulerAngles, angleToRotateWhenTalk.eulerAngles);

                //Smooth rotation
                //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothRotation);
                transform.rotation = Quaternion.Lerp(myRotation, angleToRotateWhenTalk, Time.deltaTime * smoothRotation);
                //lookAtCam.perfomLookAt();
                perfomAvatarLookAt();
                CubeLookAt.transform.LookAt(Player.transform);
            }
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    //void OnCollisionEnter(Collision col)
    private void OnTriggerEnter(Collider col)
    {
        if (!stopMoving)
        {
            //Debug.Log("on a collision.. ");
            //Debug.Log("collision obj name: " + col.gameObject.name);

            if (col.gameObject.name == goingTo.name)
            {
                //Destroy(col.gameObject);
                //rb.AddForce(rb.velocity * -1);
                rb.velocity = new Vector3(0, 0, 0);
                if (goingTo.name == pointA.name)
                {
                    goingTo = pointB;
                }
                else
                {
                    goingTo = pointA;
                }

                transform.LookAt(goingTo.transform);
                //lookAtCam.perfomLookAt();
                perfomAvatarLookAt();

            }
            //else if (col.gameObject.name == pointB.name)
            //{
            //    transform.LookAt(pointA.transform);
            //}
        }
    }

    
}