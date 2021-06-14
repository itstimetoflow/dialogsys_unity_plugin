using UnityEngine;
using UnityEditor;
using DialogSysComp;
using UnityEngine.EventSystems;



//Script containing all personalized Editors
// Its necessary to put Editors classes on a separated script on a folder called 'Editors'

// Shows infos on Inspector..
#if UNITY_EDITOR


// Setting up the button on the Inspector to execute populate the entities component, 
// besides shows infos from several vars..
[CustomEditor(typeof(DSysComponent))]
public class DSysEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //base.OnInspectorGUI();
        DSysComponent myScript = (DSysComponent)target;
        if (GUILayout.Button("Build Entities"))
        {
            //Verify if the entity trigger layer exist..
            int mask = LayerMask.GetMask(myScript.entityTriggerLayerName);
            Debug.Log("------->>>>> Trigger layer mask:" + mask);
            if (mask == 0)
            {
                EditorApplication.isPlaying = false;
                Debug.LogError("Layer for enitity trigger: " + myScript.entityTriggerLayerName + " not exist. Create it.");
            }

            myScript.PopulateEntitiesComponent();
            //EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorUtility.SetDirty(myScript);
        }

        if (GUILayout.Button("Prepare Internal Components"))
        {
            Transform dialogBoxTransf = myScript.GetComponentInParent<Transform>().Find("DialogBox").parent;
            if (myScript.messageBox != null)
            {
                dialogBoxTransf.localScale = new Vector3(1, 1, 1);
                RectTransform rectTransform = myScript.GetComponentInParent<Transform>().Find("DialogBox").GetComponent<RectTransform>();
                rectTransform.localScale = new Vector3(1, 1, 1);
                rectTransform.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
                rectTransform.pivot.Set(.5f, .5f);
                //rectTransform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                GUILayout.Label("Transformations applyied to MessageBox game object.");
            }
            else
            {
                Debug.LogError("Cant find DialogBox game object.");
            }

            //dialogBoxTransf.Find("")
            myScript.ButtonTalkGO.transform.localPosition = new Vector3(1, -130, 1);


            //Getting the main EventSystem
            myScript.eventSystem = Transform.FindObjectOfType<EventSystem>();
            EditorUtility.SetDirty(myScript);
        }

        //Showing off the values of the vars on Inspector..
        GUILayout.Label("=================================== ");
        GUILayout.Label("Variable values: ");
        GUILayout.Label("Component is ready?: " + myScript.ready);
        GUILayout.Label("Player ID: " + myScript.playerId);
        GUILayout.Label("dialogOn: " + myScript.dialogOn);
        GUILayout.Label("dialogBeenShown: " + myScript.dialogBeenShown);
        GUILayout.Label("buttonTalkOn: " + myScript.buttonTalkOn);
        GUILayout.Label("btnTalkReady: " + myScript.btnTalkReady);
        GUILayout.Label("entityReadyToTalkWithPlayer: " + myScript.entityReadyToTalkWithPlayer);
        GUILayout.Label("NextNodeIdSet: " + myScript.NextNodeIdSet);


    }
}



[CustomEditor(typeof(LanguageSelection))]
public class LanguageSelectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //base.OnInspectorGUI();
        LanguageSelection myScript = (LanguageSelection)target;
        
        //Showing off the values of the vars on Inspector..
        GUILayout.Label("=================================== ");
        GUILayout.Label("Variable values: ");
        GUILayout.Label("Id Language Selected: " + myScript.idLanguageSelected);
    }
}


[CustomEditor(typeof(TriggerTalk))]
public class TriggerTalkEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //base.OnInspectorGUI();
        TriggerTalk myScript = (TriggerTalk)target;

        //Showing off the values of the vars on Inspector..
        GUILayout.Label("=================================== ");
        GUILayout.Label("Variable values: ");
        GUILayout.Label("Dialog Nid to Talk: " + myScript.dialogNidToTalk);
        GUILayout.Label("Is operating? " + myScript.operating);
        GUILayout.Label("Who is talking front ID (reached by TriggerTalkFront): " + myScript.whoTalkFrontNid);

    }
}


#endif