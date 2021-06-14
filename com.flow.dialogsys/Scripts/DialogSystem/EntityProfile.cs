using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using DialogSys;
using DialogSysComp;

public class EntityProfile : MonoBehaviour
{
    public List<string> Tags;
    public string SystemName;
    [HideInInspector]
    public string Nid = "";
    [HideInInspector]
    public bool ReadyToTalk = false;
    private string DSystemCompHolder = "DialogSystem";
    private DSysComponent DSysComp;
    private DialogSystem DSys;
    private int counter = 0;


    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void Start()
    {
        
    }


    private void Update()
    {
        //Verify if nid is already gotten
        //Dont put it on Start() cause DialogSystem can be setted after
        //Try to do it until Nid != null or counter > 300
        if( (Nid == null || Nid == "") && counter <= 300 )
        {
            //DSystemCompHolder = ;
            DSysComp = GameObject.Find("DialogSystem").GetComponent<DSysComponent>();
            if (DSysComp.ready)
            {
                DSys = DSysComp.DSys;
                //if (DSys != null)
                //{
                    Debug.Log("DSys exist..");
                    GetEntityId();
                //}
            }
        }

        if (counter < 310) counter++;
    }


    public void GetEntityId()
    {
        //DSys.
        //XmlNodeList result = DSys.Entities .SelectNodes("/data/global_variables/w/raw");
        //XmlNode xmlNode;


        int i = DSysComp.DSys.Entities.FindIndex( t => t.SystemName == SystemName );
        if (i >= 0)
        {
            Nid = DSys.Entities[i].Nid;
        }
    }


    // Shows infos from several vars..
    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(EntityProfile))]
    public class EntitiyProfileEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            //base.OnInspectorGUI();
            EntityProfile myScript = (EntityProfile)target;

            //Showing off the values of the vars on Inspector..
            GUILayout.Label("Variable values: ");
            GUILayout.Label("Entity Nid: " + myScript.Nid);
            GUILayout.Label("Ready to talk? " + myScript.ReadyToTalk);

        }
    }
    #endif

}
