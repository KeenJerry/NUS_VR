using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public Transform location;
    public float speed = 20;
    public float maxDis = 20;
    public float minDis = 15;
    private Transform camera;
    private bool open = false;
    private Vector3 dir;

	// Use this for initialization
	void Start () {
        camera = GameObject.Find("VRCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(open);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (open)
            {
                open = false;
            }
            else
            {
                open = true;
                dir = (location.position - camera.position).normalized;
                transform.position = dir * maxDis + location.position;
            }
        }
            

        float dis = (transform.position - camera.position).magnitude;
        if (open)
        {
            if (dis > minDis)
                transform.position -= dir * speed * Time.deltaTime;
        }
        else
        {
            if (dis < maxDis)
                transform.position += dir * speed * Time.deltaTime;
        }
	}
}
