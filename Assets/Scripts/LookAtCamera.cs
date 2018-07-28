using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    static public GameObject camera;

    public GameObject vrCamera;
    // Use this for initialization
    void Awake () {
        if (vrCamera != null)
            camera = vrCamera;
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(transform.position + transform.position - camera.transform.position);
	}
}
