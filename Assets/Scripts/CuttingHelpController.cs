using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingHelpController : MonoBehaviour {
    static public bool show = false;
    public float speed = 40;
    private float cuttingHelpHide = -93;
    private float cuttingHelpShow = -75;
    private Vector3 dir = new Vector3(1, 0, 0);
	
	// Update is called once per frame
	void Update () {
		if (show)
        {
            if (transform.position.x >= cuttingHelpShow) return;
            Vector3 temp = transform.position;
            temp += dir * speed * Time.deltaTime;
            if (temp.x > cuttingHelpShow) temp.x = cuttingHelpShow;
            transform.position = temp;
        }
        else
        {
            if (transform.position.x <= cuttingHelpHide) return;
            Vector3 temp = transform.position;
            temp -= dir * speed * Time.deltaTime;
            if (temp.x < cuttingHelpHide) temp.x = cuttingHelpHide;
            transform.position = temp;
        }
	}
}
