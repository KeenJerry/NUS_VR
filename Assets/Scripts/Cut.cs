using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut : MonoBehaviour {

    // Use this for initialization
    private GameObject Trajectory;
    public AudioSource CutAudio;
    public AudioSource BombAudio;
	void Start () {
        Trajectory = GameObject.Find("Trajectory");
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("food"))
        {   
            if(Trajectory)
                Trajectory.GetComponent<LaunchFood>().cutFood(other.gameObject);
            // music
            if(CutAudio)
                CutAudio.Play();
        }
        if (other.CompareTag("bomb"))
        {
            if (Trajectory)
                Trajectory.GetComponent<LaunchFood>().cutBomb(other.gameObject);
            // music
            if (BombAudio)
                BombAudio.Play();
        }
    }
}
