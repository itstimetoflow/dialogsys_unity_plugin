using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MessageBox it's a dialog box that appears between dialog chats, indicating some additional info, 
// like when the player get some new item.

public class MessageBox : MonoBehaviour
{
    public bool messageBoxOn = false;
    private string messageBoxShowAnimation = "messageBoxShowAnim";
    private string messageBoxHideAnimation = "messageBoxHideAnim";
    private Animator animator;
    private CanvasGroup canvasGroup;
    public DialogSysComp.DSysComponent DSysComp = null;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        //gameObject.SetActive(false);
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        if(DSysComp == null)
        {
            DSysComp = Transform.FindObjectOfType<DialogSysComp.DSysComponent>();
        }
    }

    public void showMessageBox()
    {
        messageBoxOn = true;
        canvasGroup.blocksRaycasts = true;
        //gameObject.SetActive(true);
        //MessageBoxGO.transform.Find("Canvas").Find("Content").GetComponent<Animator>().Play(messageBoxShowAnimation);
        animator.Play(messageBoxShowAnimation);
        DSysComp.audioManager.messageBox.Play();
        
    }

    public void hideMessageBox()
    {
        //MessageBoxGO.transform.Find("Canvas").Find("Content").GetComponent<Animator>().Play(messageBoxHideAnimation);
        animator.Play(messageBoxHideAnimation);
        //button.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void onMessageBoxFinishHide()
    {
        messageBoxOn = false;
    }

   //public void onFinishHide()
   // {
   //     //GameObject me = transform.gameObject;
   //     gameObject.SetActive(false);

   // }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
