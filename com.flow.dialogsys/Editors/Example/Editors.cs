using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DialogSysComp;
using UnityEngine.EventSystems;

//Script containing all personalized Editors
// Its necessary to put Editors classes on a separated script on a folder called 'Editors'

// Shows infos on Inspector..
#if UNITY_EDITOR

// Dialog System Editors are in a different file, inside app plugin folder



[UnityEditor.CustomEditor(typeof(MovePlayerPhysics))]
public class MovePlayerPhysEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //base.OnInspectorGUI();
        MovePlayerPhysics myScript = (MovePlayerPhysics)target;

        //Showing off the values of the vars on Inspector..
        GUILayout.Label("=================================== ");
        GUILayout.Label("Variable values: ");
        if (myScript.playerTriggerTalkFront != null)
            GUILayout.Label("Player Trigger Talk Front: " + myScript.playerTriggerTalkFront.name);
        else
            GUILayout.Label("Player Trigger Talk Front: null");

        if (myScript.otherEntityToTalk != null)
            GUILayout.Label("Player Trigger Talk Front: " + myScript.otherEntityToTalk.name);
        else
            GUILayout.Label("Other Entity To Talk: null");

        GUILayout.Label("Stop Moving: " + myScript.stopMoving);

    }
}

[CustomEditor(typeof(MoveNpc))]
public class MoveNpcEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MoveNpc myScript = (MoveNpc)target;

        //Showing off the values of the vars on Inspector..
        GUILayout.Label("=================================== ");
        GUILayout.Label("Variable values: ");
        GUILayout.Label("Is Looking: " + myScript.isLooking);
        GUILayout.Label("Stop Moving: " + myScript.stopMoving);

        if (myScript.rb != null)
            GUILayout.Label("Rigid Body: " + myScript.rb.name);
        else
            GUILayout.Label("Rigid Body: null");

        if (myScript.goingTo != null)
            GUILayout.Label("Going To: " + myScript.goingTo.name);
        else
            GUILayout.Label("Going To: null");
    }
}


#endif