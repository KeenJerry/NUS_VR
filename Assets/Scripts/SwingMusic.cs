using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingMusic : MonoBehaviour {

    // Use this for initialization
    public AudioSource Swing;
    public float SwingSpeed;
    public Vector3 LastPosition;

	void Start () {
        LastPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        SwingSpeed = ((transform.position - LastPosition).magnitude) / Time.deltaTime;
        if(SwingSpeed > 10f)
        {
            Swing.PlayOneShot(Swing.clip);
        }
        LastPosition = transform.position;
	}
}
