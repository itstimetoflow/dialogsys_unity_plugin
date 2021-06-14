using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogSys;
using System.Xml;
using DialogSysComp;

public class TriggerTalkFront : MonoBehaviour
{

    //public string PlayerSystemNameForCollision = "player001";
    public List<WhoCanITalkNowFront> whoCanITalkNowFront;
    [Tooltip("The distance from entity of the trigger ray.")]
    public float rayFactor = 10f;
    [Tooltip("Select the entity parent game object.")]
    public GameObject parent = null;
    [Tooltip("The trigger ray beam width (there are three rays)")]
    public float rayOffset = .25f;
    [HideInInspector]
    public DSysComponent DSysComp;

    Vector3 ray2PosInit;
    Vector3 ray3PosInit;
    Vector3 ray2PosDirection;
    Vector3 ray3PosDirection;

    //private  EntitiyProfile meGameObjEntityProfile;
    //public GameObject DSystemCompHolder;
    //private DSysComponent DSysComp;
    //private DialogSystem DSys;
    //public bool canTalkByHimself = false;

    //public GameObject triggerTalk;

    [System.Serializable]
    public class WhoCanITalkNowFront
    {
        public string nid;
        public string systemName;
        public GameObject entityGO;
    }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //meGameObjEntityProfile = transform.parent.GetComponent<EntitiyProfile>();
        whoCanITalkNowFront = new List<WhoCanITalkNowFront>();

        if(parent == null)
        {
            Debug.LogError("The parent entity wasn't be specified.");
        }
        

        

        //if (canTalkByHimself)
        //{
        //    DSysComp = DSystemCompHolder.GetComponent<DSysComponent>();
        //    DSys = DSysComp.dialogSystem;
        //}
    }


    private void Update()
    {
        //trash..
        if (2 != 3)
        {
            //Debug.Log("Lets try to talk ...");

            ////string nidOkToTalk = "";
            ////Treating the process to talk with entities..
            //if (canTalkByHimself)
            //{

            //    if (DSysComp.ready == true && WhoCanITalkNow.Count > 0)
            //    {
            //        WhoCanITalkNow[0] = "21"; //Solve this later..

            //        Debug.Log("DSysComp.ready == true && WhoCanITalkNow.Count > 0 is OK...");

            //        bool foundAll = true;

            //        for (int i = 0; i < DSys.MainRootNodes.Count; i++)
            //        {
            //            for (int j = 0; j < WhoCanITalkNow.Count; j++)
            //            {
            //                for (int k = 0; k < DSys.MainRootNodes[i].allEntitiesTalking.Length; k++)
            //                {

            //                    if (DSys.MainRootNodes[i].allEntitiesTalking[k] != DSys.playerId)
            //                    {
            //                        foundAll = true;
            //                        int index = DSys.MainRootNodes[i].allEntitiesTalking[k].IndexOf(WhoCanITalkNow[j]);
            //                        if (index < 0)
            //                        {
            //                            foundAll = false;
            //                            break;
            //                        }
            //                        if (foundAll == false) break;
            //                    }
            //                }

            //            }

            //            if (foundAll == true)
            //            {
            //                XmlNode node = DSys.getRealNode(DSys.MainRootNodes[i].nid);
            //                if (verifyIfNodeRootIsOkToEnter(node))
            //                    break;
            //                else
            //                {
            //                    foundAll = false;
            //                }
            //            }


            //        }
            //        if (foundAll == true)
            //        {
            //            string temp = "";
            //            foreach (var item in WhoCanITalkNow)
            //            {
            //                temp += item + ", ";
            //            }
            //            Debug.Log("Player is OK to talk to: " + temp);
            //        }
            //        else
            //        {
            //            Debug.Log("Player cant talk now");
            //        }

            //    }
            //}
        }

        //Getting the DsysComponent..
        if (DSysComp == null)
        {
            //Getting the DsysComponent..
            DSysComp = transform.parent.Find("TriggerTalk").gameObject.GetComponent<TriggerTalk>().DSysComp;
        }

        if (DSysComp != null && DSysComp.ready)
        {
            RaycastDealer();
        }

    }


    void RaycastDealer()
    {
        RaycastHit hit = new RaycastHit();
        RaycastHit hit2 = new RaycastHit();
        RaycastHit hit3 = new RaycastHit();

        //float x = transform.position.x;
        //float y = transform.position.y;
        //float z = transform.position.z;

        Vector3 offSet = transform.right.normalized * rayOffset;

        //ray2PosInit = new Vector3(x + rayOffset, y, z);
        ray2PosInit = transform.position + offSet;
        ray3PosInit = transform.position - offSet;

        // x, y and z Normalized..
        //x = x + (transform.forward.normalized.x * rayFactor);
        //y = y + (transform.forward.normalized.y * rayFactor);
        //z = z + (transform.forward.normalized.z * rayFactor);

        Vector3 rayVector = transform.forward.normalized * rayFactor;

        ray2PosDirection = transform.position + rayVector;
        ray3PosDirection = ray2PosDirection;

        Ray landRay = new Ray(transform.position, transform.position + (transform.forward.normalized * rayFactor));
        Ray landRay2 = new Ray(ray2PosInit, ray2PosDirection);
        Ray landRay3 = new Ray(ray3PosInit, ray3PosDirection);

        //Debug.DrawRay(ray2PosInit, ray2PosEnd, Color.red);
        //Debug.DrawRay(ray3PosInit, ray3PosEnd, Color.red);

        //int layerMask = LayerMask.GetMask("EntityTrigger");
        int layerMask = LayerMask.GetMask( DSysComp.entityTriggerLayerName );

        //Try the hit one of the rays..
        if (!verifyHit(transform.position, transform.TransformDirection(Vector3.forward), hit, rayFactor, layerMask))
        {
            if(!verifyHit(ray2PosInit, transform.TransformDirection(Vector3.forward), hit2, rayFactor, layerMask))
                verifyHit(ray3PosInit, transform.TransformDirection(Vector3.forward), hit3, rayFactor, layerMask);
        }
    }

    public bool verifyHit(Vector3 pos, Vector3 dir, RaycastHit hit, float rayFactor, LayerMask layerMask)
    {
        bool ret = false;
        //if (Physics.Raycast(transform.position, transform.forward, rayFactor, LayerMask.GetMask("EntityTrigger")))
        //if ( Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward.normalized), out hit, rayFactor, LayerMask.GetMask("EntityTrigger")) )
        //if (Physics.Raycast()
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayFactor, layerMask))
        if (Physics.Raycast(pos, dir, out hit, rayFactor, layerMask))
            {
            //Debug.Log("Hit! ");
            ret = true;

            Debug.DrawRay(pos, dir * hit.distance, Color.yellow);

            OnTriggerStayFunc(hit.collider);

        }
        else
        {
            if (whoCanITalkNowFront.Count > 0)
            {
                whoCanITalkNowFront.RemoveAt(0);
                //whoCanITalkNowFront.Clear();
            }
        }
        return ret;
    }


    void OnTriggerStayFunc(Collider other)
    {
        if (other.name == "TriggerTalkFront")
        {
            EntityProfile otherGameObjEntityProfile = other.transform.parent.GetComponent<EntityProfile>();

            //We can talk with just one entitiy at a time..
            //if (otherGameObjEntityProfile.SystemName != "" && WhoCanITalkNowFront.Count == 0)
            if (otherGameObjEntityProfile.SystemName != "")
            {
                //Support one element at a time
                //if (whoCanITalkNowFront.Count == 0)
                //{
                int k = whoCanITalkNowFront.FindIndex(t => t.nid == otherGameObjEntityProfile.Nid);
                if (k < 0)
                {
                    WhoCanITalkNowFront who = new WhoCanITalkNowFront();
                    who.nid = otherGameObjEntityProfile.Nid;
                    who.systemName = otherGameObjEntityProfile.SystemName;
                    who.entityGO = other.transform.parent.gameObject;
                    whoCanITalkNowFront.Add(who);
                    //otherGameObjEntityProfile.ReadyToTalk = true;
                    //meGameObjEntityProfile.ReadyToTalk = true;
                    //otherGameObjEntityProfile.ReadyToTalk = true;
                    //entityProfile.ReadyToTalk = true;
                }
                if (whoCanITalkNowFront.Count > 1)
                    whoCanITalkNowFront.RemoveAt(0);
                //}
                


                //Support one element at a time
                //if (whoCanITalkNowFront.Count == 0)
                //{
                //    int k = whoCanITalkNowFront.FindIndex(t => t.nid == otherGameObjEntityProfile.Nid);
                //    if (k < 0)
                //    {
                //        WhoCanITalkNowFront who = new WhoCanITalkNowFront();
                //        who.nid = otherGameObjEntityProfile.Nid;
                //        who.systemName = otherGameObjEntityProfile.SystemName;
                //        whoCanITalkNowFront.Add(who);
                //        //otherGameObjEntityProfile.ReadyToTalk = true;
                //        //meGameObjEntityProfile.ReadyToTalk = true;
                //        //otherGameObjEntityProfile.ReadyToTalk = true;
                //        //entityProfile.ReadyToTalk = true;
                //    }
                ////    if (whoCanITalkNowFront.Count > 1)
                ////        whoCanITalkNowFront.RemoveAt(1);
                //}
            }
        }
    }


    void OnTriggerExitFunc(Collider other)
    {
        Debug.Log("Collision out.. ");

        if (other.name == "TriggerTalkFront")
        {
            EntityProfile otherGameObjEntityProfile = other.transform.parent.GetComponent<EntityProfile>();
            int k = whoCanITalkNowFront.FindIndex(t => t.nid == otherGameObjEntityProfile.Nid);
            if (k >= 0)
            {
                whoCanITalkNowFront.RemoveAt(k);
            }
            //.FirstOrDefault(t => t == otherGameObjEntityProfile.SystemName);

            //meGameObjEntityProfile.ReadyToTalk = false;
            //entityProfile.ReadyToTalk = false;

        }
    }


    private void OnTriggerStay(Collider other)
    {

        OnTriggerStayFunc(other);
    }


    private void OnTriggerExit(Collider other)
    {
       
        OnTriggerExitFunc(other);
    }


    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if(parent == null)
        {
            parent = transform.parent.gameObject;
        }
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.cyan;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
        //Gizmos.DrawLine(transform.position, Vector3.forward + new Vector3(0, 0, rayLength));
        var x = transform.position.x;
        var y = transform.position.y;
        var z = transform.forward.z;

        Vector3 offSet = transform.right.normalized * rayOffset;

        //Gizmos.DrawLine(transform.position, transform.position + (transform.forward.normalized * rayFactor));
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * rayFactor));
        Gizmos.DrawLine(transform.position + offSet, transform.position + offSet + (transform.forward * rayFactor));
        Gizmos.DrawLine(transform.position - offSet, transform.position - offSet + (transform.forward * rayFactor));
        Gizmos.color = Color.yellow;
        //Gizmos.DrawRay(new Ray(transform.position, transform.forward.normalized * rayFactor));
        //Debug.Log("transform.forward: " + transform.forward * rayFactor);
    }
    #endif

    ////private void OnCollisionStay(Collision collision)
    //private void OnTriggerStay(Collider other)
    ////private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Collision detected $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$ ");
    //}
}
