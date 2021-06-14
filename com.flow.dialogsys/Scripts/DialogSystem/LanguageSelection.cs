using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using DialogSysComp;

public class LanguageSelection : MonoBehaviour
{

    public Dropdown languageDropdown;
    public string pathToTranslationFile = "Resources\\languagesfile.csv";
    [SerializeField]
    public Dictionary<string, string> DialogsLanguageTranslations;
    public DSysComponent DSysComp;
    [Tooltip("In a multithreaded enviroment, allow be executed at the same time that another operations in the game")]
    public bool runAsynchronously = true;
    [HideInInspector]
    public int idLanguageSelected = 0;
    public Text textBoxForTest;
    public string textForTextBoxTest = "";
    

    // Start is called before the first frame update
    void Start()
    {
        loadLanguages();

        languageDropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(languageDropdown);
        });

        
    }

    private void Update()
    {
        if (textBoxForTest != null)
        {
            //If exist test text for the TextBoxTest, put it there..
            if (textForTextBoxTest.Trim() != "")
            {
                textBoxForTest.text = textForTextBoxTest;
                textForTextBoxTest = "";
            }
        }
    }
    //Unity Debug is conclicting with System.Diagnostics.Debug..
    //void DebugTest(object message)
    //{
    //    UnityEngine.Debug.Log(message);
    //}


    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(Dropdown change)
    {
        //m_Text.text = "New Value : " + change.value;
        UnityEngine.Debug.Log("Language changed to: " + change.value);
        idLanguageSelected = change.value;

        Action act = () =>
        {
            loadTranslations();
            UnityEngine.Debug.Log("Finish load translations!");
        };

        // Create new stopwatch.
        Stopwatch stopwatch = new Stopwatch();
        // Begin timing.
        stopwatch.Start();

        Action actStopAndShowTimer = () =>
        {
            // Stop timing.
            stopwatch.Stop();
            // Write result.
            UnityEngine.Debug.Log("Time elapsed: " + stopwatch.Elapsed.TotalSeconds + " secs");
        };

        if (runAsynchronously)
        {
            //Running asyncronoulsy
            UnityEngine.Debug.Log("Running Asyncronoulsy..");
            Task.Run(async () =>
            {
                await Task.Run(act);
                actStopAndShowTimer();
            });
        }
        else
        {
            UnityEngine.Debug.Log("Running Syncronoulsy..");
            act();
            actStopAndShowTimer();
        }


    }


    void loadLanguages()
    {
        using (var reader = new StreamReader(pathToTranslationFile, System.Text.Encoding.UTF8))
        {
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();

            // Getting the languages avaliable..

            int lineNumber = 0;
            List<string> options = new List<string>();
            languageDropdown.ClearOptions();
            //while (!reader.EndOfStream)
            while (lineNumber <= 1)
            {
                var line = reader.ReadLine();
                //Getting the second line, that has the names..
                if (lineNumber == 1)
                {
                    var values = line.Split(';');

                    foreach (var item in values)
                    {
                        //UnityEngine.Debug.Log("Item: " + item);
                        options.Add(item);
                    }

                    //listA.Add(values[0]);
                    //listB.Add(values[1]);
                    languageDropdown.AddOptions(options);
                    //Console.WriteLine("Engl: >{0}< | Port: >{1}<", values[0], values[1]);
                }
                lineNumber++;
            }

        }
    }


    void loadTranslations()
    {
        // Now getting all the translations.. 
        //Reseting the streamer position to begining..

        DialogsLanguageTranslations = new Dictionary<string, string>();
        string textForTest = "";

        //using (var reader = new StreamReader(@"C:\www\dialogsystem\DialogSystem\saved_files\text_to_translate02.csv", System.Text.Encoding.UTF8))
        using (var reader = new StreamReader( pathToTranslationFile, System.Text.Encoding.UTF8 ))
        {

            List<string> options = new List<string>();
            int lineNumber = 0;
            while (!reader.EndOfStream)
            //while (lineNumber <= 1)
            {
                var line = reader.ReadLine();
                //Getting the second line, that has the names..
                if ( lineNumber >= 2 )
                {
                    var values = line.Split(';');


                    //Adding the first language and the selected language..
                    DialogsLanguageTranslations.Add(values[0], values[idLanguageSelected]);

                    if (textBoxForTest != null)
                    {
                        textForTest += "Line: >" + values[0] + "<  |  Translation: >" + values[1] + "<\r\n";
                    }

                    //listA.Add(values[0]);
                    //listB.Add(values[1]);
                    //Console.WriteLine("Engl: >{0}< | Port: >{1}<", values[0], values[1]);
                }
                else
                {
                    lineNumber++;
                }
                
            }
        }

       
        //foreach (KeyValuePair<string, string> item in DialogsLanguageTranslations)
        //{
        //    textForTest += "Line: >" + item.Key + "<  |  Translation: >" + item.Value + "<\r\n";
        //}

        //textBoxForTest.text = textForTest;
        textForTextBoxTest = textForTest;
        //UnityEngine.Debug.Log("Text: \r\n" + textForTest);
        if (DSysComp != null && DSysComp.ready)
        {
            UnityEngine.Debug.Log("Updating dictionary of the DSys..");
            DSysComp.DSys.TranslationDictionary = DialogsLanguageTranslations;
            DSysComp.DSys.selectedLanguageForTranslation = idLanguageSelected;

            //Translating 'Talk' buton on the DialogSys component..
            //ButtonBehavTalk btnBehavTalk = DSysComp.ButtonTalkGO.GetComponent<ButtonBehavTalk>();
            UnityEngine.Debug.Log("btnTalkText text: " + DSysComp.btnTalkLabel);
            DSysComp.btnLabelChanged = true;
        }

        
    }
}