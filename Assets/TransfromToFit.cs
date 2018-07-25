using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransfromToFit : MonoBehaviour {

    // Use this for initialization
    public Vector3 LocalRotation;
    public Vector3 LocalPosition;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = LocalPosition;
        transform.localEulerAngles = LocalRotation;
	}
}
