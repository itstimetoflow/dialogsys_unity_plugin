using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFar : MonoBehaviour
{
    public RenderTexture renderTexture;
    public float xFromBottom = 0.03f;
    public float yFromBottom = 0.03f;
    public float imgWidth = 0.24f;
    public float imgHeight = 0.12f;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    void OnGUI()
    {
        //GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 600, 600), renderTexture);
        //GUI.DrawTexture(new Rect(xFromBottom, Screen.height - (imgWidth + yFromBottom), imgWidth, imgHeight), renderTexture);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), renderTexture);
    }
}
