using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut : MonoBehaviour {

    // Use this for initialization
    private GameObject Trajectory;
	void Start () {
        Trajectory = GameObject.Find("Trajectory");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("food"))
        {   
            if(Trajectory)
                Trajectory.GetComponent<LaunchFood>().cutFood(other.gameObject);
        }
    }
}
