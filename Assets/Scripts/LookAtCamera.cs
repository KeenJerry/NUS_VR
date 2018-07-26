using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    private GameObject camera;

    // Use this for initialization
    void Start () {
        camera = GameObject.Find("VRCamera");
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(transform.position + transform.position - camera.transform.position);
	}
}
