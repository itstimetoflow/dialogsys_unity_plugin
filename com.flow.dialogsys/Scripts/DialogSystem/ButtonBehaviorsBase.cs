using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//using DialogSys;
//using DialogSysComp;

// This script is for buttons with these characteristics:
// Will be animated when initialized (fade in) 
// Supports multiple callbacks
// Will be animated when hided and destroied after 

public class ButtonBehaviorsBase : MonoBehaviour
{
    //public String nidDialogToStart = "1";
    //public bool buttonTalkReady = false;
    //public DSysComponent DSysComp;
    //public Action callbackFirst; //First to be executed
    //public Action callback;
    private List<Action> callbacks = new List<Action>();
    //public bool verificationOfDsysComp = false;
    //public Text mainTextBox;
    protected bool showOnInit = true;
    protected string showAnimation = "buttonShowAnim";
    protected string hideAnimation = "buttonHideAnim";
    //public bool isTalkButton = false;
    protected bool elemClicked = false;
    protected bool animateHideOnClick = true;
    protected Animator animator;
    protected Button button;
    //private bool execCallbacksAfterFinishHide = true;
    protected bool destroyElemAfterFinish = true;
    //public bool killButtonAfterHide = true;


    // Start is called before the first frame update
    private void Start()
    {
        ExecOnStart();

        button.onClick.AddListener(() =>
        {
            OnClickButton();
        });
    }

    public void ExecOnStart()
    {
        //Debug.Log("On Start..");

        button = GetComponent<Button>();
        animator = gameObject.GetComponent<Animator>();
        //callbacks = new List<Action>();

        if (showOnInit)
        {
            showElement();
        }


        //button.interactable = false;
        //button.onClick.AddListener(() =>
        //{
        //    OnClickButton();
        //});

        // For tests..
        //AddCallback(testCallback);
    }


    // For tests..
    public void testCallback()
    {
        Debug.Log("Test callback..");
    }


    // Update is called once per frame
    private void Update()
    {

    }


    //This funtion add callbacks to the end of callback queue
    public void AddCallback( Action callback )
    {
        callbacks.Add(callback);
    }

    public void CleanCallbacks()
    {
        callbacks.Clear();
    }

    // This function was created to not rewrite code on the son's classes
    public void ExecuteCallbacks()
    {
        //button.onClick.RemoveAllListeners();
        if( callbacks.Count > 0)
        {
            for (int i = 0; i < callbacks.Count; i++)
            {
                callbacks[i]();
                callbacks.RemoveAt(i); //Remove after exec
            }
        }
    }

    //void OnInitButton()
    //{
    //    if (showOnInit)
    //    {
    //        animator.Play(showAnimation);
    //        //if (isTalkButton)
    //        //{
    //        //    //transform.GetComponentInChildren<Button>().Select();
    //        //    DSysComp.buttonTalkOn = true;
    //        //}
    //    }

    //}

    public void showElement()
    {
        //if (isTalkButton)
        //    DSysComp.buttonTalkOn = true;

        ExecShowElement();
    }

    // This function was created to not rewrite code on the son's classes
    protected void ExecShowElement()
    {
        animator.Play(showAnimation);
        //Debug.Log("showing elem (btn)..");
    }


    //Hiding the button by code
    public void hideElement()
    {

        button.interactable = false;
        //Animator animator = gameObject.GetComponent<Animator>();
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


    void OnAnimButtonIsIdle()
    {
        //buttonTalkReady = true;
        button.interactable = true;
        //Debug.Log("DialogSys: Button ready..");

        //Testing hiding via code..
        //hideButton();
        button.Select();
    }

    public void OnClickButton()
    {
        Debug.Log("OnClickButton from ButtonBehaviorsBase..");
        //if (buttonTalkReady)
        //{
        //    Debug.Log("DialogSys: Button is ready and clicked..");
        //    elemClicked = true;
        //}
        elemClicked = true;
        ExecOnClickButton();
    }

    // This function was created to not rewrite code on the son's classes
    public void ExecOnClickButton()
    {

        if (animateHideOnClick)
        {
            animator.Play(hideAnimation);
        }
        else
        {
            ExecuteCallbacks();

            //Destroy element at the end, if setted..
            if (destroyElemAfterFinish)
                Destroy(gameObject);
        }
    }


    //public void OnFinishHideButtonGoOpenDialogBox()
    //{
    //    //if (nid != "" && DSysComp != null)
    //    //{
    //    //    DSysComp.OpenDialogBox(nid);
    //    //}
    //    //else
    //    //{
    //    //    Debug.Log("Nid == null!  000000000000000000000000000000000000000000000000");
    //    //}
    //    //mainTextBox.text = "";
    //}


    // The final animation has a flag that execute this function at its last frame
    public void OnFinishHideButton()
    {
        ExecOnFinishHideButton();
    }

    // This function was created to not rewrite code on the son's classes
    public void ExecOnFinishHideButton()
    {
        if (elemClicked)
        {
            elemClicked = false;
            ExecuteCallbacks();
        }

        //Destroy element at the end, if setted..
        if (destroyElemAfterFinish)
            Destroy(gameObject);
    }



    bool AnimatorIsPlaying(Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

}
