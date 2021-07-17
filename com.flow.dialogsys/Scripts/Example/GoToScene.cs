using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    public string sceneToLoad;
    public string sceneToUnload;
    public GameObject MessageGoToOtherScene;
    private AsyncOperation asyncOperation;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("On Start() function..");
        Debug.Log(MessageGoToOtherScene);
        if (MessageGoToOtherScene == null)
        {
            Debug.Log("Obj is null");
            MessageGoToOtherScene = GameObject.Find("MessageGoToOtherScene");
        }
        MessageGoToOtherScene.SetActive(false);
    }

    private void Awake()
    {
        //if (MessageGoToOtherScene == null)
        //{
        //    MessageGoToOtherScene = GameObject.Find("MessageGoToOtherScene");
        //}
        //MessageGoToOtherScene.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("MainGameData") == null)
        {
            Debug.LogWarning("MainGameData not found");
        }

        //if (asyncOperation.)
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (MessageGoToOtherScene == null)
        //{
        //    MessageGoToOtherScene = GameObject.Find("MessageGoToOtherScene");
        //}
        if (other.name == "Player")
        {
            MessageGoToOtherScene.SetActive(true);
        }
        //MessageGoToOtherScene.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.name == "Player")
        {
            
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                //MessageGoToOtherScene.SetActive(false);
                Debug.Log("Loading other scene..");
                asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
                asyncOperation.completed += (asyncOper) =>
                {
                    //Do stuff here with returned value                 asyncOperation.completed += (asyncOper) =>
                    SceneManager.UnloadSceneAsync(sceneToUnload);

                };
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            MessageGoToOtherScene.SetActive(false);
        }
    }
}
