using System;
//using System.Collections;
using System.Collections.Generic;
using System.Linq;

//Needed for XML functionality
using System.Xml;
//using System.Xml.Serialization;
//using System.Xml.Xsl;
//using System.Xml.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.IO;
//using UnityEditor;

//Used to parse json
// using SimpleJSON;

using System.Dynamic;
//using System.Net;
//using System.Reflection;

//Used for Json - Xml convertions..
using Newtonsoft.Json;

//using Jint;

//using ExtensionMethods; //Defined below

//using DialogSysComp;
using DialogSys;

//using NaughtyAttributes;

//using System.Threading;
using System.Threading.Tasks;
//using DialogClasses;
using MainGD;
using TMPro;
//using System.Threading;
//using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif


namespace DialogSysComp
{

    public class DSysComponent : MonoBehaviour
    {
        // The class that holds the main data of the game 
        [HideInInspector]
        public MainGD.MainGameData mainGameData;
        public string mainGameDataName = "MainGameData";
        //MainGlobaVariablesClass.Root
        //public  MainGameData.MainGD.MainGlobaVariablesClass.Root MainGlobalVariables;
        //public List<MainGameData.MainGD.MainDialogSysDatasetClass> MainDatasets;

        // For test..
        public Int32 counterTest = 0;


        [HideInInspector]
        public bool AutoLoadRootNodes = false;

        //public GameObject DialogBoxGameObj;
        public DialogSystem DSys { get; set; }
        public string pathToXmlFile = "..\\..\\..\\saved_files\\export\\save_data_get_sword_improved.xml"; //Assets\\XmlData\\user_data_04.xml
        string jsonString;
        public string textForTextBox = "save_data_get_sword_improved.xml";
        //[Tooltip("Subfolder name for the application resources. Default: 'DialogSystem'")]
        //public string appResourcesSubfolder = "DialogSystem";
        [Tooltip("Subfolder name for the game avatars images. Ex: 'Avatars', without initial slash '\\'")]
        public string avatarsResourcesSubfolder = "Avatars\\Images";
        //public Button BtnNext;
        //public List<Button> BtnsOptions = new List<Button>();

        [HideInInspector]
        public string playerId;// = "20";
        [Tooltip("Enter the entities holder game object name created by you ('Entitites'?)")]
        public string rootEntitiesGOName = "Entities";
        [Tooltip("Its necessary to create a layer to put all dialog system trigger game objects in the entities (Ex: 'EntityTrigger').")]
        public string entityTriggerLayerName = "EntityTrigger";
        [Tooltip("Assign the EventSystem game object")]
        public EventSystem eventSystem;
        // Message Box is the box that is showed between node lines dialogs 
        // that show some info when some condition is satisfied 
        [HideInInspector]
        public GameObject MessageBoxGO = null;
        [Tooltip("Assign the game object that holds de Message Box Component: the game object Content (son of the MessageBox game object)")]
        public MessageBox messageBox;
        [Tooltip("Assign the AudioObjectsPrefab placed in the scene")]
        public AudioManager audioManager = null;

        public GameObject btnBase;
        [HideInInspector]
        public List<GameObject> btnsOptionsPositions;
        public GameObject btnPositionsGO;
        [HideInInspector]
        public GameObject btnOkPosition;
        //public GameObject btnOk;
        //public Button btnTalk;
        [HideInInspector]
        public bool btnTalkReady = false;
        [HideInInspector]
        public string NextNodeIdSet;


        [HideInInspector]
        public TriggerTalk triggerTalkFromPlayer;

        [HideInInspector]
        public UnityEvent updateDialogBoxEvent;
        [HideInInspector]
        public UnityEvent showMessageBoxEvent;

        // Indicates if the component is ready to be accessed
        [HideInInspector]
        public bool ready = false;

        //public List<GameObject> EntitiesGameObjects;
        //public DialogSysEntitiesComponent Ent;
        //public List<string> testListString;

        //Fields for the new aproach
        public GameObject DialogSysGO;
        public GameObject DialogBoxGO;
        public GameObject ButtonTalkGO;
        public string btnTalkLabel = "Talk";
        public TMP_Text entityNameTMP;
        //public GameObject DialogBoxGOInstantiated;
        public Text MainTextBox;
        public TMP_Text MainTextBoxTMP;
        //private bool buttonTalkReady = false;

        public GameObject ButtonStartTest;

        //public bool testInitButtonTalk = false;

        //[Header("Variable Statuses: ")]
        [HideInInspector]
        public bool dialogOn = false;
        [HideInInspector]
        public bool dialogBeenShown = false;
        [HideInInspector]
        public bool buttonTalkOn = false;
        //public bool messageBoxOn = false;

        [HideInInspector]
        public String entityReadyToTalkWithPlayer;


        private String dialogBoxShowAnimation = "dialogBoxShowAnim";
        private String dialogBoxHideAnimation = "dialogBoxHideAnim";

        [HideInInspector]
        public bool btnLabelChanged = true;

        [Tooltip("Chat text will be animated when goes appearing")]
        public bool animateDialogueText = true;
        //[Tooltip("In a multithreaded enviroment, allow the dialog to happen at the same time that another operations in the game")]
        public bool runAsynchronously
        {
            get { return _runAsynchronously; }
            set
            {
                _runAsynchronously = value;
                if (ready == true)
                {
                    // DO SOMETHING HERE
                    DSys.DSysCompLocal.runAsynchronously = _runAsynchronously;
                }
            }
        }
        private bool _runAsynchronously = false;


        //[Tooltip("Shows development information (for test purposes)")]
        public bool showTestingInfo
        {
            get { return _showTestingInfo; }
            set
            {
                _showTestingInfo = value;
                if (ready == true)
                {
                    // DO SOMETHING HERE
                    DSys.DSysCompLocal.showTestingInfo = _showTestingInfo;
                }
            }
        }
        private bool _showTestingInfo = false;

        public GameObject testingInfoBox;

        public AnimatingDialogueText AnimatingDialogueTextComp;

        //Put it here to be the last element on the Inspector, before the button
        //to create the entities..
        [Tooltip("All entities created on the dialog system, will be shown here")]
        public List<DialogSysEntitiesComponent> EntitiesComponent;

        public Image imageAvatar;

        // A list of callbacks to execute after the execution of every chat line print
        List<callbackToExecuteAfterEveryChatLine> callbacksToExecAfterEveryChatLine = new List<callbackToExecuteAfterEveryChatLine>();

        [HideInInspector]
        public List<ImageAvatarLoaded> ImagesAvatarsLoaded = new List<ImageAvatarLoaded>();

        //Stores all Avatars sprites that will be loaded 
        public class ImageAvatarLoaded
        {
            public string fileName;
            public Sprite sprite;

            public ImageAvatarLoaded( string fName, Sprite sprt)
            {
                fileName = fName;
                sprite = sprt;
            }
        }



        //public AnimatedDialogueText AnimatedDialogueText;

        //void OnAnimButtonIsIdle_DialSys()
        //{
        //    buttonTalkReady = true;
        //    Debug.Log("DialogSys: Button ready..");
        //}

        //public void OnClickButton_DialSys()
        //{
        //    if (buttonTalkReady)
        //    {
        //        Debug.Log("DialogSys: Button clicked..");
        //    }
        //}

        //void OnGUI()
        //{
        //    EditorGUI.Foldout(new Rect(3, 3, 30, 15), true, "Testando Foldout..");
        //}


        public void OpenDialogBox(string nid) {

            dialogOn = true;

            Debug.Log("DialogSys: Button hided. Open Dialog Box..");
            //ShowDialogBox();

            //GameObject dialogBox = Instantiate<GameObject>(DialogBoxGO);
            //DialogBoxGOInstantiated = dialogBox;
            //GameObject dialogBoxPos = DialogSysGO.transform.Find("Canvas").Find("DialogBoxPos").gameObject;
            //dialogBox.transform.position = dialogBoxPos.transform.position;
            //dialogBox.transform.parent = dialogBoxPos.transform;
            GameObject dialogBox = DialogBoxGO;

            //Defining positions
            btnOkPosition = dialogBox.transform.Find("Content").Find("btnOkPositionGO").gameObject;
            GameObject btnPositionsGO = dialogBox.transform.Find("Content").Find("btnPositionsGO").gameObject;
            btnsOptionsPositions[0] = btnPositionsGO.transform.Find("pos01").gameObject;
            btnsOptionsPositions[1] = btnPositionsGO.transform.Find("pos02").gameObject;
            btnsOptionsPositions[2] = btnPositionsGO.transform.Find("pos03").gameObject;
            btnsOptionsPositions[3] = btnPositionsGO.transform.Find("pos04").gameObject;
            btnsOptionsPositions[4] = btnPositionsGO.transform.Find("pos05").gameObject;
            MainTextBox = dialogBox.transform.Find("Content").Find("MainText").GetComponent<Text>();
            MainTextBoxTMP = dialogBox.transform.Find("Content").Find("MainTextTMP").GetComponent<TMP_Text>();

            Debug.Log("Node Id to init open: "+ nid);
            DSys.showOff.ShowingOffNode(nid);
        }



        public void showDialogBox()
        {
            DialogBoxGO.transform.Find("Content").GetComponent<Animator>().Play(dialogBoxShowAnimation);
            dialogBeenShown = true;
        }

        public void hideDialogBox()
        {
            DialogBoxGO.transform.Find("Content").GetComponent<Animator>().Play(dialogBoxHideAnimation);
            MainTextBox.text = "";
            MainTextBoxTMP.text = "";
            dialogBeenShown = false;
            entityNameTMP.text = "";
            entityNameTMP.ForceMeshUpdate();
        }


        void Start()
        {

            //if (DialogBoxGameObj != null)
            //{

            //Debug.Log("====> DSysComp Start(): Task #" + Task.CurrentId.ToString());
            //ShowThreadInformation("DSysComp Start()");

            Debug.Log("Starting..");

            //Try to rename the DSysComponent, if its not already renamed..
            //if(this.name == "DialogSystem")
            //{
            //    this.name = "DialogSystem";
            //}

            //testCreatureLoad();
            //testDataLoad();
            // init();
            //disableNextBtn();

            //Getting the MainGameData by the name
            mainGameData = GameObject.Find(mainGameDataName).GetComponent<MainGameData>();


            DSys = new DialogSystem();
            //DialogSystem dialogSystem = new DialogSystem("C:\\www\\dialogsystem\\DialogSystem\\CSharp\\testing\\xml\\Testing XML\\user_data_03.xml");

            //UnityEvent showingOffNodeEvent = new UnityEvent();
            //showingOffNodeEvent.AddListener(()=> { dialogSystem.ShowingOffNode(nid); } );

            //DSys.DSysComp = this;
            DSys.playerId = playerId;
            //mainGameData.DialogSystem = DSys;
            //pathToXmlFile = "C:\\www\\dialogsystem\\DialogSystem\\CSharp\\testing\\xml\\Testing XML\\user_data.xml";
            //pathToXmlFile = "C:\\www\\dialogsystem\\DialogSystem\\CSharp\\testing\\xml\\Testing XML\\user_data_two-talking.xml";

            //AnimatingDialogueTextComp = GetComponent<AnimatingDialogueTextComp>();

            if (entityNameTMP != null)
            {
                DSys.hasEntityNameAndImgGameObjects = true;
            }


            // Defining the personalized code adjustment callback..
            Func<string, string> codeUserDefinedCallback = new Func<string, string>(
                (string code) =>
                {
                    //Do whatever you like..
                    //Test..
                    //Debug.Log("Inside codeCallback @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    return code;
                });

            //Action doItBeforePrintEveryChatLineCallback = new Action(() =>
            //{
            //    //Debug.Log("Inside doThingsOnNodeLineCallback #$#$#$#$#$#$#$#$#$#$#$#$#$#$#$#$#$#$#$#$#$#$#$");
            //});

            DSys.callbackAdjustCode = codeUserDefinedCallback;
            //DSys.callbackToExecuteBeforePrintEveryChatLine = doItBeforePrintEveryChatLineCallback;

            if (testingInfoBox != null)
            {
                if (showTestingInfo)
                    testingInfoBox.SetActive(true);
                else
                    testingInfoBox.SetActive(false);
            }

            //If has a entity text gameobject..
            if (entityNameTMP != null)
            {
                entityNameTMP.text = "";
                entityNameTMP.ForceMeshUpdate();
            }


            DSys.DSysCompLocal = new DialogSystem.DsysCompLocal();

            DSys.LocalGlobalVarsToMainGlobalVars = LocalGlobalVarsToMainGlobalVars;
            DSys.MainGlobalVarsToLocalGlobalVars = MainGlobalVarsToLocalGlobalVars;
            DSys.LocalDatasetsToMainDatasets = LocalDatasetsToMainDatasets;
            DSys.MainDatasetsToLocalDatasets = MainDatasetsToLocalDatasets;
            


            ready = DSys.Init(pathToXmlFile);

            //Will update the DialogSystem DsysCompLocal variable 
            Update_Dsys_DsysCompLocal();

            //Getting some gameobjects..
            if (DialogBoxGO == null) {

                DialogBoxGO = transform.Find("DialogBox").gameObject;
            }
            else if( DialogBoxGO != null)
            {
                if (btnPositionsGO == null)
                {
                    btnPositionsGO = DialogBoxGO.transform.Find("Content").Find("btnPositionsGO").gameObject;
                }
            }

            MessageBoxGO = messageBox.transform.parent.gameObject;


            //Getting TriggerTalk from Player..
            GetTriggerTalkFromPlayer();

            //DialogSys.Program component = new DialogSys.Program();
            //component.Main();
            //}

            updateDialogBoxEvent.AddListener(() =>
            {
                UpdateDialogBox();
            });


            //Testing adding a button..
            //GameObject canvas = DialogSysGO.transform.Find("Canvas").gameObject;
            //GameObject btnTemp = Instantiate<GameObject>(btnBase, canvas.transform.position, canvas.transform.rotation, canvas.transform);

            ButtonStartTest.GetComponent<Button>().onClick.AddListener(() =>
            {
                ButtonTalkGO.GetComponent<Animator>().Play(ButtonTalkGO.GetComponent<ButtonBehaviors>().showAnimation);
            });

            // Add verification routines before a chat line is executed
            //testVerificationBeforeChatLines();

            // Setting routines to execute after every chat line has been printed
            settingCallbacksAfterEveryChatLinePrinted();

            // Setting routines to verify conditions to show messages on MessageBox
            settingVerificationsToShowMessagesAfterChatLines();

            // Initially, not showing any chars on the TextMeshPro 
            // text component, if it will be animated
            //if (animateDialogueText)
            //{
            //    //MainTextBoxTMP.maxVisibleCharacters = 0;
            //    MainTextBoxTMP.text = "";
            //    MainTextBoxTMP.ForceMeshUpdate();
            //}

        }


        //Update the DialogSystem DsysCompLocal variable 
        void Update_Dsys_DsysCompLocal()
        {
            //DSys.DSysCompLocal.AutoLoadRootNodes = AutoLoadRootNodes;
            //DSys.DSysCompLocal.DialogBoxGO = DialogBoxGO;

            //if(DSys.DSysCompLocal.mainGameData != null)
            //    DSys.DSysCompLocal.mainGameData = mainGameData;
            //if (DSys.DSysCompLocal.pathToXmlFile != null)
            //    DSys.DSysCompLocal.pathToXmlFile = pathToXmlFile;
            playerId = DSys.playerId;
            //ready = DSys.DSysCompLocal.ready;
            //using get and set to trigger when variable changes value 
            //DSys.DSysCompLocal.runAsynchronously = runAsynchronously;
            //DSys.DSysCompLocal.showTestingInfo = showTestingInfo;
            
        }


        //Get TriggerTalk from Player
        void GetTriggerTalkFromPlayer()
        {
            int k = EntitiesComponent.FindIndex(t => t.Nid == DSys.playerId);
            if (k >= 0)
            {
                Debug.Log("Entity sys name: " + EntitiesComponent[k].SystemName);
                triggerTalkFromPlayer = EntitiesComponent[k].GameObj.transform.Find("TriggerTalk").GetComponent<TriggerTalk>();
            }
        }



        void Update()
        {
            //ShowThreadInformation("DSysComp Update()");

            if (ready) {

                if (!dialogOn)
                {
                    //DSys.DSysComp.imageAvatar.sprite = null;
                    imageAvatar.sprite = null;
                }
                ////UpdateDialogBox();
                //updateDialogBoxEvent.Invoke();

                //showMessageBoxEvent.Invoke();

                //If theres some message to show on MessageBox, show it before show the DialogBox content..
                if (DSys.messagesToPrint.Count > 0 && messageBox.messageBoxOn == false)
                {
                    //Print Messages..
                    MessageToPrint messageToPrint = DSys.messagesToPrint[0];
                    //messageBox.SetActive(true);

                    //MessageBoxGO.transform.Find("Canvas").Find("Content").Find("MainText").GetComponent<Text>().text = messageToPrint.msg;
                    MessageBoxGO.transform.Find("Content").Find("MainText").GetComponent<Text>().text = messageToPrint.msg;
                    //MessageBoxGO.transform.Find("Content").GetComponent<CanvasGroup>().alpha = 1;
                    hideDialogBox();
                    messageBox.showMessageBox();
                    //Button btn = MessageBoxGO.transform.Find("Canvas").Find("Content").Find("Button")
                    Button btn = MessageBoxGO.transform.Find("Content").Find("Button")
                            .GetComponent<Button>();
                    //ButtonBehavMessageBox btnMsgBox = MessageBoxGO.transform.Find("Canvas").Find("Content").Find("Button").GetComponent<ButtonBehavMessageBox>();

                    btn.onClick.RemoveAllListeners();
                    btn.Select();

                    if (messageToPrint.callback != null)
                    {

                        btn.onClick.AddListener(
                            //btnMsgBox.AddCallback(
                            () =>
                            {
                                messageToPrint.callback();
                            });
                    }

                    // When click button to 'ok' the message box, unhide DialogBox, if has not
                    // any other message to show..
                    btn.onClick.AddListener(
                    //btnMsgBox.AddCallback (
                        () =>
                        {
                            Debug.Log("test callback from btnMsgBox..");
                            messageBox.hideMessageBox();
                            ////if (DSys.messagesToPrint.Count <= 0 && !DSys.finishConversation)
                            //if (DSys.messagesToPrint.Count <= 0 && !DSys.finishConversation && DSys.showOff.elementsToPrint.textToPrint != "")
                            //{
                            //    showDialogBox();
                            //}
                        });

                    //Remove message after print it
                    DSys.messagesToPrint.RemoveAt(0);
                }
                else // If theres no message to print..
                {
                    //UpdateDialogBox();
                    //Debug.Log("Will update DialogBox..");
                    //updateDialogBoxEvent.Invoke();

                    // Some tasks of DialogSystem must run on a separate thread (on a multithreading system), to 
                    // gain performance. Unity doesnt allow access game objects from the game main thread, so DialogSystem class
                    // can just exec the operations and store on variables. Here, we will verify if this vars are set and 
                    // update the game opjects..
                    //var showOff = DSys.showOff;
                    //var elToPrint = showOff.elementsToPrint;
                    DialogSystem.ShowOffNode showOff = DSys.showOff;
                    DialogSystem.ShowOffNode.ElementToPrint elToPrint = DSys.showOff.elementToPrint;

                    //Debug.Log("elToPrint: " + elToPrint);
                    //Debug.Log("elToPrint.buttonsToPrint: " + elToPrint.buttonsToPrint);

                    //Verify if is the end of conversation to close de dialogbox

                    //Debug.Log("finishConversation: " + DSys.finishConversation);
                    if (DSys.finishConversation)
                    {
                        Debug.Log("DSys.finishConversation: " + DSys.finishConversation + "  ºººººººººººººººººººººººººººº");
                    }

                    // If the conversation is finished and theres no more messages to print, 
                    // hide the DialogBox within seting de dialogOn to false, indicating 
                    // the definitive end of the all conversation..
                    if (DSys.finishConversation == true && !messageBox.messageBoxOn) //DSys.messagesToPrint.Count <= 0 &
                    //if (  DSys.lastClickConversation )
                    {
                        //Debug.Log( "DSys.lastClickConversation == true ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" );
                        //DialogBoxGO.transform.Find("Content").GetComponent<Animator>().Play("dialogBoxHideAnim");
                        //DSys.lastClickConversation = false;
                        hideDialogBox();
                        DSys.finishConversation = false;
                        dialogOn = false;
                    }

                    //if (dialogOn && !dialogBeenShown && !messageBox.messageBoxOn && DSys.messagesToPrint.Count > 0)
                    if (dialogOn && !dialogBeenShown && !messageBox.messageBoxOn)
                    {
                        showDialogBox();
                    }

                    // If has something to print and the trigger talk from player is not 
                    // operating (executing some animation and stuff..), execute .. 

                    //Debug.Log("++++++++++++++++++----> elToPrint.textToPrint:" + elToPrint.textToPrint);
                    //Debug.Log("MainTextBoxTMP.maxVisibleCharacters: " + MainTextBoxTMP.maxVisibleCharacters);

                    //if (elToPrint != null && elToPrint.textToPrint != "")
                    if (elToPrint != null)
                    {
                        if(elToPrint.textToPrint.Trim() != "")
                            execBeforePrintEveryChatLine(ref elToPrint);

                        if (elToPrint.testingInfoTextToPrint != "")
                        {
                            MainTextBox.text = elToPrint.textToPrint;
                            //MainTextBoxTMP.text = elToPrint.textToPrint;
                            testingInfoBox.transform.Find("MainText").GetComponent<Text>().text = elToPrint.testingInfoTextToPrint;
                            elToPrint.testingInfoTextToPrint = "";
                        }

                        GameObject btnOk = null;


                        if (elToPrint.buttonToPrint != null)
                        {
                            //Debug.Log("++++++++++++++++++----> Im here..");
                            //GameObject btn;
                            if (elToPrint.buttonToPrint.label != "")
                            {
                                //btnOk = showOff.printButton(elToPrint.buttonToPrint.label, elToPrint.buttonToPrint.callback);
                                btnOk = printButton(elToPrint.buttonToPrint.label, elToPrint.buttonToPrint.callback);
                                //Getting the Ok button..
                                //btnOk = btnOkPosition.transform.GetChild(0).gameObject;
                                elToPrint.buttonToPrint.label = "";
                                elToPrint.buttonToPrint.callback = null;
                                //System.Object obj = new System.Object();
                                //obj.Dispose();

                                if (animateDialogueText)
                                {
                                    //btnOk.SetActive(false); //Will be active after text animation is finished
                                    btnOkPosition.SetActive(false);
                                }
                            }
                        }


                        if (elToPrint.buttonsToPrint != null)
                        {
                            if (elToPrint.buttonsToPrint.buttons.Count > 0)
                            {
                                //showOff.PrintButtons(elToPrint.buttonsToPrint.buttons, elToPrint.buttonsToPrint.startingAConversation);
                                PrintButtons(elToPrint.buttonsToPrint.buttons, elToPrint.buttonsToPrint.startingAConversation);
                                elToPrint.buttonsToPrint.buttons.Clear();
                                elToPrint.buttonsToPrint.startingAConversation = false;

                                if (animateDialogueText)
                                    btnPositionsGO.SetActive(false); //Will be active after text animation is finished
                            }
                        }


                        if (elToPrint.textToPrint.Trim() != "")
                        {
                            Debug.Log("++++++++++++++++++----> There some textToPrint..");

                            //Entity name..
                            if (elToPrint.entityName.Trim() != "")
                            {
                                //DSys.DSysComp.entityNameTMP.text = elToPrint.entityName;
                                //DSys.DSysComp.entityNameTMP.ForceMeshUpdate();
                                entityNameTMP.text = elToPrint.entityName;
                                entityNameTMP.ForceMeshUpdate();
                            }

                            //Img avatar..
                            string entityImg = elToPrint.entityImg.Trim();
                            if ( entityImg != "")
                            {
                                //DSys.DSysComp.imageAvatar.sprite = null;
                                imageAvatar.sprite = null;
                                //Verify if it wasnt loaded yet..
                                int k = ImagesAvatarsLoaded.FindIndex(t => t.fileName == entityImg);
                                if (k >= 0)
                                {
                                    //DSys.DSysComp.imageAvatar.sprite = ImagesAvatarsLoaded[k].sprite;
                                    imageAvatar.sprite = ImagesAvatarsLoaded[k].sprite;
                                }
                                else
                                {
                                    String filename = Path.GetFileNameWithoutExtension(entityImg);
                                    //Image avatars must be on main 'Assets\Resources' folder
                                    Texture2D texture = Resources.Load(avatarsResourcesSubfolder + "\\" + filename) as Texture2D;
                                    if (texture != null)
                                    {
                                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                                        //DSys.DSysComp.imageAvatar.sprite = sprite;
                                        imageAvatar.sprite = sprite;
                                        ImagesAvatarsLoaded.Add(new ImageAvatarLoaded(entityImg, sprite));
                                    }
                                    else
                                        Debug.LogWarning("The image on resources folder: " + avatarsResourcesSubfolder + "\\" + elToPrint.entityImg + " could not be found.");
                                }

                            }

                            String txt = elToPrint.textToPrint.Trim(); 
                            MainTextBox.text = txt;
                            MainTextBoxTMP.text = txt;
                            MainTextBoxTMP.ForceMeshUpdate();

                            callbacksExecutionAfterLinePrinted();

                            if (animateDialogueText)
                            {
                                MainTextBoxTMP.maxVisibleCharacters = 0;
                                //executeAnimationDialogText(btnOk, btnPositionsGO);
                                executeAnimationDialogText(btnOkPosition, btnPositionsGO);
                                Debug.Log("+++++++++++++++++++++++++++++----------------------->elToPrint keep going..");
                            }
                            //executeAnimationDialogText();
                            //Action exec2 = () =>
                            //        {
                            //            AnimatingDialogueTextComp.AnimateText();
                            //            while (AnimatingDialogueTextComp.aAnimationIsRunning)
                            //            {
                            //                //
                            //            }
                            //        };
                            //Action exec =
                            //    async () =>
                            //        //() =>
                            //        {
                            //            await Task.Run(exec2);
                            //        };

                            //Task task = new Task(exec2);
                            ////task.Start();
                            //task.RunSynchronously();
                            //exec();
                            //AnimatedDialogueText.restartAnimation = true;
                            //AnimatedDialogueText.ExecAnimateDialogueText(elToPrint.textToPrint);
                            elToPrint.textToPrint = "";
                            elToPrint.entityName = "";
                        }


                    }
                }

                NextNodeIdSet = DSys.DSysCompLocal.NextNodeIdSet;

                ////Testing values on global variables..
                //if (ready)
                //{
                //    var byName = (IDictionary<string, object>)DSys.GlobalVariables;
                //    //bool val = (bool)byName["got_sword"];
                //    var gotSword = DSys.GlobalVariables.got_sword;
                //    bool abcExist = propertyExist( DSys.GlobalVariables, "abc" );

                //    Debug.Log("got_sword: "+ gotSword +" of type: "+ gotSword.GetType().ToString() + "  <<<<<<<<<<<<<<<<");
                //}

                //if (!testInitButtonTak){
                //    //Show Talk button, for test..
                //    Task.Run(async () =>
                //    {
                //        await Task.Run(() =>
                //        {
                //            SomeOperationsForTest.SomeOperTest.HoldSeconds(3000);
                //            ButtonTalkGO.GetComponent<Animator>().Play(ButtonTalkGO.GetComponent<ButtonBehaviors>().showAnimation);
                //        });

                //    });
                //    testInitButtonTak = true;
                //}
            }
        }




        // Will execute before print every chat line. 
        // Parameter elToPrint is the current chat element processed (text line, entity name, etc) 
        // that will be printed
        public void execBeforePrintEveryChatLine(ref DialogSystem.ShowOffNode.ElementToPrint elToPrint)
        {
            // Define what will happen here..
            //// Testing..
            //counterTest++;
            ////textForTextBox = counterTest.ToString();
            //elToPrint.textToPrint = counterTest.ToString() + " - " + elToPrint.textToPrint;

        }



        // Setting the callbacks (Functions) that will be executed when every
        // chat line has been printed  
        // Those Functions must return a boolean. If the function return false,
        // it will be performed again on the next chat line print. Otherwise, it will
        // not be executed again
        public void settingCallbacksAfterEveryChatLinePrinted()
        {
            // Define those callbacks (Functions) here..
            // Use the 'callbacksToExecAfterEveryChatLine' variable to do so..
            // Example: 
            //Func<bool> callbackX =
            //   () =>
            //       {
            //           if (x > 0)
            //               return true;
            //           else
            //               return false;
            //       };
            //callbacksToExecAfterEveryChatLine.Add(new callbackToExecuteAfterEveryChatLine(callbackX));
            
            //Testing..
            Func<bool> callbackX =
               () =>
                   {
                       Debug.LogWarning("callbacksToExecAfterEveryChatLine executed! - " + counterTest.ToString());
                       return true;
                   };
            callbacksToExecAfterEveryChatLine.Add(new callbackToExecuteAfterEveryChatLine(callbackX));

        }


        // Execute a list of callbacks (Functions) after every chat line print 
        // Note: Message Box verifications are made by the DialogStystem script, after
        // every chat line has processed to be printed
        public void callbacksExecutionAfterLinePrinted()
        {
            for (int q = 0; q < callbacksToExecAfterEveryChatLine.Count; q++)
            {
                if (callbacksToExecAfterEveryChatLine[q].done == false)
                {
                    callbacksToExecAfterEveryChatLine[q].done =  callbacksToExecAfterEveryChatLine[q].function();
                    //callbacksToExecAfterEveryChatLine[q].done = true;
                }
                else
                {
                    //Remove element after true execution..
                    callbacksToExecAfterEveryChatLine.RemoveAt(q);
                }
            }
        }



        // Setting the verifications (to show messages) that will be made when every
        // chat line has processed to be printed 
        public void settingVerificationsToShowMessagesAfterChatLines()
        {
            //Debug.Log( "On testVerificationBetweenNodeLines..." );
            
            messageToPrintVerification msgToPrintVerif;
            String msg;

            // Messages for the project example... 

            msg = "Greta got upset. You must find a way to please her.";
            msgToPrintVerif = new messageToPrintVerification(verifyFirstTalkToGreta, msg);
            DSys.msgsToPrintVerifications.Add(msgToPrintVerif);

            msg = "You've got the Radiada Flower! Use it to impress Greta.";
            msgToPrintVerif = new messageToPrintVerification(verifyJapaneseFlower, msg);
            DSys.msgsToPrintVerifications.Add(msgToPrintVerif);

            msg = "You've got the sword! Use it to defeat your enemies.";
            //Debug.Log("msg transl.: " + msg);
            msgToPrintVerif = new messageToPrintVerification(verifySword, msg);
            DSys.msgsToPrintVerifications.Add(msgToPrintVerif);

            msg = "A message after another one..";
            msgToPrintVerif = new messageToPrintVerification(verifyFolowMessage, msg);
            DSys.msgsToPrintVerifications.Add(msgToPrintVerif);

            msg = "This is the last message. Go get them!";
            msgToPrintVerif = new messageToPrintVerification(verifyLastMessage, msg);
            DSys.msgsToPrintVerifications.Add(msgToPrintVerif);


            Func<bool> action = new Func<bool>(() =>
            {
                bool ret = false;
                ret = mainGameData.GlobalVariables.data.show_message3;
                return ret;
            });
            msg = "You have to get the sword! ";
            msgToPrintVerif = new messageToPrintVerification(action, msg);
            DSys.msgsToPrintVerifications.Add(msgToPrintVerif);

        }


        public bool verifyFirstTalkToGreta()
        {
            bool ret = false;
            ret = mainGameData.GlobalVariables.data.talk_to_greta_01;
            return ret;
        }

        public bool verifyJapaneseFlower()
        {
            bool ret = false;
            ret = mainGameData.GlobalVariables.data.got_japanese_flower_seeds;
            return ret;
        }

        public bool verifySword()
        {
            bool ret = false;

            bool gotSword = mainGameData.GlobalVariables.data.got_sword;
            if (gotSword)
                    ret = true;
            //}

            return ret;
        }

        public bool verifyFolowMessage()
        {
            bool ret = false;
            ret = mainGameData.GlobalVariables.data.show_message;
            return ret;
        }

        public bool verifyLastMessage()
        {
            bool ret = false;
            ret = mainGameData.GlobalVariables.data.show_message2;
            return ret;
        }


        //Get a property from a ExpandoObject if it exist
        public static bool propertyExist(dynamic variable, string property)
        //public static bool IsPropertyExist(dynamic settings, string name)
        {
            if (variable is ExpandoObject)
                return ((IDictionary<string, object>)variable).ContainsKey(property);

            return variable.GetType().GetProperty(property) != null;
        }



        void UpdateDialogBox()
        {
            //// Some tasks of DialogSystem must run on a separate thread (on a multithreading system), to 
            //// gain performance. Unity doesnt allow access game objects from the game main thread, so DialogSystem class
            //// can just exec the operations and store on variables. Here, we will verify if this vars are set and 
            //// update the game opjects..
            //var showOff = DSys.showOff;
            //var elToPrint = showOff.elementsToPrint;

            ////Debug.Log("elToPrint: " + elToPrint);
            ////Debug.Log("elToPrint.buttonsToPrint: " + elToPrint.buttonsToPrint);

            ////Verify if is the end of conversation to close de dialogbox

            ////Debug.Log("finishConversation: " + DSys.finishConversation);
            
            //// If the conversation is finished and theres no more messages to print, 
            //// hide the DialogBox within seting de dialogOn to false, indicating 
            //// the definitive end of the all conversation..
            //if ( DSys.finishConversation == true && dialogBeenShown && DSys.messagesToPrint.Count <= 0 && !messageBox.messageBoxOn)
            //{
            //    //DialogBoxGO.transform.Find("Content").GetComponent<Animator>().Play("dialogBoxHideAnim");
            //    hideDialogBox();
            //    DSys.finishConversation = false;
            //    dialogOn = false;
            //}

            ////if (dialogOn && !dialogBeenShown && !messageBox.messageBoxOn && DSys.messagesToPrint.Count > 0)
            //if ( dialogOn && !dialogBeenShown && !messageBox.messageBoxOn )
            //    {
            //    showDialogBox();
            //}

            //// If has something to print and the trigger talk from player is not 
            //// operating (executing some animation and stuff..), execute .. 
            //if (elToPrint != null)
            //{

            //    if (elToPrint.textToPrint != "")
            //    {
            //        MainTextBox.text = elToPrint.textToPrint;
            //        elToPrint.textToPrint = "";
            //    }

            //    if (elToPrint.buttonToPrint != null)
            //    {
            //        if (elToPrint.buttonToPrint.label != "")
            //        {
            //            showOff.printButton(elToPrint.buttonToPrint.label, elToPrint.buttonToPrint.callback);
            //            elToPrint.buttonToPrint.label = "";
            //            elToPrint.buttonToPrint.callback = null;
            //            //System.Object obj = new System.Object();
            //            //obj.Dispose();
            //        }
            //    }

            //    if (elToPrint.buttonsToPrint != null)
            //    {
            //        if (elToPrint.buttonsToPrint.buttons.Count > 0)
            //        {
            //            showOff.PrintButtons(elToPrint.buttonsToPrint.buttons, elToPrint.buttonsToPrint.startingAConversation);
            //            elToPrint.buttonsToPrint.buttons.Clear();
            //            elToPrint.buttonsToPrint.startingAConversation = false;
            //        }
            //    }

            //}
        }

        //[InitializeOnLoadMethod]
        //static void OnProjectLoadedInEditor()
        //{
        //    Debug.Log("Project loaded in Unity Editor"); 
        //    //accessing
        //}

        //[InitializeOnLoadMethod]
        //void accessingFunc()
        //{
        //    Debug.Log("Non static method..");
        //}

        //void updateEntitiesFromStatic(List<DialogSysEntitiesComponent> entStatic)
        //{
        //    EntitiesComponent = entStatic; 
        //}


        public void PopulateEntitiesComponent()
        {
            //Debug.Log("Up and running");


            EntityProfile[] objectsFound = null;

            //Getting all GameObjects with EntitiesProfile Component..
            // If user sets the Entity main game object to look at it, access it first..
            if (rootEntitiesGOName.Trim() != "")
            {
                //Debug.Log("&& --> Root Entity gameObject name set: "+ rootEntitiesGOName);

                //If object exist..
                GameObject rootEntitiesGO = GameObject.Find(rootEntitiesGOName);
                if ( rootEntitiesGO != null)
                {
                    //Debug.Log("&& --> Root Entity gameObject name set and found..");
                    objectsFound = rootEntitiesGO.GetComponentsInChildren<EntityProfile>();
                }
            }
            else // If not, try to find it searching on obj. with EntityProfile component..
            {
                objectsFound = FindObjectsOfType<EntityProfile>();
            }

            //If found the specific entities..
            if (objectsFound != null)
            {

                List<GameObject> EntitiesGameObjects = new List<GameObject>();
                foreach (var item in objectsFound)
                {
                    GameObject gameObjectReal = item.transform.gameObject;
                    Debug.Log("Infos GO: " + item.name + " / System Name: " + item.SystemName);

                    //if (gameObjectReal == null)
                    //{
                    //    //Try to get the entity from main Entity gameObject
                    //    gameObjectReal = GameObject.Find("Entities").transform.Find(item.SystemName).GetComponent<GameObject>();
                    //}
                    EntitiesGameObjects.Add(gameObjectReal);
                    //gameObjectReal.transform.Find("Cube").gameObject.AddComponent<Rigidbody>();
                }

                //pathToXmlFile = "C:\\www\\dialogsystem\\DialogSystem\\CSharp\\testing\\xml\\Testing XML\\user_data.xml";

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(pathToXmlFile);

                // Getting Entities
                XmlNodeList result = xmlDoc.SelectNodes("/data/nodes_entities//node");
                XmlNode entity;
                EntitiesComponent = new List<DialogSysEntitiesComponent>();
                //foreach (XmlNode ds in result)
                for (int i = 0; i < result.Count; i++)
                {
                    //Console.WriteLine(ds["node_title"].InnerText);
                    entity = result[i];
                    //Console.WriteLine(entity.Attributes["nid"].Value);
                    Debug.Log(entity.Attributes["nid"].Value);
                    //Console.WriteLine(entity["system_name"].InnerText);
                    Debug.Log(entity["system_name"].InnerText);
                    //dynamic jsonObj = JsonConvert.DeserializeObject<ExpandoObject>(entity["raw"].InnerText);
                    string jsonText = JsonConvert.SerializeXmlNode(entity);
                    dynamic jsonObj = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
                    //Removing datasets from entities.. It will be store on a different variable
                    jsonObj.node.datasets = null;
                    DialogSysEntitiesComponent newEntityComp = new DialogSysEntitiesComponent();
                    //DialogSysEntitiesComponent newEntityComp = gameObject.AddComponent(typeof(DialogSysEntitiesComponent)) as DialogSysEntitiesComponent; ;
                    newEntityComp.populate(entity["name"].InnerText, entity.Attributes["nid"].Value, jsonObj.node, entity["system_name"].InnerText, entity["type"].InnerText, entity["pic"].InnerText);

                    //Search the gameObject to put
                    GameObject gameObjFound = EntitiesGameObjects.FirstOrDefault(t => t.GetComponent<EntityProfile>().SystemName == entity["system_name"].InnerText);
                    if (gameObjFound != null)
                    {
                        Debug.Log("gameObjFound != null ...");
                        newEntityComp.GameObj = gameObjFound;

                        // Verifying if gameobject TriggerTalkFront are on entity trigger layer ..
                        Transform triggerTalkFront = gameObjFound.transform.Find("TriggerTalkFront");
                        if (triggerTalkFront.gameObject.layer != LayerMask.NameToLayer(entityTriggerLayerName))
                        {
                            LayerMask layer = LayerMask.NameToLayer(entityTriggerLayerName);
                            triggerTalkFront.gameObject.layer = layer;
                        }

                        //Putting the DSysComponent on the TriggerTalk component.. - Already done on TriggerTalk
                        //gameObjFound.transform.Find("TriggerTalk").GetComponent<TriggerTalk>().DSysComp = this;
                    }
                    else
                    {
                        Debug.LogWarning("gameObject not found: "+ entity["system_name"].InnerText);
                    }

                    EntitiesComponent.Add(newEntityComp);
                }
            }
            else
            {
                Debug.LogError("None entity found with the EntityProfile component on it..");
            }
            
        }


        //public void executeAnimationDialogText(GameObject btnGameObj = null, bool isTalk = false)
        //{
        //    //if animating text, finish first, after that execute..
        //    if (animateDialogueText)
        //    {
        //        if(!isTalk && btnGameObj != null)
        //            btnGameObj.SetActive(false);
        //        //DSys.DSysComp.AnimatedDialogueText.gameObjectButton = newBtnGO;
        //        Action exec =
        //            async () =>
        //                //() =>
        //                {
        //                await Task.Run(() =>
        //                {
        //                    AnimatingDialogueTextComp.AnimateText(MainTextBoxTMP.textInfo.characterCount);
        //                    while (AnimatingDialogueTextComp.aAnimationIsRunning)
        //                    {
        //                            //
        //                    }
        //                });
        //                if (!isTalk && btnGameObj != null)
        //                    btnGameObj.SetActive(true);
        //            };
        //        //Task task = new Task(temp);
        //        //task.Start();
        //        exec();

        //        //btnSon.onClick.AddListener(() =>
        //        ////btnSon.GetComponent<ButtonBehaviorsBase>().AddCallback( () =>
        //        //{
        //        //    if (DSys.DSysComp.AnimatedDialogueText.aAnimationIsRunning)
        //        //    {
        //        //        Debug.Log("---- Animation is running when press button..");
        //        //        DSys.DSysComp.AnimatedDialogueText.finishAnimation = true;
        //        //        //btnSon.onClick.RemoveAllListeners();
        //        //        //btnSon.GetComponent<ButtonBehaviorsBase>().AddCallback(actFinal);
        //        //        //actFinal();
        //        //    }
        //        //    else
        //        //    {
        //        //        Debug.Log("---- Animation is NOT running ..");
        //        //        actFinal();
        //        //    }
        //        //    //btnSon.onClick.RemoveAllListeners();
        //        //});

        //    }
        //    //else
        //    //{
        //    //    btnSon.GetComponent<ButtonBehaviorsBase>().AddCallback(actFinal);
        //    //}
        //}


        public void executeAnimationDialogText(GameObject btnOk = null, GameObject btnsOptions = null)
        {

            //Debug.Log("====> executeAnimationDialogText: Task #" + Task.CurrentId.ToString());
            //ShowThreadInformation("executeAnimationDialogText");

            Action exec = () =>
            {
                int totalChars = MainTextBoxTMP.textInfo.characterCount;
                Debug.Log("totalChars: " + totalChars);
                AnimatingDialogueTextComp.AnimateText(totalChars);
                Debug.Log("aAnimationIsRunning: " + AnimatingDialogueTextComp.aAnimationIsRunning);
                while (AnimatingDialogueTextComp.aAnimationIsRunning)
                {
                    //Debug.Log("----------------- on the while..");
                }
            };

            Action execFinalAsync = async () =>
            {

                //Debug.Log( "====> execFinalAsync: Task #" + Task.CurrentId.ToString() );
                //ShowThreadInformation("execFinalAsync");
                await Task.Run(exec);

                //Debug.LogWarning("========  >>>>> Will activate buttons: " );

                if (btnOk != null)
                {
                    //Debug.LogWarning("====>>> btnOk: " + btnOk.name);
                    btnOk.SetActive(true);
                }

                if (btnsOptions != null)
                {
                    //Debug.LogWarning("====>>> btnsOptions: " + btnsOptions.name);
                    btnsOptions.SetActive(true);
                }
                
            };

            Action execFinal = () =>
            {
                exec();
                if (btnOk != null)
                    btnOk.SetActive(true);
                if (btnsOptions != null)
                    btnsOptions.SetActive(true);

            };

            execFinalAsync();
            //execFinal();

            Debug.Log("----------------------------->finish executeAnimationDialogText2..");
        }


        //private static void ShowThreadInformation(String taskName)
        //{
        //    String msg = null;
        //    Thread thread = Thread.CurrentThread;
        //    //lock (lockObj)
        //    //{
        //        msg = String.Format("=========> {0} thread information\n", taskName) +
        //              String.Format("   Background: {0}\n", thread.IsBackground) +
        //              String.Format("   Thread Pool: {0}\n", thread.IsThreadPoolThread) +
        //              String.Format("   Thread ID: {0}\n", thread.ManagedThreadId);
        //    //}
        //    //Debug.Log(msg);
        //}



        public void printNextButton(String nid = "", string parentNid = "")
        {
            //Console.WriteLine("Click for the next..");
            Debug.Log("Click for the next..");
            //Debug.Log("Click for the next..");
            //Console.ReadLine();
            //UnityEvent ShowingOffNodeEvent = new UnityEvent();
            //if (dialSysComp.BtnNext != null)
            //{
            //dialSysComp.enableNextBtn();
            //Creating buttons based on btnBase (that is a GameObject that has a button inside)
            //btnNextPosition is a GameObj that will hold the new next button.. 
            //GameObject btnPosNext = DSysComp.btnOkPosition;
            //GameObject newBtnGO = Instantiate<GameObject>(DSysComp.btnBase);
            GameObject btnPosNext = btnOkPosition;
            GameObject newBtnGO = Instantiate<GameObject>(btnBase);
            //Set new button of child of btnPos
            newBtnGO.transform.parent = btnPosNext.transform;
            newBtnGO.transform.position = btnPosNext.transform.position;
            //btnSon is a button that is son of the new created button GameObject
            Button btnSon = newBtnGO.GetComponentInChildren<Button>();
            //To generalize the behavior of the button, it will be named to 'Ok' 
            btnSon.GetComponentInChildren<Text>().text = "Ok";
            //bool buttonPressed = false;
            //WaitClass wait = gameObject.AddComponent<WaitClass>();
            WaitClass _wait = newBtnGO.AddComponent<WaitClass>();
            _wait.StartWait();

            btnSon.onClick.AddListener(() =>
            {
                if (nid != "")
                {
                    //ShowingOffNode(nid);
                }
                //dialSysComp.BtnNext.onClick.RemoveAllListeners();
                Destroy(newBtnGO);
                _wait.StoptWait();
            });
            DSys.DisableNextButton = true;

            //Wait for the button to be pressed
            //wait.LetsGo();

        }




        public GameObject printButton(String label, Action callback)
        {
            //Translating the label button..
            Debug.Log("==> label: " + label);
            label = DSys.translateText(label);

            //Console.WriteLine("Print button");
            Debug.Log("On printButton()... Click for the next..");
            //GameObject btnPosNext = DSys.DSysComp.btnOkPosition;
            GameObject btnPosNext = btnOkPosition;
            //GameObject newBtnGO = Instantiate<GameObject>(DSys.DSysComp.btnBase, btnPosNext.transform);
            GameObject newBtnGO = Instantiate<GameObject>(btnBase, btnPosNext.transform);
            //Set new button of child of btnPos
            //newBtnGO.transform.parent = btnPosNext.transform;
            newBtnGO.transform.position = btnPosNext.transform.position;
            //btnSon is a button that is son of the new created button GameObject
            Button btnSon = newBtnGO.GetComponentInChildren<Button>();
            btnSon.Select();
            //To generalize the behavior of the button, it will be named to 'Ok' 
            btnSon.GetComponentInChildren<Text>().text = label;
            //bool buttonPressed = false;
            //WaitClass wait = gameObject.AddComponent<WaitClass>();
            //WaitClass _wait = newBtnGO.AddComponent<WaitClass>();

            //Set focus on the button
            //DSys.DSysComp.eventSystem.SetSelectedGameObject(btnSon.gameObject);
            eventSystem.SetSelectedGameObject(btnSon.gameObject);
            //EventSystemManager.currentSystem.SetSelectedGameObject(btnSon, null);

            btnSon.onClick.AddListener(() =>
            {
                //dialSysComp.BtnNext.onClick.RemoveAllListeners();
                //Destroy(newBtnGO);
                //callback();
            });
            //Action callbck = () =>
            //{
            //    Action clbck = callback;
            //    DSys.verificationAfterLinePrinted(clbck);
            //};

            Action actFinal;

            Action act = () =>
            {
                //DSys.callbacksExecutionAfterLinePrinted();
                DSys.messageVerificationAfterLinePrinted(callback);
            };


            //if (DSys.DSysComp.runAsynchronously)
            if (runAsynchronously)
            {
                actFinal = async () =>
                {
                    await Task.Run(act);
                };
            }
            else
            {
                actFinal = () =>
                {
                    act();
                };
            }

            //btnSon.GetComponent<ButtonBehaviorsBase>().AddCallback(() =>
            //{
            //    if (DSys.DSysComp.runAsynchronously)
            //    {
            //        ExecAsyncShowingOffNode(actFinal);
            //    }
            //    else
            //    {
            //        actFinal();
            //    }

            //});

            //DSys.DSysComp.executeAnimationDialogText(newBtnGO);


            btnSon.GetComponent<ButtonBehaviorsBase>().AddCallback(actFinal);

            Debug.Log("DSys.finishConversation: " + DSys.finishConversation + " ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");

            //If the conversation has finish, sinalize the variable on the last click button
            //if (DSys.finishConversation)
            //{
            //    btnSon.GetComponent<ButtonBehaviorsBase>().AddCallback(() =>
            //    {
            //        DSys.lastClickConversation = true;
            //    });
            //}

            //DSys.DisableNextButton = true;

            return newBtnGO;
        }




        public void printOkButton(String nid = "", string parentNid = "")
        {
            //Console.WriteLine("Click for the next..");
            Debug.Log("Click for the next..");
            //Debug.Log("Click for the next..");
            //Console.ReadLine();
            //UnityEvent ShowingOffNodeEvent = new UnityEvent();
            //if (dialSysComp.BtnNext != null)
            //{
            //dialSysComp.enableNextBtn();
            //Creating buttons based on btnBase (that is a GameObject that has a button inside)
            //btnNextPosition is a GameObj that will hold the new next button.. 
            //GameObject btnPosNext = DSys.DSysComp.btnOkPosition;
            //GameObject newBtnGO = Instantiate<GameObject>(DSys.DSysComp.btnBase);
            GameObject btnPosNext = btnOkPosition;
            GameObject newBtnGO = Instantiate<GameObject>(btnBase);
            //Set new button of child of btnPos
            newBtnGO.transform.parent = btnPosNext.transform;
            newBtnGO.transform.position = btnPosNext.transform.position;
            //btnSon is a button that is son of the new created button GameObject
            Button btnSon = newBtnGO.GetComponentInChildren<Button>();
            //To generalize the behavior of the button, it will be named to 'Ok' 
            btnSon.GetComponentInChildren<Text>().text = "Ok";
            //bool buttonPressed = false;
            //WaitClass wait = gameObject.AddComponent<WaitClass>();
            WaitClass _wait = newBtnGO.AddComponent<WaitClass>();

            btnSon.onClick.AddListener(() =>
            {
                if (nid != "")
                {
                    //ShowingOffNode(nid);
                    //nextNodeId = nid;
                }
                //dialSysComp.BtnNext.onClick.RemoveAllListeners();
                _wait.StoptWait();
                Destroy(newBtnGO);
            });

            _wait.StartWait();
            DSys.DisableNextButton = true;

            //Wait for the button to be pressed
            //wait.LetsGo();

        }


        // Print option buttons (connections) to be chosen ..
        public void PrintButtons(List<ButtonNode> buttons, bool startingAConversation = false)
        {
            //Console.WriteLine();
            //Console.Write("Option(s): ");
            //Debug.Log("parentNid: " + parentNid);
            Debug.Log("");
            Debug.Log("Option(s): ");

            //Limiting to 3 buttons..
            int btnPositionLimit = 5;
            int i = 0;
            //dialSysComp.disableButtons();
            foreach (ButtonNode btn in buttons)
            {
                if (i < btnPositionLimit)
                {
                    //Console.Write(" [" + btn.Nid + "]: " + btn.Title);
                    string nid = btn.Nid;
                    string aux_id = btn.Aux_Id;

                    //Creating buttons based on btnBase (that is a GameObject that has a button inside)
                    //btnsPositions are a set of GameObj that will hold the new buttons.. 
                    //GameObject btnPos = DSys.DSysComp.btnsOptionsPositions[i];
                    //GameObject newBtnGO = Instantiate<GameObject>(DSys.DSysComp.btnBase);
                    GameObject btnPos = btnsOptionsPositions[i];
                    GameObject newBtnGO = Instantiate<GameObject>(btnBase);
                    //Set new button of child of btnPos
                    newBtnGO.transform.SetParent(btnPos.transform, false);
                    newBtnGO.transform.position = btnPos.transform.position;
                    //btnSon is a button that is son of the new created button GameObject
                    Button btnSon = newBtnGO.GetComponentInChildren<Button>();
                    //btnSon.GetComponentInChildren<Text>().text = "test button";
                    //btnSon.onClick.AddListener(() => { clickFunc(btn); });

                    //dialSysComp.enableButtom(i);
                    string btnTitle = btn.Title;
                    string btnNid = "";
                    //Translating the text button..
                    Debug.Log("==> btnTitle: " + btnTitle);

                    btnTitle = DSys.translateText(btnTitle);

                    //if (DSys.DSysComp.showTestingInfo)
                    if (showTestingInfo)
                    {
                        btnNid = " [" + btn.Nid + "]: ";
                    }

                    btnSon.GetComponentInChildren<Text>().text = btnNid + btnTitle;
                    Debug.Log(" [" + btn.Nid + "]: " + btnTitle);
                    //Debug.Log(" [" + btn.Nid + "]: " + btn.Title, false);

                    //WaitClass _wait = newBtnGO.AddComponent<WaitClass>();
                    //bool go = false;

                    //Set focus on the button
                    //DSys.DSysComp.eventSystem.SetSelectedGameObject(btnSon.gameObject);
                    eventSystem.SetSelectedGameObject(btnSon.gameObject);

                    //If the conversation has finish, sinalize the variable on the last click button
                    Debug.Log("DSys.finishConversation: " + DSys.finishConversation + " ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");


                    btnSon.onClick.AddListener(() =>
                    {
                        string btnSonLocName = btnSon.transform.parent.name;

                        //First hide main obtion button clicked, after that hide and destroy all others
                        Action hideOtherButtons = () =>
                        {
                            foreach (var btnOptPos in btnsOptionsPositions)
                            {
                                if (btnOptPos.transform.childCount > 0)
                                {
                                    //Button btnChild = btnsOptPos.transform.GetComponentInChildren<Button>();
                                    if(btnOptPos.name != btnSonLocName)
                                    {
                                        ButtonBehaviorsBase btnBehBase = btnOptPos.transform.GetComponentInChildren<Button>().GetComponent<ButtonBehaviorsBase>();
                                        btnBehBase.CleanCallbacks();
                                        btnBehBase.hideElement();
                                    }
                                }
                            }
                        };

                        btnSon.GetComponent<ButtonBehaviorsBase>().AddCallback(hideOtherButtons);

                        //Killing all options buttons
                        //foreach (var btnsOptPos in DSys.DSysComp.btnsOptionsPositions)
                        //foreach (var btnsOptPos in btnsOptionsPositions)
                        //{
                        //    if (btnsOptPos.transform.childCount > 0)
                        //    {
                        //        GameObject btnGO = btnsOptPos.transform.GetChild(0).gameObject;
                        //        Debug.Log("GO name: " + btnGO.name);
                        //        Destroy(btnGO);
                        //    }
                        //}

                        string nid2 = nid;
                        int i2 = i;
                        //ShowingOffNode(nid, startingAConversation);
                        DSys.showOff.ShowingOffNode(nid, aux_id, startingAConversation);
                        //nextNodeId = nid;
                        //go = true;
                        //_wait.StoptWait();
                        //Debug.Log("nextNodeId: " + nextNodeId);
                        Debug.Log("nextNodeId: " + DSys.showOff.nextNodeId);
                        Debug.Log("btn i: " + i2);
                        //dialSysComp.BtnsOptions[i].onClick.RemoveAllListeners();
                        //dialSysComp.disableButtons();


                        //if (DSys.finishConversation)
                        //{
                        //    DSys.lastClickConversation = true;
                        //}

                    });

                    //_wait.StartWait();
                    //Debug.Log("_wait.waiting: " + _wait.waiting);
                    //while (!go)
                    //{
                    //    Debug.Log("Still waiting...");
                    //}
                    //Debug.Log("Keep going.. <-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-");

                    if (i < (btnPositionLimit - 1)) i++;
                }
            }
            //Console.Write(" : ");
            Debug.Log(" : ");
            //Debug.Log(" : ");

        }


        //Print the last Next btn to finalize a conversation
        public void printFinalizeConversationButton()
        {
            //Console.WriteLine("Click for the next..");
            Debug.Log("Click for finalize conversation..");
            //Console.ReadLine();
            //UnityEvent ShowingOffNodeEvent = new UnityEvent();

            //Creating button based on btnBase (that is a GameObject that has a button inside)
            //btnNextPosition is a GameObj that will hold the new next button.. 
            //GameObject btnPosNext = DSysComp.btnOkPosition;
            GameObject btnPosNext = btnOkPosition;
            //GameObject newBtnGO = Instantiate<GameObject>(DSysComp.btnBase);
            GameObject newBtnGO = Instantiate<GameObject>(btnBase);
            //Set new button of child of btnPos
            newBtnGO.transform.parent = btnPosNext.transform;
            newBtnGO.transform.position = btnPosNext.transform.position;
            //btnSon is a button that is son of the new created button GameObject
            Button btnSon = newBtnGO.GetComponentInChildren<Button>();
            btnSon.GetComponentInChildren<Text>().text = "Finalize";

            btnSon.onClick.AddListener(() =>
            {
                //DSysComp.MainTextBox.text = "End of conversation.";
                //DSysComp.MainTextBoxTMP.text = "End of conversation.";
                MainTextBox.text = "End of conversation.";
                MainTextBoxTMP.text = "End of conversation.";
                //dialSysComp.BtnNext.onClick.RemoveAllListeners();
                Destroy(newBtnGO);
            });
            DSys.DisableNextButton = true;
        }


        // This function is necessary? Nodes will be shown for specific chars, if we are
        // talking with them an such.. Maybe for testing pourposes will implement it... Elaborate this more...
        public void ShowMainRootNodes()
        {
            //Console.WriteLine("");
            //Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            //Console.WriteLine("on ShowMainRootNodes()..");
            Debug.Log("");
            Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            Debug.Log("on ShowMainRootNodes()..");

            List<ButtonNode> rootNodesListButtons = new List<ButtonNode>();
            // Verify, for every main root node, if its conditions are ok
            // to create the node buttons..
            //MainRootNodes
            //foreach (MainRootNode rootNode in MainRootNodes)
            foreach (DialogSystem.MainRootNode rootNode in DSys.MainRootNodes)
            {
                //XmlNodeList result = rootNode.SelectNodes("./wrapper/conditions/content/raw");
                //if (result.Count > 0)
                //{
                //string condition = result[0]["pre"].InnerText;
                foreach (XmlNode n in DSys.DialogsDataOutput)
                {
                    if (n["id"].InnerText == rootNode.nid)
                    {
                        if (DSys.verifyConditions(n))
                        {
                            //rootNode = rootNode["node"];
                            //Console.WriteLine(rootNode["id"].InnerText);
                            //Console.WriteLine(rootNode["wrapper"]["node_title"].InnerText);
                            rootNodesListButtons.Add(new ButtonNode(n["id"].InnerText, n["aux_id"].InnerText, n["node_title"].InnerText));
                        }
                    }
                }
                //}
            }
            /*ShowOffNode*/
            DSys.showOff = new DialogSystem.ShowOffNode(DSys);
            PrintButtons(rootNodesListButtons, true);
            //Console.WriteLine();
            Debug.Log("");

            //rootNodesList.Add("");
            //Console.WriteLine("You choose: " + choose + "..");
            //found = true;
            //ShowingOffNode(choose);
            //break;


            if (true)
            {
                //var me = this
                //var main = this._main


                //var select = me._models.find('#model_sel_filter').clone()
                //select.removeAttr('id')

                //me.nodeRoots = []

                //me.nodeHolderPile = []

                //$('nodes_dialogs>nodes>node').each(function(){

                //        console.log('on dialogs_nodes>nodes>node..')

                //    var node = $(this)
                //    let nid = node.attr('nid') || ''
                //    let btnNodeSelec = me._models.find("#btn-node-conn").clone()
                //    btnNodeSelec.removeAttr('id')
                //    btnNodeSelec.attr('nid', nid)
                //    btnNodeSelec.attr('title', 'Id: ' + nid)
                //    btnNodeSelec.text(node.find('>wrapper>node_title').text())
                //    select.find('options').append(btnNodeSelec)

                //    btnNodeSelec.on('click', function(){

                //            console.log('clicked nodeSelect..')
                //        let nid = $(this).attr('nid') || ''
                //        me._GLOBALdebug = main._GLOBAL
                //        me.testNDebugFirstTasks(nid)
                //        main._variablesStates[0] = { G: '', node: [] }
                //            me._idStateSaved = null
                //        select.find('options button').attr('disabled', 'true')
                //        select.find(".inp-filter").remove()
                //        me.scrollDebugAndTestContent()
                //    })
                //    me.nodeRoots.push(nid)
                //})

                //// Executing filtering on input
                //select.find(".inp-filter").on("keyup", function(){
                //        // console.log("value: " + $(this).val());
                //        main._ui.filterList( $(this).val(), select.find('options button'))
                //})


                //select.prepend('Select a conversation.. </br>')


                //if (elemToPut !== null)
                //    {
                //    $(elemToPut).append(select)
                //}

                //    me.scrollDebugAndTestContent()
                //return select
            }

        }


        // Update the Main GlobalVariables or the GlobalVariables of the Dialogsystem class
        public void LocalGlobalVarsToMainGlobalVars()
        {
            //Updating the MainGameData Class
            // Converting to GlobalVariablesClass
            String jsonText = JsonConvert.SerializeObject(DSys.GlobalVariables);
            jsonText = "{ \"data\": " + jsonText + " }";
            //DSysComp.MainGlobalVariables = 
            //    JsonConvert.DeserializeObject<DialogClasses.MainGlobaVariablesClass.Root>(jsonText);
            mainGameData.GlobalVariables = JsonConvert.DeserializeObject<MainGD.MainGameData.MainGlobaVariablesClass.Root>(jsonText);
            int a = 1;
            
        }

        public void MainGlobalVarsToLocalGlobalVars()
        {
            string jsonText = JsonConvert.SerializeObject(mainGameData.GlobalVariables.data);
            DSys.GlobalVariables = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
            int a = 1;
        }

        public void LocalDatasetsToMainDatasets()
        {
            if (DSys.Datasets.Count > 0)
            {
                //Erasing all Main Game Data Datasets..
                //DSysComp.mainGameData.Datasets = new List<MainGD.MainGameData.MainDialogSysDatasetClass>();
                mainGameData.Datasets = new MainGameData.MainDialogSysDatasetClass();

                foreach (var ds in DSys.Datasets)
                {
                    //List<DialogClassesOperator.DatasetClassListEntry> listOfDatasetClasses = 
                    //    new List<DialogClassesOperator.DatasetClassListEntry>();



                    ////Searching for the respective class contructor
                    //List<MainGD.MainGameData.DialogClassesOperatorClass.DatasetClassListEntry> listOfDatasetClasses =
                    //    DSysComp.mainGameData.DialogClassesOperator.ListOfDatasetClasses;
                    //int k = listOfDatasetClasses.FindIndex(t => t.mdid == ds.Mdid);
                    ////If item found..
                    //if (k >= 0)
                    //{
                    //    //ConstructorInfo constr = listOfDatasetClasses[k].constructor;
                    //    //var datasetModel = constr.Invoke(new object[] { });

                    //    //Converting from json to dataset model
                    //    //ds.Data.__did = ds.Did;
                    //    String jsonText = JsonConvert.SerializeObject(ds.Data);
                    //    string jsonTextFormated = JsonConvert.SerializeObject(ds.Data, Newtonsoft.Json.Formatting.Indented);
                    //    ////jsonText = "{ \"data\": " + jsonText + " }";
                    //    ////Type tipo = typeof(); 
                    //    ////string qualifiedName = typeof(YourClass).AssemblyQualifiedName;
                    //    //Type type = Type.GetType(listOfDatasetClasses[k].classType);
                    //    ////Type type2 = GetType(listOfDatasetClasses[k].classType);
                    //    ////Type type2 = Type.GetType("DatasetModel01.Root");
                    //    ////Type type3 = Type.GetType("DialogClasses.DatasetModel01");
                    //    ////Type type4 = Type.GetType("DialogClasses.DatasetModel01.Root");
                    //    ////MainGameData.MainGlobaVariablesClass
                    //    ///
                    //    Type t = listOfDatasetClasses[k].classType;

                    //    // var data = JsonConvert.DeserializeObject(jsonText, t);
                    //    //var dataConv = Convert.ChangeType(data, type);

                    //    //MainGD.MainGameData.MainDialogSysDatasetClass mainDataset = new MainGD.MainGameData.MainDialogSysDatasetClass(ds.Did, ds.Mdid, JsonConvert.DeserializeObject(jsonText, t), jsonTextFormated);
                    //    //["DatasetModel" + ds.Mdid] (ds.Did, ds.Mdid, JsonConvert.DeserializeObject(jsonText, t), jsonTextFormated);
                    //    //DSysComp.mainGameData.Datasets.Add(mainDataset);
                    //}

                    ds.Data.__did = ds.Did; //Saving the dataset id direct into the data for the MainGameData gets it when necessary
                    String jsonText = JsonConvert.SerializeObject(ds.Data);
                    mainGameData.Datasets.addElementToDatasetList(ds.Mdid, jsonText);

                }
            }
            //return true;
        }




        //public void MainDatasetsToLocalDatasets()
        //{
        //    ////foreach (var ds in Datasets)
        //    //foreach (var DS in DSysComp.mainGameData.Datasets)
        //    //{
        //    //    //List<DialogClassesOperator.DatasetClassListEntry> listOfDatasetClasses = 
        //    //    //    new List<DialogClassesOperator.DatasetClassListEntry>();



        //    //    //Searching for the respective dataset on the local variable
        //    //    //List<DialogClassesOperator.DatasetClassListEntry> listOfDatasetClasses =
        //    //    //    DSysComp.mainGameData.DialogClassesOperator.ListOfDatasetClasses;
        //    //    int k = Datasets.FindIndex(t => t.Did == DS.Did);
        //    //    //If item found..
        //    //    if (k >= 0)
        //    //    {
        //    //        //ConstructorInfo constr = listOfDatasetClasses[k].constructor;
        //    //        //var datasetModel = constr.Invoke(new object[] { });

        //    //        //Converting to json 
        //    //        String jsonText = JsonConvert.SerializeObject(DS.Data);

        //    //        Datasets[k].Data = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);

        //    //    }
        //    //}
        //    mainGameData.MainDatasetsToDialogSysDatasets();
        //}


        // At this MainDatasetsToDialogSysDatasets() function, set a foreach for each dataset model declared:
        // Ex:
        //    Foreach for dataset model 1 
        //    foreach (var DS in Datasets.ListDatasetModel_1) {
        //      act(DS);
        //    }
        public void MainDatasetsToLocalDatasets()
        {

            //foreach (var DS in Datasets.ListDatasetModel_1)
            //{
            //    int k = DialogSystem.Datasets.FindIndex(t => t.Did == DS.__did);
            //    //If item found..
            //    if (k >= 0)
            //    {
            //        //Converting to json 
            //        String jsonText = JsonConvert.SerializeObject(DS);

            //        DialogSystem.Datasets[k].Data = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
            //    }
            //}

            //foreach (var DS in Datasets.ListDatasetModel_2)
            //{

            //    int k = DialogSystem.Datasets.FindIndex(t => t.Did == DS.__did);
            //    //If item found..
            //    if (k >= 0)
            //    {
            //        //Converting to json 
            //        String jsonText = JsonConvert.SerializeObject(DS);

            //        DialogSystem.Datasets[k].Data = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
            //    }
            //}


            Action<dynamic> act = (DS) => {
                int k = DSys.Datasets.FindIndex(t => t.Did == DS.__did);
                //If item found..
                if (k >= 0)
                {
                    //Converting to json 
                    String jsonText = JsonConvert.SerializeObject(DS);

                    DSys.Datasets[k].Data = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
                }
            };

            // A 'foreach' must be created for each dataset model..

            //Foreach for dataset model 1 
            foreach (var DS in mainGameData.Datasets.ListDatasetModel_1)
            {
                act(DS);
            }

            //Foreach for dataset model 2 
            foreach (var DS in mainGameData.Datasets.ListDatasetModel_2)
            {
                act(DS);
            }

        }

        //private void MainDatasetsToLocalDatasets_Do(dynamic DS)
        //{
        //    int k = DSys.Datasets.FindIndex(t => t.Did == DS.__did);
        //    //If item found..
        //    if (k >= 0)
        //    {
        //        //Converting to json 
        //        String jsonText = JsonConvert.SerializeObject(DS);

        //        DSys.Datasets[k].Data = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
        //    }
        //}


    }





    

    //// Setting up the button on the Inspector to execute populate the entities component, 
    //// besides shows infos from several vars..
    //#if UNITY_EDITOR
    //[CustomEditor(typeof(DSysComponent))]
    //public class DSysEditor : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        DrawDefaultInspector();
    //        //base.OnInspectorGUI();
    //        DSysComponent myScript = (DSysComponent)target;
    //        if (GUILayout.Button("Build Entities"))
    //        {
    //            //Verify if the entity trigger layer exist..
    //            int mask = LayerMask.GetMask(myScript.entityTriggerLayerName);
    //            Debug.Log("------->>>>> Trigger layer mask:"+ mask);
    //            if (mask == 0)
    //            {
    //                EditorApplication.isPlaying = false;
    //                Debug.LogError("Layer for enitity trigger: " + myScript.entityTriggerLayerName + " not exist. Create it.");
    //            }

    //            myScript.PopulateEntitiesComponent();
    //            //EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    //            EditorUtility.SetDirty(myScript);
    //        }

    //        if (GUILayout.Button("Prepare Internal Components"))
    //        {
    //            //if()
    //            RectTransform rectTransform = myScript.GetComponentInParent<Transform>().Find("DialogBox").GetComponent<RectTransform>();
    //            rectTransform.localScale = new Vector3(1, 1, 1);
    //            rectTransform.sizeDelta = new Vector2( Camera.main.pixelWidth, Camera.main.pixelHeight);
    //            rectTransform.pivot.Set(.5f, .5f);
    //            //rectTransform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));

    //            //Getting the main EventSystem
    //            myScript.eventSystem = Transform.FindObjectOfType<EventSystem>();
    //            EditorUtility.SetDirty(myScript);
    //        }

    //        //Showing off the values of the vars on Inspector..
    //        GUILayout.Label("=================================== ");
    //        GUILayout.Label("Variable values: ");
    //        GUILayout.Label("Component is ready?: " + myScript.ready);
    //        GUILayout.Label("Player ID: " + myScript.playerId);
    //        GUILayout.Label("dialogOn: " + myScript.dialogOn);
    //        GUILayout.Label("dialogBeenShown: " + myScript.dialogBeenShown);
    //        GUILayout.Label("buttonTalkOn: " + myScript.buttonTalkOn);
    //        GUILayout.Label("btnTalkReady: " + myScript.btnTalkReady);
    //        GUILayout.Label("entityReadyToTalkWithPlayer: " + myScript.entityReadyToTalkWithPlayer);
    //        GUILayout.Label("NextNodeIdSet: " + myScript.NextNodeIdSet);
            

    //    }
    //}
    //#endif

    [System.Serializable]
    public class DialogSysEntitiesComponent
    {
        public String Name; // Entity name
        public String Type; // Entity type
        public String Nid; // Entity id
        public String SystemName; // Entity system name
        public String Pic; // Entity pic
        public GameObject GameObj; // Game object 


        public void populate(String name, String nid, dynamic data, String system_name = null, String type = null, String pic = null)
        {
            Nid = nid.Trim();
            Name = name.Trim();
            SystemName = system_name.Trim();
            Type = type.Trim();
            Pic = pic.Trim();

            //Data = data;
        }
    }



    /*
    public class AnimateDialogueText
    {
        public TMP_Text DialogTextBox;

        //The Speed the text is animated on screen. Waits 0.05 seconds before animating the next character.
        //Useful for letting the player accelerate the speed animation.
        public float speedText = 0.05f;
        public int speedText2 = 500;

        public void AnimatedDialogueText(TMP_Text TMPText, string stringToAnimate)
        {
            DialogTextBox = TMPText;
            ExecAnimateDialogueText(stringToAnimate);
        }

        public void OnUpdate(string stringToAnimate)
        {
            //Simple controls to accelerate the text speed.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                speedText = speedText / 100;
                if (speedText2 > 10)
                    speedText2 = speedText2 - 10;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                speedText = 0.05f;
                speedText2 = 500;
            }

            //if (Input.GetKeyDown(KeyCode.KeypadEnter))
            //{
            //    ExecAnimateDialogueText(stringToAnimate);
            //}
        }

        //Animate the chars apearing on the dialogue text box
        public void ExecAnimateDialogueText(string text)
        {
            StartCoroutine(AnimateTextCoroutine(text));
            //AnimateText(text);
        }

        private IEnumerator AnimateTextCoroutine(string text)
        {
            DialogTextBox.text = text;
            //DialogTextBox.text = "";
            DialogTextBox.ForceMeshUpdate();

            int totalCharacters = DialogTextBox.textInfo.characterCount;
            //int totalCharacters = text.Length;
            Debug.Log("characterCount: " + totalCharacters);

            int i = 0;
            while (i < totalCharacters)
            //while (i < totalCharacters - 1)
            {
                Debug.Log("Animating...");
                DialogTextBox.maxVisibleCharacters = i;
                //DialogTextBox.text += text[i];
                //DialogTextBox.ForceMeshUpdate();
                i++;
                yield return new WaitForSeconds(speedText);
            }

            Debug.Log("Done animating!");
            System.Threading.Thread.Sleep(2000); ;

            //StartCoroutine(AnimateTextCoroutine(text));

        }
    }
    */
}




