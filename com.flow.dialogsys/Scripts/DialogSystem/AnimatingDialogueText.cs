using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DialogSysComp;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class AnimatingDialogueText : MonoBehaviour
{

    TMP_Text DialogTextBox;
    private DSysComponent DSysComponent;
    [TextArea]
    string stringToAnimate;

    //The speed that text chars will be shown on dialog. 
    //public float speedTextDelete = 0.05f;
    public int speedText = 20;
    int speedTextNow = 20;
    //public bool talkButtonPressed = false;
    [HideInInspector]
    public bool finishAnimation = false;
    [HideInInspector]
    public bool aAnimationIsRunning = false;
    [HideInInspector]
    public int maxVisibleCharacters;
    [HideInInspector]
    public int totalCharacters;
    [HideInInspector]
    public bool forceMeshUpdate;
    //public GameObject gameObjectButton;
    [HideInInspector]
    public bool updateMaxVisibleChars = false;
    [HideInInspector]
    bool playSondOnFinishAnimation = false;


    // Start is called before the first frame update
    void Start()
    {
        DSysComponent = gameObject.GetComponent<DialogSysComp.DSysComponent>();
        DialogTextBox = DSysComponent.MainTextBoxTMP;
        
        //On click 'Talk' button, restart dialog text to max visible chars to 0
        Button talkButton = DSysComponent.ButtonTalkGO.GetComponentInChildren<Button>();
        talkButton.onClick.AddListener(()=>{
            //DialogTextBox.maxVisibleCharacters = 0;
            //talkButtonPressed = true;
        });

        //maxVisibleCharacters = DialogTextBox.textInfo.characterCount;
        DialogTextBox.text = "";
        maxVisibleCharacters = 9999;
        //Button talkButton = DSysComponent.ButtonTalkGO.GetComponentInChildren<Button>();

        speedTextNow = speedText;

    }


    // Update is called once per frame
    void Update()
    {
        //if the dialog box is opened..
        if ( DSysComponent.ready && DSysComponent.dialogOn && DSysComponent.animateDialogueText)
        {
            //If theres text to animate..
            //if (stringToAnimate.Trim() != "")
            //if (DialogTextBox.textInfo.characterCount > 0 && DialogTextBox.maxVisibleCharacters == 0)
            //{
            //    //ExecAnimateDialogueText(stringToAnimate);
            //    ExecAnimateDialogueText();

            //}


            
            //totalCharacters = DialogTextBox.textInfo.characterCount;

            //updateMaxVisibleChars = true;
            //if (updateMaxVisibleChars)
            if (DialogTextBox.maxVisibleCharacters != maxVisibleCharacters)
                {
                DialogTextBox.maxVisibleCharacters = maxVisibleCharacters;
                DialogTextBox.ForceMeshUpdate();
                //    updateMaxVisibleChars = false;
            }

            if (playSondOnFinishAnimation)
            {
                DSysComponent.audioManager.typing.Play();
                playSondOnFinishAnimation = false;
            }

            //if (forceMeshUpdate)
            //{
            //    DialogTextBox.ForceMeshUpdate();
            //    forceMeshUpdate = false;
            //}

            //if (DSysComponent.dialogOn)
            //{

                //Simple controls to accelerate the text speed.
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //speedTextDelete = speedTextDelete / 100;
                    if (speedTextNow > 10)
                        speedTextNow = speedTextNow - 10;
                }
                else if (Input.GetKeyUp(KeyCode.Space))
                {
                    //speedTextDelete = 0.05f;
                    speedTextNow = speedText;
                }

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    //ExecAnimateDialogueText(stringToAnimate);
                    finishAnimation = true;
                }
            //}

        }
        //Debug.Log("maxVisibleCharacters: " + DialogTextBox.maxVisibleCharacters);

        
    }



    ////Animate the chars apearing on the dialogue text box
    //public void ExecAnimateDialogueText(string text = "")
    ////public void ExecAnimateDialogueText()
    //{
    //    //finishAnimation = true;
    //    //while (aAnimationIsRunning)
    //    //{
    //    //    //
    //    //}
    //    StartCoroutine(AnimateTextCoroutine(text));
    //    //AnimateText(text);
    //}



    //private IEnumerator AnimateTextCoroutine(string text = "")
    //{
    //    finishAnimation = false;
    //    aAnimationIsRunning = true;

    //    if (text.Trim() != "")
    //    {
    //        DialogTextBox.text = text;
    //    }

    //    //DialogTextBox.text = "";
    //    DialogTextBox.ForceMeshUpdate();

    //    int totalCharacters = DialogTextBox.textInfo.characterCount;
    //    //int totalCharacters = text.Length;
    //    Debug.Log("characterCount: " + totalCharacters);

    //    int i = 0;
    //    int z = -3000; //To prevent infinite loop
    //    DialogTextBox.maxVisibleCharacters = 0;

    //    while (i < totalCharacters && z < 0)
    //    //while (i < totalCharacters - 1)
    //    {
    //        if (finishAnimation)
    //        {
    //            Debug.Log("%%%%%%%%%%  Restart animation...");
    //            DialogTextBox.maxVisibleCharacters = totalCharacters;
    //            //finishAnimation = false;
    //            break;
    //        }
    //        Debug.Log("Animating...");
    //        DialogTextBox.maxVisibleCharacters = i;
    //        //DialogTextBox.text += text[i];
    //        //DialogTextBox.ForceMeshUpdate();
    //        i++;
    //        z++;
    //        yield return new WaitForSeconds(speedText);
    //    }


    //    //if (restartAnimation)
    //    //{
    //    //    Debug.Log("%%%%%%%%%%  Restart animation...");
    //    //    //DialogTextBox.maxVisibleCharacters = totalCharacters;
    //    //    //DialogTextBox.maxVisibleCharacters = 0;
    //    //    restartAnimation = false;
    //    //}

    //    if (z >= 0 )
    //        Debug.Log("Probably happened a infinite loop...");

    //    Debug.Log("Done animating!");
    //    //System.Threading.Thread.Sleep(2000); 

    //    //stringToAnimate = "";

    //    //StartCoroutine(AnimateTextCoroutine(text));
    //    aAnimationIsRunning = false;
    //}



    public void AnimateText(int totalChars)
    {
        //gameObjectButton.SetActive(false);
        //gameObjectButton.GetComponent<ButtonBehaviorsBase>().hideElement();

        //finishAnimation = false;
        aAnimationIsRunning = true;

        //if (text.Trim() != "")
        //{
        //    DialogTextBox.text = text;
        //}


        //DialogTextBox.text = "";
        //DialogTextBox.ForceMeshUpdate();
        forceMeshUpdate = true;

        totalCharacters = totalChars;
        Debug.Log("totalCharacters: " + totalChars);

        int i = 0;
        int z = -3000; //To prevent infinite loop
        maxVisibleCharacters = 0;

        while (i <= totalChars && z < 0)
        //while (i < totalCharacters - 1)
        {
            if (finishAnimation)
            {
                Debug.Log("%%%%%%%%%%  Restart animation...");
                maxVisibleCharacters = totalChars;
                updateMaxVisibleChars = true;
                //finishAnimation = false;
                break;
            }
            //Debug.Log("Animating...");
            maxVisibleCharacters = i;
            updateMaxVisibleChars = true;
            //DialogTextBox.text += text[i];
            //DialogTextBox.ForceMeshUpdate();
            i++;
            z++;
            System.Threading.Thread.Sleep(speedTextNow); ;
            //yield return new WaitForSeconds(speedText);
        }
        playSondOnFinishAnimation = true;
        finishAnimation = false;
        //i = 0;
        //z = -3000;


        //if (restartAnimation)
        //{
        //    Debug.Log("%%%%%%%%%%  Restart animation...");
        //    //DialogTextBox.maxVisibleCharacters = totalCharacters;
        //    //DialogTextBox.maxVisibleCharacters = 0;
        //    restartAnimation = false;
        //}

        if (z >= 0)
            Debug.Log("Probably happened a infinite loop...");

        Debug.Log("Done animating!");
        //System.Threading.Thread.Sleep(2000); 

        //stringToAnimate = "";

        //StartCoroutine(AnimateTextCoroutine(text));
        aAnimationIsRunning = false;
        //gameObjectButton.SetActive(true);
        //gameObjectButton.GetComponent<ButtonBehaviorsBase>()
        //gameObjectButton.GetComponent<ButtonBehaviorsBase>().showElement();
    }


    // Setting up the button on the Inspector to execute populate the entities component, 
    // besides shows infos from several vars..
#if UNITY_EDITOR
    [CustomEditor(typeof(AnimatingDialogueText))]
    public class AnimatingDialogueTextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            //base.OnInspectorGUI();
            AnimatingDialogueText myScript = (AnimatingDialogueText)target;

            //Showing off the values of the vars on Inspector..
            GUILayout.Label("=================================== ");
            GUILayout.Label("Variable values: ");
            GUILayout.Label("Finish Animation: " + myScript.finishAnimation);
            GUILayout.Label("A animation is running: " + myScript.aAnimationIsRunning);
            GUILayout.Label("Max Visible Characters: " + myScript.maxVisibleCharacters);
            GUILayout.Label("Total Characters: " + myScript.totalCharacters);
            GUILayout.Label("Force Mesh Update: " + myScript.forceMeshUpdate);
            GUILayout.Label("Update Max Visible Chars: " + myScript.updateMaxVisibleChars);

        }
    }
#endif
}
