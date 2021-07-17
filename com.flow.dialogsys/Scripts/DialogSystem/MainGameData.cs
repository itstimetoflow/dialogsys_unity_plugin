using System;
using System.Collections.Generic;
using UnityEngine;
//using System.Reflection;
using Newtonsoft.Json;
//using DialogSys;
//using System.Dynamic;
//using DialogSysComp;

/// <summary>
/// ///////////////////////////////////////////////////////////////////////////////////////

/// This is the file that holds all variables that will interact with the dialog system. 
/// Likewise, you can use it to set all variables that will be accessed globally on your application 

/// /// ///////////////////////////////////////////////////////////////////////////////////
/// </summary>

namespace MainGD
{

    [Serializable]
    public class MainGameData : MonoBehaviour
    {
        //public DSysComponent DSysComp = null;
        //[HideInInspector]
        //public DialogSystem DialogSystem = null; //Disable -> Will be set on DSysComponent
        //public Bubu Test;
        public MainGlobaVariablesClass.Root GlobalVariables;
        //public List<MainDialogSysDatasetClass> Datasets;
        public MainDialogSysDatasetClass Datasets;
        public DialogClassesOperatorClass DialogClassesOperator;

        // Start is called before the first frame update
        void Start()
        {
            GlobalVariables = new MainGlobaVariablesClass.Root();
            //Datasets = new List<MainDialogSysDatasetClass>();
            Datasets = new MainDialogSysDatasetClass();
            DialogClassesOperator = new DialogClassesOperatorClass();

            //if (DSysComp == null)
            //{
            //    Debug.LogError("It's necessary to indicate the DialogSystem game object.");
            //}

        }



        //public void testCreateDataset()
        //{
        //    DatasetModel01 datasetModel01 = new DatasetModel01();
        //    //Saving the type of the object..
        //    //datasetModel01.data.health = 99;
        //    MainDialogSysDatasetClass dataset = new MainDialogSysDatasetClass("1", "1", datasetModel01, "test..");

        //    // get public constructors
        //    List<Type> datasetModelsTypes = new List<Type>();
        //    // determine type here
        //    //var type = typeof(DatasetModel01.Root);
        //    datasetModelsTypes.Add(typeof(DatasetModel01));
        //    Type tipo = typeof(DatasetModel01);

        //    ConstructorInfo constructor = tipo.GetConstructor(Type.EmptyTypes);

        //    // invoke the first public constructor with no parameters.
        //    var datasetModelA = constructor.Invoke(new object[] { });

        //}



        // Declare your global variables here.. 
        //It is preferable to declare numeric variables as float
        [Serializable]
        public class MainGlobaVariablesClass
        {

            [Serializable]
            public class Root // GlobaVariablesClass
            {
                public Data data; //{ get; set; }
            }

            [Serializable]
            public class A
            {
                public String c; //{ get; set; }
                public String d; //{ get; set; }
            }

            [Serializable]
            public class Teste
            {
                public A a; //{ get; set; }
                public string b; //{ get; set; }
            }

            [Serializable]
            public class Data
            {
                public bool found_gina;
                public bool welcome_message;
                public bool first_talk_nestor;
                public bool talk_to_greta_01;
                public bool talked_about_the_flower;
                public bool got_japanese_flower_seeds;
                public bool testing;
                public bool pass;
                public bool explanation; //{ get; set; }
                public bool got_sword; //{ get; set; }
                public bool show_message; //{ get; set; }
                public bool show_message2; //{ get; set; }
                public bool show_message3; //{ get; set; }
                public string vital; // { get; set; }
                public int strength; // { get; set; }
                public Teste teste; // { get; set; 
                public float counter;
            }
        }



        public class DialogClassesOperatorClass
        {
            public List<DatasetClassListEntry> ListOfDatasetClasses;

            public class DatasetClassListEntry
            {
                //The dataset model id
                public string mdid;
                //Constructor for a class type. It is responsible to help instantiate the class
                //public ConstructorInfo constructor;
                //public string classType;
                public Type classType;

                //public DatasetClassListEntry(string modelDsId, string type)
                public DatasetClassListEntry(string modelDsId, Type type)
                {
                    mdid = modelDsId;
                    //constructor = type.GetConstructor(Type.EmptyTypes);
                    classType = type;
                }
            }

            // Mount the list of the all datasets classes used by Datasets of the DialogSystem
            // Theres one list for each type of Dataset. Why not just make one? Because it would 
            // be necessary create a list with a generic object (System.Object) that are not supported 
            // to be visualized on Unity Inspector pane
            public DialogClassesOperatorClass()
            {
                ListOfDatasetClasses = new List<DatasetClassListEntry>();


                //The list must be filled manualy, for each dataset model that exists..            
                // Datasets models are declared down below 
                ListOfDatasetClasses.Add(new DatasetClassListEntry("1", typeof(DatasetModel_1)));
                ListOfDatasetClasses.Add(new DatasetClassListEntry("2", typeof(DatasetModel_2)));

            }
        }


        //// At this MainDatasetsToDialogSysDatasets() function, set a foreach for each dataset model declared:
        //// Ex:
        ////    Foreach for dataset model 1 
        ////    foreach (var DS in Datasets.ListDatasetModel_1) {
        ////      act(DS);
        ////    }
        //public void MainDatasetsToDialogSysDatasets( )
        //{

        //    //foreach (var DS in Datasets.ListDatasetModel_1)
        //    //{
        //    //    int k = DialogSystem.Datasets.FindIndex(t => t.Did == DS.__did);
        //    //    //If item found..
        //    //    if (k >= 0)
        //    //    {
        //    //        //Converting to json 
        //    //        String jsonText = JsonConvert.SerializeObject(DS);

        //    //        DialogSystem.Datasets[k].Data = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
        //    //    }
        //    //}

        //    //foreach (var DS in Datasets.ListDatasetModel_2)
        //    //{

        //    //    int k = DialogSystem.Datasets.FindIndex(t => t.Did == DS.__did);
        //    //    //If item found..
        //    //    if (k >= 0)
        //    //    {
        //    //        //Converting to json 
        //    //        String jsonText = JsonConvert.SerializeObject(DS);

        //    //        DialogSystem.Datasets[k].Data = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
        //    //    }
        //    //}


        //    Action<dynamic> act = MainDatasetsToDialogSysDatasetsDo;

        //    // A 'foreach' must be created for each dataset model..

        //    //Foreach for dataset model 1 
        //    foreach (var DS in Datasets.ListDatasetModel_1) {
        //        act(DS);
        //    }

        //    //Foreach for dataset model 2 
        //    foreach (var DS in Datasets.ListDatasetModel_2)
        //    {
        //        act(DS);
        //    }

        //}

        //private void MainDatasetsToDialogSysDatasetsDo(dynamic DS)
        //{
        //    int k = DSysComp.DSys.Datasets.FindIndex(t => t.Did == DS.__did);
        //    //If item found..
        //    if (k >= 0)
        //    {
        //        //Converting to json 
        //        String jsonText = JsonConvert.SerializeObject(DS);

        //        DSysComp.DSys.Datasets[k].Data = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
        //    }
        //}


        //Main Game data Datasets
        //Dont alter, used by MainGameData

        //[Serializable]
        //public class MainDialogSysDatasetClass
        //{
        //    public string Did; //{ get; set; } // Dataset id
        //    public string Mdid; //{ get; set; } // Model Dataset id
        //    //[Serializable]
        //    public System.Object Data; // { get; set; } // Actual data (variables and such)
        //    [TextArea(3, 10)]
        //    public string raw;

        //    public MainDialogSysDatasetClass(string did, string mdid, System.Object data, string stringRaw)
        //    {
        //        Did = did.Trim();
        //        Mdid = mdid.Trim();
        //        Data = data;
        //        raw = stringRaw;
        //    }
        //}

        // Update this class for each dataset model created
        [Serializable]
        public class MainDialogSysDatasetClass
        {
            //public string Did; //{ get; set; } // Dataset id
            //public string Mdid; //{ get; set; } // Model Dataset id
            ////[Serializable]
            //public System.Object Data; // { get; set; } // Actual data (variables and such)
            //[TextArea(3, 10)]
            //public string raw;

            //public MainDialogSysDatasetClass(string did, string mdid, System.Object data, string stringRaw)
            //{
            //    Did = did.Trim();
            //    Mdid = mdid.Trim();
            //    Data = data;
            //    raw = stringRaw;
            //}

            public List<DatasetModel_1> ListDatasetModel_1;
            public List<DatasetModel_2> ListDatasetModel_2;

            public MainDialogSysDatasetClass()
            {
                ListDatasetModel_1 = new List<DatasetModel_1>();
                ListDatasetModel_2 = new List<DatasetModel_2>();
            }
            public bool addElementToDatasetList(string mdid, string jsonText)
            {
                bool ret = false;

                if( mdid == "1")
                {
                    ListDatasetModel_1.Add(JsonConvert.DeserializeObject<DatasetModel_1>(jsonText));
                    ret = true;
                }
                else if( mdid == "2")
                {
                    ListDatasetModel_2.Add(JsonConvert.DeserializeObject<DatasetModel_2>(jsonText));
                    ret = true;
                }
                
                return ret;
            }

        }


        // Declarations of the classs for each dataset model created
        [Serializable]
        public class DatasetModel_1
        {
            //public Data data { get; set; }
            //public class Data
            //{
            //private 
            //The variable __did is mandatory.. 
            public string __did;
            //Declare the vars here..
            //It is preferable to declare numeric variables as float
            public float health; // { get; set; }
            public float magic; //{ get; set; }
            public float location; // { get; set; }
            public float finance; // { get; set; }
            public string born; // { get; set; }
            public float funds; // { get; set; }
            public List<String> partners_id; //{ get; set; }
            public float friendship;


        }

        // Convert a Object to a especific class, using Json conversor
        // This case, converting to DatasetModel01
        //public static DatasetModel_1 convertToClassDatasetModel01(System.Object obj)
        //{
        //    String jsonText = JsonConvert.SerializeObject(obj);
        //    //Type type = Type.GetType("MainGD.MainGameData.DatasetModel01");
        //    //ret = new Array();
        //    return JsonConvert.DeserializeObject<DatasetModel_1>(jsonText);
        //}


        [Serializable]
        public class DatasetModel_2
        {
            //public Data data { get; set; }
            //public class Data
            //{
            public string __did;
            public float friendship_w_player;
            public float reliability; //{ get; set; }
            public float courage; // { get; set; }
            public float shyness; // { get; set; }
            public float happiness; // { get; set; }
            public float anger; //{ get; set; }
            public float strength; //{ get; set; }
            //}
        }

    }
}