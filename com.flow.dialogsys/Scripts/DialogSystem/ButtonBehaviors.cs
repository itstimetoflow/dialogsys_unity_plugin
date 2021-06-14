using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using DialogSys;
using DialogSysComp;

public class ButtonBehaviors : MonoBehaviour
{
    public String nidDialogToStart = "1";
    public bool buttonTalkReady = false;
    public DSysComponent DSysComp;
    //public Action callbackFirst; //First to be executed
    public Action callback;
    //public bool verificationOfDsysComp = false;
    public Text mainTextBox;
    public bool showOnInit = true;
    public string showAnimation = "buttonShowAnim";
    public string hideAnimation = "buttonHideAnim";
    public bool isTalkButton = false;
    public bool btnClicked = false;
    public bool animOnClick = true;
    //public bool killButtonAfterHide = true;

    void OnInitButton()
    {
        if (showOnInit)
        {
            gameObject.GetComponent<Animator>().Play(showAnimation);
            if (isTalkButton)
            {
                //transform.GetComponentInChildren<Button>().Select();
                DSysComp.buttonTalkOn = true;
            }
        }

    }

    public void showButton()
    {
        gameObject.GetComponent<Animator>().Play(showAnimation);
        Debug.Log("showing button..");
        if (isTalkButton)
            DSysComp.buttonTalkOn = true;
    }


    void OnAnimButtonIsIdle()
    {
        buttonTalkReady = true;
        GetComponent<Button>().interactable = true;
        Debug.Log("DialogSys: Button ready..");

        //Testing hiding via code..
        //hideButton();
        transform.GetComponentInChildren<Button>().Select();
    }

    public void OnClickButton()
    {
        if (buttonTalkReady)
        {
            Debug.Log("DialogSys: Button is ready and clicked..");
            btnClicked = true;
        }
        if(animOnClick )
            gameObject.GetComponent<Animator>().Play(hideAnimation);

    }

    public void OnFinishHideButtonGoOpenDialogBox()
    {
        //if (nid != "" && DSysComp != null)
        //{
        //    DSysComp.OpenDialogBox(nid);
        //}
        //else
        //{
        //    Debug.Log("Nid == null!  000000000000000000000000000000000000000000000000");
        //}
        //mainTextBox.text = "";
    }


    public void OnFinishHideButton()
    {
        GetComponent<Button>().interactable = false;

        if (isTalkButton)
            DSysComp.buttonTalkOn = false;

        //If DSysComp is not present, try to find it..
        if (DSysComp == null)
        {
            Debug.Log("Im here..");
            //Searching for the main gameobject that holds the DialogSysComponent..
            DSysComp = GameObject.Find("DialogSystem").GetComponent<DSysComponent>();
            mainTextBox = DSysComp.MainTextBox;
            //DSysComp.DSys.showOff.elementsToPrint = null;
            Debug.Log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&\n  MainTextBox: " + mainTextBox.text );
            //mainTextBox.text = "";
        }
        else
        {
            if (btnClicked)
            {
                btnClicked = false;
                if (nidDialogToStart != "")
                {
                    DSysComp.OpenDialogBox(nidDialogToStart);
                }
                else
                {
                    Debug.Log("Nid == null!  000000000000000000000000000000000000000000000000");
                }
            }
            //mainTextBox.text = "";
        }

        if (callback != null)
        {
            callback();
        }

        //Destroy(gameObject);
    }

    //Hiding the button by code
    public void hideButton()
    {
        
        Animator animator = gameObject.GetComponent<Animator>();
        animator.Play(hideAnimation);
        //Below code tested, but not working..
        //Debug.Log("Animator is playing: "+ AnimatorIsPlaying(animator));
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("hidding"));
        //Debug.Log("Will hide button..");
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName(hideAnimation));
        //while (animator.GetCurrentAnimatorStateInfo(5).IsTag("hidding"))
        //{
        //    Debug.Log("Animation: " + hideAnimation + " is performing..");
        //}
        //Debug.Log("Hide button finished..");
    }


    bool AnimatorIsPlaying(Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }


    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Button>().interactable = false;
        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            OnClickButton();
        });
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    

}
