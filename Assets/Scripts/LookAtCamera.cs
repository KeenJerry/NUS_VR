using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    static public GameObject camera;

    // Use this for initialization
    void Start () {
        string[] possibleCameraNameList = new string[] { "VRCamera", "VRCamera (eye)", "Camera", "MainCamera" };
        foreach (string cameraName in possibleCameraNameList)
        {
            GameObject temp = GameObject.Find(cameraName);
            if (temp != null)
                camera = GameObject.Find("VRCamera");
        }
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(transform.position + transform.position - camera.transform.position);
	}
}
