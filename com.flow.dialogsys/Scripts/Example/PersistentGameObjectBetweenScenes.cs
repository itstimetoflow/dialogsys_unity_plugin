using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameObjectBetweenScenes : MonoBehaviour
{

    private void Awake()
    {
        //If already has this object, dont duplicate it
        var listOfPersistentGOs = FindObjectsOfType<PersistentGameObjectBetweenScenes>();
        if (listOfPersistentGOs.Length > 0)
        {
            PersistentGameObjectBetweenScenes[] k = Array.FindAll(listOfPersistentGOs, element => element.name == gameObject.name);
            //int k = listOfPersistentGOs
            Debug.Log("GOs found..");
            Debug.Log(k.Length);
            if (k.Length > 1)
            {
                Destroy(gameObject); //Destroy this..
                return;
            }
        }

        //if ( GameObject.Find(gameObject.name) != null)
        //{
        //    Destroy(gameObject); //Destroy this..
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
