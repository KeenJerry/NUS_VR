using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]

public class BGMManager : MonoBehaviour {

    // Use this for initialization
    public AudioSource BGMNotStart;
    public AudioSource BGMStart;
    public AudioSource BGMWin;
    public AudioSource BGMLose;

    public GameObject Trajetory;
    public LaunchFood LaunchFoodComponent;

	void Start () {
        LaunchFoodComponent = Trajetory.GetComponent<LaunchFood>();
 
	}
	
	// Update is called once per frame
	void Update () {
        if(LaunchFoodComponent)
            switch (LaunchFoodComponent.status)
            {
                case LaunchFood.Status.FREE:
                    {
                        BGMStart.Stop();
                        if (BGMNotStart)
                        {
                            if (!BGMNotStart.isPlaying)
                            {
                                BGMNotStart.Play();
                            }
                        }
                        break;
                    }
                    
                case LaunchFood.Status.LAUNCH:
                    {
                        if (BGMStart)
                        {
                            BGMNotStart.Stop();
                            if (!BGMStart.isPlaying)
                            {
                                BGMStart.Play();
                            }
                        }
                            break;
                    }
                    
                case LaunchFood.Status.LOSE:
                    {
                        if (BGMLose) BGMLose.Play();
                        break;
                    }
                    
                case LaunchFood.Status.WIN:
                    {
                        if (BGMWin) BGMWin.Play();
                        break;
                    }
                   
            }
	}
}
