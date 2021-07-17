using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DialogSysComp;

public class MovePlayerPhysics : MonoBehaviour
{

    public float unitsToMove = 8f;
    //public float unitsToMove2 = 14f;
    public float unitsToRotate = 250f;
    public float jumpForce = 10f;
    [HideInInspector]
    public bool stopMoving = false;
    //public GameObject image_holder;
    //public GameObject cameraToLookAt;

    public GameObject avatar;
    public GameObject cameraToAvatarToLookAt;
    public bool fixedPoint = true;
    public Vector3 fixedPointVector = new Vector3(11, 12, -18); // Position of the main camera (screen) from 0,0,0 point
    [HideInInspector]
    public TriggerTalkFront playerTriggerTalkFront = null;
    [HideInInspector]
    public GameObject otherEntityToTalk = null;

    [HideInInspector]
    public DialogSysComp.DSysComponent DSysComp;
    public string DSysCompName = "DialogSystem";

    public Rigidbody rb;

    //[HideInInspector]
    //float RotateSpeed = 60f;

    public bool useJoystick = true;
    public float verticalInp;
    public float horizontalInp;

    //public Quaternion lastMainObjetRotation;

    public float speed = 30.0f;
    private Quaternion qTo;

    private float cameraAngle = -30;

    public Vector3 camAngleVectorOutput;
    public Vector3 directionOutput;


    // Its necessary to put Editors classes on a separated script on a folder called 'Editors'
    //// Shows infos on Inspector..
    //#if UNITY_EDITOR
    //[UnityEditor.CustomEditor(typeof(MovePlayerPhysics))]
    //public class MovePlayerPhysEditor : UnityEditor.Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        DrawDefaultInspector();
    //        //base.OnInspectorGUI();
    //        MovePlayerPhysics myScript = (MovePlayerPhysics)target;

    //        //Showing off the values of the vars on Inspector..
    //        GUILayout.Label("Variable values: ");
    //        if (myScript.playerTriggerTalkFront != null ) 
    //            GUILayout.Label("Player Trigger Talk Front: " + myScript.playerTriggerTalkFront.name);
    //        else
    //            GUILayout.Label("Player Trigger Talk Front: null");

    //        if (myScript.otherEntityToTalk != null)
    //            GUILayout.Label("Player Trigger Talk Front: " + myScript.otherEntityToTalk.name);
    //        else
    //            GUILayout.Label("Other Entity To Talk: null");

    //        GUILayout.Label("Stop Moving: "+ myScript.stopMoving);

    //    }
    //}
    //#endif



    // Start is called before the first frame update
    void Start()
    {
        playerTriggerTalkFront = transform.Find("TriggerTalkFront").GetComponent<TriggerTalkFront>();

        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.PlayOneShot((AudioClip)Resources.Load("intro"));

        rb = GetComponent<Rigidbody>();

        //lastMainObjetRotation = rb.rotation;
        qTo = transform.rotation;

        DSysComp = GameObject.Find(DSysCompName).GetComponent<DSysComponent>();

    }

    // Update is called once per frame
    private void Update()
    {
        //if (Input.GetKey("up"))
        //{
        //    // Move the object forward along its z axis 1 unit/second.
        //    transform.Translate(Vector3.forward * unitsToMove * Time.deltaTime);
        //}else 
        //if (Input.GetKey("down"))
        //{
        //    // Move the object forward along its z axis 1 unit/second.
        //    transform.Translate(Vector3.back * unitsToMove * Time.deltaTime);
        //}

        //If it is on a conversation, dont move
        if (DSysComp.dialogBeenShown)
        {
            stopMoving = true;
        }
        else 
        {
            stopMoving = false;
        }

        //if (!stopMoving) { 
        //    if (Input.GetKey("left"))
        //    {
        //        transform.Rotate(new Vector3(0, -1, 0) * unitsToRotate * Time.deltaTime, Space.Self);
        //    }
        //    else
        //    if (Input.GetKey("right"))
        //    {
        //        transform.Rotate(new Vector3(0, 1, 0) * unitsToRotate * Time.deltaTime, Space.Self);
        //    }
        //}


        /////////////////////////////////////////////
        ////Using W,A,S and D to move..
        ////To the movement match the camera position, the direction of the player
        //// rotation must be relative to the camera, that is turned -30o on Y  
        //float cameraAngle = -30;

        ////float diagonalUnitsToMove = (float)(unitsToMove / (Math.Sqrt(2) / 2)); //Calculating by cosseno

        //if (Input.GetKey("w"))
        //{
        //    if (Input.GetKey("a"))
        //    {
        //        //transform.LookAt(new Vector3(-1, 0, 1));
        //        transform.rotation = Quaternion.Euler(0, -45 + cameraAngle, 0);
        //        //transform.Rotate(new Vector3(-1, 0, 1));
        //        //transform.Translate(new Vector3(-1, 0, 1).normalized * unitsToMove * Time.deltaTime);
        //    }
        //    else if (Input.GetKey("d"))
        //    {
        //        //transform.LookAt(new Vector3(1, 0, 1));
        //        //transform.Rotate(new Vector3(1, 0, 1));
        //        transform.rotation = Quaternion.Euler(0, 45 + cameraAngle, 0);
        //        //transform.Translate(new Vector3(1, 0, 1).normalized * unitsToMove * Time.deltaTime);
        //    }
        //    else
        //    {
        //        //transform.LookAt(Vector3.forward);
        //        //transform.Rotate(Vector3.forward);
        //        transform.rotation = Quaternion.Euler(0, 0 + cameraAngle, 0);
        //                        //transform.Translate(Vector3.forward * unitsToMove * Time.deltaTime);
        //    }
        //    //transform.Translate(Vector3.forward * unitsToMove * Time.deltaTime);
        //}
        //else if (Input.GetKey("s"))
        //{
        //    if (Input.GetKey("a"))
        //    {
        //        transform.rotation = Quaternion.Euler(0, -135 + cameraAngle, 0);
        //    }
        //    else if (Input.GetKey("d"))
        //    {
        //        transform.rotation = Quaternion.Euler(0, 135 + cameraAngle, 0);
        //    }
        //    else
        //    {
        //        transform.rotation = Quaternion.Euler(0, 180 + cameraAngle, 0);
        //    }

        //}
        //else if (Input.GetKey("a"))
        //{
        //    transform.rotation = Quaternion.Euler(0, -90 + cameraAngle, 0);
        //    //transform.Translate(Vector3.left * unitsToMove * Time.deltaTime);
        //}
        //else if (Input.GetKey("d"))
        //{
        //    transform.rotation = Quaternion.Euler(0, 90 + cameraAngle, 0);
        //    //transform.Translate(Vector3.right * unitsToMove * Time.deltaTime);
        //}

        //if(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
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

    private void FixedUpdate()
    {
        if (!stopMoving)
        {
            /////////////////////////////////////////////
            //Using W,A,S and D to move..
            //To the movement match the camera position, the direction of the player
            // rotation must be relative to the camera, that is turned -30o on Y  
            

            //MovePlayer();

            MovePlayerBetter();


            
            verifyAndManageWhenTalkToAnotherEntity();
        }

    }


    void MovePlayerBetter()
    {

        //Get the value of the Horizontal input axis.
        //float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        //Get the value of the Vertical input axis.
        //float verticalInput = Input.GetAxis("Vertical");
        float verticalInput = Input.GetAxisRaw("Vertical");


        if (horizontalInput != 0 || verticalInput != 0)
        {

            //var rotCamAngle = Quaternion.AngleAxis(cameraAngle, Vector3.up);
            // that's a local direction vector that points in forward direction but also 45 upwards.
            //var lDirection = rotCamAngle * Vector3.forward;
            // If you need the direction in world space you need to transform it.
            //var wDirection = transform.TransformDirection(lDirection);

            //Vector3 camAngleVector = quatCamAngle.eulerAngles;
            Vector3 inputDirection = new Vector3(horizontalInput, 0.0f, verticalInput);
            Vector3 direction = Quaternion.Euler(0, cameraAngle, 0) * inputDirection;

            //camAngleVectorOutput = wDirection;
            //directionOutput = direction;

            direction = direction.normalized;

            if (direction != Vector3.zero)
                qTo = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, qTo, Time.deltaTime * speed);

            verticalInp = verticalInput;
            horizontalInp = horizontalInput;


            transform.Translate(direction * unitsToMove * Time.deltaTime, Space.World);

        }
    }

    // Using MovePlayerBetter() instead..
    void MovePlayer()
    {
        

        if (Input.GetKey("w"))
        {
            if (Input.GetKey("a"))
            {
                //transform.rotation = Quaternion.Euler(0, -45 + cameraAngle, 0);
                rb.rotation = Quaternion.Euler(0, -45 + cameraAngle, 0);
            }
            else if (Input.GetKey("d"))
            {
                rb.rotation = Quaternion.Euler(0, 45 + cameraAngle, 0);
            }
            else
            {
                rb.rotation = Quaternion.Euler(0, 0 + cameraAngle, 0);
                
            }
        }
        else if (Input.GetKey("s"))
        {
            if (Input.GetKey("a"))
            {
                rb.rotation = Quaternion.Euler(0, -135 + cameraAngle, 0);
            }
            else if (Input.GetKey("d"))
            {
                rb.rotation = Quaternion.Euler(0, 135 + cameraAngle, 0);
            }
            else
            {
                rb.rotation = Quaternion.Euler(0, 180 + cameraAngle, 0);
            }

        }
        else if (Input.GetKey("a"))
        {
            rb.rotation = Quaternion.Euler(0, -90 + cameraAngle, 0);
            //transform.Translate(Vector3.left * unitsToMove * Time.deltaTime);
        }
        else if (Input.GetKey("d"))
        {
            rb.rotation = Quaternion.Euler(0, 90 + cameraAngle, 0);
            //transform.Translate(Vector3.right * unitsToMove * Time.deltaTime);
        }



        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            //transform.Translate(Vector3.forward * unitsToMove * Time.deltaTime);
            //Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            //GetComponent<Rigidbody>().MovePosition(transform.position + direction * movementSpeed * Time.fixedDeltaTime);
            //rb.MovePosition(transform.position + Vector3.forward * unitsToMove * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + transform.TransformDirection(Vector3.forward * unitsToMove) * Time.fixedDeltaTime);
            //rigidbody.position + transform.TransformDirection(localPositionOffset)
        }




        //////////////////////////////////////////
        //Get controller input to move..
        //////////////////////////////////////////
        ///
        if (useJoystick)
        {

            //Get the value of the Horizontal input axis.
            //float horizontalInput = Input.GetAxis("Horizontal");
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            //Get the value of the Vertical input axis.
            //float verticalInput = Input.GetAxis("Vertical");
            float verticalInput = Input.GetAxisRaw("Vertical");

            //Debug.Log("hInput: " + horizontalInput + " / vInput: " + verticalInput);

            verticalInp = verticalInput;
            horizontalInp = horizontalInput;

            if (verticalInput > 0)
            {
                if (horizontalInput < 0)
                    transform.rotation = Quaternion.Euler(0, -45 + cameraAngle, 0);
                else if (horizontalInput > 0)
                    transform.rotation = Quaternion.Euler(0, 45 + cameraAngle, 0);
                else
                    transform.rotation = Quaternion.Euler(0, 0 + cameraAngle, 0);
            }
            else if (verticalInput < 0)
            {
                if (horizontalInput < 0)
                    transform.rotation = Quaternion.Euler(0, -135 + cameraAngle, 0);
                else if (horizontalInput > 0)
                    transform.rotation = Quaternion.Euler(0, 135 + cameraAngle, 0);
                else
                    transform.rotation = Quaternion.Euler(0, 180 + cameraAngle, 0);
            }
            else if (horizontalInput < 0)
            {
                rb.rotation = Quaternion.Euler(0, -90 + cameraAngle, 0);
                //transform.Translate(Vector3.left * unitsToMove * Time.deltaTime);
            }
            else if (horizontalInput > 0)
            {
                rb.rotation = Quaternion.Euler(0, 90 + cameraAngle, 0);
                //transform.Translate(Vector3.right * unitsToMove * Time.deltaTime);
            }

            if (horizontalInput != 0 || verticalInput != 0)
            {
                transform.Translate(Vector3.forward * unitsToMove * Time.deltaTime);
                //rb.MovePosition(rb.position + transform.TransformDirection(Vector3.forward * unitsToMove) * Time.fixedDeltaTime);
            }


            //float limit = 0.8f;

            //if (verticalInput > limit)
            //{
            //    if (horizontalInput < -limit)
            //        transform.rotation = Quaternion.Euler(0, -45 + cameraAngle, 0);
            //    else if (horizontalInput > limit)
            //        transform.rotation = Quaternion.Euler(0, 45 + cameraAngle, 0);
            //    else
            //        transform.rotation = Quaternion.Euler(0, 0 + cameraAngle, 0);
            //}
            //else if (verticalInput < -limit)
            //{
            //    if (horizontalInput < -limit)
            //        transform.rotation = Quaternion.Euler(0, -135 + cameraAngle, 0);
            //    else if (horizontalInput > limit)
            //        transform.rotation = Quaternion.Euler(0, 135 + cameraAngle, 0);
            //    else
            //        transform.rotation = Quaternion.Euler(0, 180 + cameraAngle, 0);
            //}
            //else if (horizontalInput < -limit)
            //{
            //    rb.rotation = Quaternion.Euler(0, -90 + cameraAngle, 0);
            //    //transform.Translate(Vector3.left * unitsToMove * Time.deltaTime);
            //}
            //else if (horizontalInput > limit)
            //{
            //    rb.rotation = Quaternion.Euler(0, 90 + cameraAngle, 0);
            //    //transform.Translate(Vector3.right * unitsToMove * Time.deltaTime);
            //}


            //if ( (horizontalInput > limit || verticalInput > limit) ||
            //    (horizontalInput < -limit || verticalInput < -limit)
            //    )
            //{
            //    transform.Translate(Vector3.forward * unitsToMove * Time.deltaTime);
            //    //rb.MovePosition(rb.position + transform.TransformDirection(Vector3.forward * unitsToMove) * Time.fixedDeltaTime);
            //}
        }




        //Jumping..
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            // rb.MovePosition(rb.position + transform.TransformDirection(Vector3.up * jumpForce) * Time.fixedDeltaTime);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }


        ////Make the image holder (avatar) of the entity look at the camera
        ////image_holder.transform.LookAt(cameraToLookAt.transform);
        //if (!fixedPoint)
        //{
        //    //Make the image holder (avatar) of the entity look at the camera
        //    avatar.transform.LookAt(cameraToAvatarToLookAt.transform);
        //}
        //else
        //{
        //    avatar.transform.LookAt(fixedPointVector + avatar.transform.position);
        //}

    }


    //When triggered to talk to a entity, control that entity movement..
    public void verifyAndManageWhenTalkToAnotherEntity()
    {
        if(playerTriggerTalkFront != null)
        {
            if(playerTriggerTalkFront.whoCanITalkNowFront.Count > 0)
            {
                Action action = () =>
                {
                    otherEntityToTalk = playerTriggerTalkFront.whoCanITalkNowFront[0].entityGO;
                    //Assuming that other entities uses de MoveNPC script..
                    //Stop the entity movement..
                    otherEntityToTalk.GetComponent<MoveNpc>().isLooking = true;

                    //Stopping everybody else movement, including Player..
                    // Player
                    //stopMoving = true;
                    // Everybody else..
                    playerTriggerTalkFront.DSysComp.EntitiesComponent.ForEach((el) =>
                    {
                       // Everybody else, except the entity that the player is talking to..
                       if( el.GameObj != null && el.GameObj.name != otherEntityToTalk.name )
                       {
                           if(el.GameObj.GetComponent<MoveNpc>() != null )
                           {
                               el.GameObj.GetComponent<MoveNpc>().stopMoving = true;
                           }
                       }
                   });

                };

                //If already has one..
                if (otherEntityToTalk != null)
                {
                    //If they are different..
                    if( otherEntityToTalk.name != playerTriggerTalkFront.whoCanITalkNowFront[0].entityGO.name)
                    {
                        //Free the old entity..
                        otherEntityToTalk.GetComponent<MoveNpc>().isLooking = false;
                        action();
                    }
                }
                else
                {
                    action();
                }
                
            }
            else
            {
                //If already has one..
                if (otherEntityToTalk != null)
                {
                    //Free the old entity..
                    otherEntityToTalk.GetComponent<MoveNpc>().isLooking = false;
                    otherEntityToTalk = null;
                }
                //Make everybody MOVE again, including Player..
                // Player
                //stopMoving = false;
                // Everybody else..
                if (playerTriggerTalkFront.DSysComp != null && playerTriggerTalkFront.DSysComp.ready)
                {
                    if (playerTriggerTalkFront.DSysComp.EntitiesComponent.Count > 0)
                    {
                        //Debug.Log("EntitiesComponent.Count: " + playerTriggerTalkFront.DSysComp.EntitiesComponent.Count);
                        playerTriggerTalkFront.DSysComp.EntitiesComponent.ForEach((el) =>
                        {
                            // Everybody else, except the entity that the player is talking to..
                            //Debug.Log("el gameObject name: " + el.SystemName);
                            if (el.GameObj != null && el.GameObj.GetComponent<MoveNpc>() != null)
                            {
                                el.GameObj.GetComponent<MoveNpc>().stopMoving = false;
                            }
                        });
                    }
                }
            }
        }
        else
        {
            Debug.LogError("TriggerTalkFront not found");
        }
    }
}
