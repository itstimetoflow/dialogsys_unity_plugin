using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavMessageBox : ButtonBehaviorsBase
{

    //protected bool destroyElemAfterFinish = false;

    //For test
    public ButtonBehavTalk buttonTalkTest;

    // Start is called before the first frame update
    private void Start()
    {
        destroyElemAfterFinish = false;
        animateHideOnClick = false;
        ExecOnStart();

        //For test
        //AddCallback(testFunc);
    }

    //For test
    public void testFunc()
    {
        //Showing the Talk test button..
        buttonTalkTest.showElement();
    }
    
}
