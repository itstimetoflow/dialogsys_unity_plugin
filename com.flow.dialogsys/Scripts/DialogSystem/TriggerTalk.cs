using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
//using UnityEditor;
using DialogSysComp;
using DialogSys;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
//using Unity.Mathematics;


//using System.Reflection;


[System.Serializable]
public class WhoCanITalk
{
    public string nid;
    public string systemName;
}

public class TriggerTalk : MonoBehaviour
{
    [HideInInspector]
    public string message = ""; // For testing..

    //private EntitiyProfile entityProfile;
    //public string PlayerSystemNameForCollision = "player001";
    public List<WhoCanITalk> WhoCanITalkNow;
    //Target front just can talk to one entitiy at a time..

    [Tooltip("Select the TriggerTalkFront game object that this TriggerTalk will talk to (usually sibling to this)")]
    public GameObject triggerTalkFrontGameObj;
    [HideInInspector]
    public TriggerTalkFront triggerTalkFront;
    [HideInInspector]
    public string whoTalkFrontNid = "";
    //public List<string> WhoCanITalkNowFront;

    [HideInInspector]
    public GameObject EntityProfileHolder;
    private EntityProfile entityProfile;
    [Tooltip("Name of the Dialog System Component game object (Ex: 'DialogSystem')")]
    public string DSystemCompHolderName = "DialogSystem";
    [HideInInspector]
    public DSysComponent DSysComp;
    [HideInInspector]
    public DialogSystem DSys;
    //Maybe becouse Tasks, i cant access anothers components gameObjects.. so delcaring buttonTalk again here
    //public GameObject buttonTalk;
    [HideInInspector]
    public ButtonBehavTalk buttonTalk;
    //public ButtonBehaviors buttonTalkBehaviors;
    public bool canTalkByMyself = false;
    [HideInInspector]
    public string dialogNidToTalk;
    //public Text textBox;
    [HideInInspector]
    public bool operating = false;
    [HideInInspector]
    public UnityEvent baseVerificationsEvent;
    private bool isPlayer = false;

    //protected string dialogBoxShowAnimation = "dialogBoxShowAnim";
    //public Animator dialogBoxAnimator;




    // Start is called before the first frame update
    void Start()
    {
        if (EntityProfileHolder == null)
        {
            entityProfile = transform.parent.GetComponent<EntityProfile>();
        }
        else
        {
            entityProfile = EntityProfileHolder.GetComponent<EntityProfile>();
        }
        //WhoCanITalkNow = new List<WhoCanITalk>();

        if (triggerTalkFrontGameObj != null)
        {
            triggerTalkFront = triggerTalkFrontGameObj.GetComponent<TriggerTalkFront>();
        }
        else
        {
            //Triyng to get from the parent..
            triggerTalkFrontGameObj = transform.parent.Find("TriggerTalkFront").gameObject;
            triggerTalkFront = triggerTalkFrontGameObj.GetComponent<TriggerTalkFront>();
        }

        if (canTalkByMyself)
        {
            DSysComp = GameObject.Find(DSystemCompHolderName).GetComponent<DSysComponent>();
            //buttonTalk = DSysComp.ButtonTalkGO;
            //buttonTalkBehaviors = DSysComp.ButtonTalkGO.GetComponent<ButtonBehaviors>();
            DSys = DSysComp.DSys;

            if (buttonTalk == null)
            {
                buttonTalk = DSysComp.ButtonTalkGO.GetComponent<ButtonBehavTalk>();
            }

            if (buttonTalk != null)
            {
                UnityAction action = new UnityAction(() =>
                {
                    DSysComp.dialogOn = true;
                    DSysComp.showDialogBox();
                    DSysComp.audioManager.openDialog.Play();
                    //dialogBoxAnimator.Play(dialogBoxShowAnimation);
                });
                //buttonTalk.AddCallback(action); // Cant add here, will be deleted later..

                //Getting the Button Component..
                Button btn = buttonTalk.GetComponentInParent<Button>();
                btn.onClick.AddListener(action);
            }
        }

        

        baseVerificationsEvent.AddListener(() =>
        {
            BaseVerifications();
        });

        
    }


  

    void Update()
    {
        baseVerificationsEvent.Invoke();

        //Verifing if this component is from the player..
        EntityProfile  parentEntity = transform.parent.GetComponent<EntityProfile>();
        if (parentEntity != null && DSysComp != null && DSysComp.ready)
        {
            if( parentEntity.Nid == DSysComp.DSys.playerId)
            {
                isPlayer = true;
            }
        }
    }



    void BaseVerifications()
    {
        //Debug.Log("Lets try to talk ...");
        //Debug.Log("->primarly im here ------------------------------------------------------------");


        //string nidOkToTalk = "";
        //Treating the process to talk with entities..

        defineVarWhowCanITalkFront();

        message = "XX Entity CAN'T talk now XX";

        // If Operating() is not executing..
        if (!operating)
        {
            if (canTalkByMyself)
            {
                if (DSysComp.ready == true )
                //if (DSysComp.ready == true)
                {
                    if ( !DSysComp.dialogOn )
                    {

                        //Debug.Log("whoTalkFrontNid: " + whoTalkFrontNid + " ******************");
                        //Debug.Log("entityReadyToTalkWithPlayer: " + DSysComp.entityReadyToTalkWithPlayer + " ******************");

                        //if (WhoCanITalkNow.Count > 0 || whoTalkFrontNid != "")
                        if (whoTalkFrontNid != "")
                        {
                            // If anybody was setted to talk..
                            if (DSysComp.entityReadyToTalkWithPlayer == "")
                            {
                                DSysComp.entityReadyToTalkWithPlayer = whoTalkFrontNid;
                                ////Working with Tasks..
                                //OperateUsingTriggerTalkFront();
                            }
                            // If already has setted somebody to talk and is diff from actual whoTalkFrontNid..
                            else if (DSysComp.entityReadyToTalkWithPlayer != whoTalkFrontNid)
                            {
                                //Hide button Talk and get ready to receive another entity..
                                if (DSysComp.buttonTalkOn)
                                {
                                    //buttonTalk.GetComponent<ButtonBehaviors>().hideButton();
                                    buttonTalk.hideElement();
                                    
                                }
                                DSysComp.entityReadyToTalkWithPlayer = "";
                            }

                            //Working with Tasks..
                            if (!DSysComp.buttonTalkOn)
                            {
                                OperateUsingTriggerTalkFront();
                            }

                        }
                        else
                        {
                            if (DSysComp.buttonTalkOn)
                            {
                                //buttonTalk.GetComponent<ButtonBehaviors>().hideButton();
                                buttonTalk.hideElement();
                                
                            }
                            DSysComp.entityReadyToTalkWithPlayer = "";
                        }
                    }
                }


                //if (DSys.finishConversation == true)
                //{
                //    DSys.finishConversation = false;
                //}
            }
        }

        //if ( whoTalkFrontNid == "" && !DSysComp.buttonTalkOn && !DSysComp.dialogOn )
        //{
        //    DSysComp.entityReadyToTalkWithPlayer = "";
        //}
    }

    void defineVarWhowCanITalkFront()
    {
        if(isPlayer)
            //Debug.Log("triggerTalkFront.whoCanITalkNowFront.Count: " + triggerTalkFront.whoCanITalkNowFront.Count);

        if (triggerTalkFrontGameObj != null && triggerTalkFront.whoCanITalkNowFront.Count >= 1)
        {
            whoTalkFrontNid = triggerTalkFront.whoCanITalkNowFront[0].nid;
            if (isPlayer)
            {
                //Debug.Log("triggerTalkFront.whoCanITalkNowFront[0].nid: " + triggerTalkFront.whoCanITalkNowFront[0].nid);
                ////Debug.Log("triggerTalkFront.whoCanITalkNowFront[1].nid: " + triggerTalkFront.whoCanITalkNowFront[1].nid);
            }
        }
        else
            whoTalkFrontNid = "";
    }


    //private void OnTriggerEnter(Collider other)
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Collision detected with: " + other.name);

        if (other.name == "TriggerTalk")
        {
            EntityProfile otherGameObjEntityProfile = other.transform.parent.GetComponent<EntityProfile>();

            if (otherGameObjEntityProfile.SystemName != "")
            {
                // Verify if the object is already on the list..
                string otherNid = otherGameObjEntityProfile.Nid;
                int k = WhoCanITalkNow.FindIndex(t => t.nid == otherNid);
                if (k < 0)
                {
                    WhoCanITalk who = new WhoCanITalk();
                    who.systemName = otherGameObjEntityProfile.SystemName;
                    who.nid = otherGameObjEntityProfile.Nid;
                    WhoCanITalkNow.Add(who);
                    otherGameObjEntityProfile.ReadyToTalk = true;
                    entityProfile.ReadyToTalk = true;
                }
            }
        }

        //baseVerificationsEvent.Invoke();
    }



    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Collision out....... ");

        if (other.name == "TriggerTalk")
        {
            EntityProfile otherGameObjEntityProfile = other.transform.parent.GetComponent<EntityProfile>();
            int i = WhoCanITalkNow.FindIndex(t => t.systemName == otherGameObjEntityProfile.SystemName);
            WhoCanITalkNow.RemoveAt(i);
            //.FirstOrDefault(t => t == otherGameObjEntityProfile.SystemName);

            entityProfile.ReadyToTalk = false;
        }
    }




    //The same as old (deleted) Operate(), junst using TTalkFront in consideration..
    public async Task OperateUsingTriggerTalkFront()
    {
        //Debug.Log("On OperateUsingTriggerTalkFront.. operating will be true..");
        operating = true;

        bool runTaskAwait = false;

        Action action = new Action(() =>
       {
           //Debug.Log("On OperateUsingTriggerTalkFront Task await.. ");

           bool foundOut = false;
           string nidNodeDialogToStart = "";
           DialogSystem.MainRootNode mainRootNode = null;
           bool whoTalkFrontNidFound = false;

           //Debug.Log("im here.. ");
           for (int i = 0; i < DSys.MainRootNodes.Count; i++)
           {

               XmlNode node = DSys.getXmlNode(DSys.MainRootNodes[i].nid);
               if (verifyIfNodeRootIsOkToEnter(node))
               {
                    //foundOUt = true;
                    mainRootNode = DSys.MainRootNodes[i];

                    //If whoCanITalkNowFront in in 

                    // The quantity of entities talking on the root node must be, at least, the same size the entities that the
                    // current entity can talk at the moment
                    //if (mainRootNode.allEntitiesTalking.Length <= (WhoCanITalkNow.Count + 1)) //1 is for the self entity
                    //Debug.Log("mainRootNode.allEntitiesTalking.Length: " + mainRootNode.allEntitiesTalking.Length);

                   // For test..
                   if(transform.parent.name == "Player")
                   {
                       int a = 1;
                   }

                    if ( mainRootNode.allEntitiesTalking.Length <= (WhoCanITalkNow.Count + 2) )
                    //1 is for the self entity and another for whoCanITalkNowFront
                    {

                       bool foundAllEntities = true;
                       for (int k = 0; k < mainRootNode.allEntitiesTalking.Length; k++)
                       {
                           string entityTalkingOnTheNode = mainRootNode.allEntitiesTalking[k];

                           bool foundEntity = false;

                            //List<TriggerTalkFront.WhoCanITalkNowFront> whoTalkFront = triggerTalkFront.whoCanITalkNowFront;
                            //String whoTalkFront = triggerTalkFront.whoCanITalkNowFront[0].nid;

                            //entityProfile is for the current entity (myself)
                            if (entityTalkingOnTheNode == entityProfile.Nid)
                           {
                               foundEntity = true;
                           }
                            //Now searching for the front entity who is talking..
                            else if (whoTalkFrontNid != "")
                           {
                               // For test..
                               if (transform.parent.name == "Player")
                               {
                                   int a = 1;
                               }

                               if (entityTalkingOnTheNode == whoTalkFrontNid)
                               {
                                   //triggerTalkFront.whoCanITalkNowFront[0].nid;
                                   foundEntity = true;
                                   whoTalkFrontNidFound = true;
                               }
                               else
                               {
                                   for (int j = 0; j < WhoCanITalkNow.Count; j++)
                                   {
                                       string whoCanITalk = WhoCanITalkNow[j].nid;
                                       //int index = DSys.MainRootNodes[i].allEntitiesTalking[k].IndexOf(WhoCanITalkNow[j].nid);
                                       //if (index >= 0)
                                       if (entityTalkingOnTheNode == whoCanITalk)
                                       {
                                           //foundOUt = false;
                                           foundEntity = true;
                                           break;
                                       }
                                   }//for C
                               }
                           }
                           

                           if (foundEntity == false)
                           {
                               foundAllEntities = false;
                           }

                       }//for B

                       if (foundAllEntities == true && whoTalkFrontNidFound == true)
                       {
                           foundOut = true;
                           break;
                       }

                   }//if
                }//if node is ok to enter

            }//for A


            if (foundOut == true)
            {
               nidNodeDialogToStart = mainRootNode.nid;

               string temp = "";
               foreach (var item in WhoCanITalkNow)
               {
                   temp += item.nid + ", ";
               }
                //Debug.Log("Player is OK to talk to: " + temp);
                message = "Entity is OK to talk to: " + temp;

               //Button btnTalk = DSysComp.btnTalk;
                //btnTalk.GetComponent<Canvas>().enabled = true;
                //btnTalk.transform.parent.GetComponent<CanvasGroup>().alpha = 1;
                dialogNidToTalk = nidNodeDialogToStart; //Not need anymore?


                //Setting up the button talk to be enable..
                //GameObject buttonTalk = DSysComp.ButtonTalkGO;
                //ButtonBehaviors buttonBehaviors = buttonTalk.GetComponent<ButtonBehaviors>();
                if (!DSysComp.buttonTalkOn && !DSysComp.dialogOn)
               {
                   //Debug.Log("Im HERE!!!!---------------- 1");
                   //buttonTalkBehaviors.showButton();
                   buttonTalk.showElement();
                   DSysComp.audioManager.triggerIn.Play();
                   //Debug.Log("Im HERE!!!!---------------- 2");
                   //buttonTalkBehaviors.nidDialogToStart = nidNodeDialogToStart;
                   buttonTalk.nidDialogToStart = nidNodeDialogToStart;
               }
               //Debug.Log("Im HERE!!!!---------------- 3");

            }
            else
            {
                //DSysComp.btnTalk.gameObject.SetActive(false);
                //DSysComp.btnTalk.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
                message = "XX Entity CAN'T talk now XX";
            }

        });

        if (runTaskAwait) {
            await Task.Run(action);
        }else
        {
            action.Invoke();
        }

        //Debug.Log("OperateUsingTriggerTalkFront() await finished.. operating will be false..");
        operating = false;
    }


    //Verify if node is not set to OK and its conditions are ok to enter on it.
    //Verify its siblings nodes too
    public bool verifyIfNodeRootIsOkToEnter( XmlNode node )
    {
        bool ret = true;
        //XmlNodeList result = xmlDoc.SelectNodes("/data/global_variables/w/raw");
        //XmlNode xmlNode;

        //if (result.Count > 0)
        //{
        //    xmlNode = result[0].FirstChild;
        //}
        //XmlNode node = DSys.getRealNode(nid);
        string nid = node["id"].InnerText;
        bool nodeOk = DSys.nodeOkManager.getOk(nid);
        //Debug.Log("nodeOk: "+ nodeOk);
        if (nodeOk == false)
        {
            if(DSys.verifyConditions(node))
                ret = true;
            else
                ret = false;
        }
        else
        {
            ret = false;
        }

        //Verify its brothers nodes, if they exist..
        if(ret == false)
        {
            //node["node"]
            XmlNodeList result = node.SelectNodes("/node");
            XmlNode nodeBrother;

            if (result.Count > 0)
            {
                //nodeBrother = result[0].FirstChild;
                for (int i = 0; i < result.Count; i++)
                {
                    nodeBrother = result[i].FirstChild;
                    if (verifyIfNodeRootIsOkToEnter(nodeBrother) == true)
                    {
                        ret = true;
                        break;
                    }
                }
            }
        }

        return ret;
    }


    // Shows infos from several vars..
    //#if UNITY_EDITOR
    //[UnityEditor.CustomEditor(typeof(TriggerTalk))]
    //public class TriggerTalkEditor : UnityEditor.Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        DrawDefaultInspector();
    //        //base.OnInspectorGUI();
    //        TriggerTalk myScript = (TriggerTalk)target;

    //        //Showing off the values of the vars on Inspector..
    //        GUILayout.Label("Variable values: ");
    //        GUILayout.Label("Dialog Nid to Talk: " + myScript.dialogNidToTalk);
    //        GUILayout.Label("Is operating? " + myScript.operating);

    //    }
    //}
    //#endif
}
