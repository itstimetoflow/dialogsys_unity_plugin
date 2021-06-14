using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace UpdateCaller
{

    public class UpdateCaller : MonoBehaviour
    {

        private static UpdateCaller instance;

        public static void AddUpdateCallback(Action updateMethod)
        {
            if (instance == null)
            {
                instance = new GameObject("[Update Caller]").AddComponent<UpdateCaller>();
            }
            instance.updateCallback += updateMethod;
        }

        private Action updateCallback;

        private void Update()
        {
            if (updateCallback != null)
            {
                updateCallback();
            }
        }


    }




}