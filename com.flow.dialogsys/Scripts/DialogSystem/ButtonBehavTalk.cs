using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DialogSys;
using DialogSysComp;


// This script is for buttons with these characteristics:
// Is the Talk button to be used by the DialogBox
// Tt will be visible after certain criteria is positive (trigger and logic stuff)
// Will be animated when hided will NEVER be destroied 


public class ButtonBehavTalk : ButtonBehaviorsBase
{

    private bool isTalkButton = true; //It ALWAYS set to TRUE
    public bool buttonTalkReady = false;
    public string nidDialogToStart = "1";
    public DSysComponent DSysComp;
    private Text mainTextBox;
    
    //private bool elemClicked = false;

    // Start is called before the first frame update
    private void Start()
    {
        destroyElemAfterFinish = false;
        showOnInit = false;
        
        ExecOnStart();

        button.onClick.AddListener(() =>
        {
            //DSysComp.executeAnimationDialogText(gameObject, true);
            OnClickButton();
        });
        
    }

    private void Update()
    {
        //Updating btn label..
        if (DSysComp.btnLabelChanged) {
            if (DSysComp != null && DSysComp.ready)
            {
                transform.GetComponentInChildren<Text>().text = DSysComp.DSys.translateText(DSysComp.btnTalkLabel);
                DSysComp.btnLabelChanged = false;
                Debug.Log("->-> Btn label updated");
            }
        }
    }


    public void showElement()
    {
        if (isTalkButton)
            DSysComp.buttonTalkOn = true;

        ExecShowElement();
    }


    void OnAnimButtonIsIdle()
    {
        buttonTalkReady = true;
        button.interactable = true;
        //Debug.Log("Talk button: ready..");
        button.Select();
    }


    public void OnClickButton()
    {
        Debug.Log("OnClickButton from ButtonBehavTalk");

        if (buttonTalkReady)
        {
            Debug.Log("Button is ready and clicked..");
            elemClicked = true;
        }

        ExecOnClickButton();
    }

    // The final animation has a flag that execute this function at its last frame
    public void OnFinishHideButton()
    {

        if (isTalkButton)
            DSysComp.buttonTalkOn = false;

        if (elemClicked)
        {

            ExecOnFinishHideButton();
            Debug.Log("After ExecOnFinishHideButton().. +++++++++++++++++++++++++++");
            //If DSysComp is not present, try to find it..
            if (DSysComp == null)
            {
                Debug.Log("Im here..");
                //Searching for the main gameobject that holds the DialogSysComponent..
                DSysComp = GameObject.Find("DialogSystem").GetComponent<DSysComponent>();
                mainTextBox = DSysComp.MainTextBox;
                //DSysComp.DSys.showOff.elementsToPrint = null;
                //Debug.Log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&\n  MainTextBox: " + mainTextBox.text);
                //mainTextBox.text = "";
            }
            else
            {
                //if (elemClicked)
                //{
                //elemClicked = false;
                if (nidDialogToStart != "")
                {
                    DSysComp.OpenDialogBox(nidDialogToStart);
                }
                //else
                //{
                //    Debug.Log("Nid == null!  000000000000000000000000000000000000000000000000");
                //}
                //}
                //mainTextBox.text = "";
            }
        }
    }
}
