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
        if (BGMNotStart != null) BGMNotStart.volume = PauseMenu.volumn;
        if (BGMStart != null) BGMStart.volume = PauseMenu.volumn;
        if (BGMWin != null) BGMWin.volume = PauseMenu.volumn;
        if (BGMLose != null) BGMLose.volume = PauseMenu.volumn;
        if (LaunchFoodComponent)
            switch (LaunchFoodComponent.status)
            {
                case LaunchFood.Status.FREE:
                    {
                        BGMStart.Stop();
                        BGMWin.Stop();
                        BGMLose.Stop();
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
                        if (BGMLose)
                        {
                            BGMStart.Stop();
                            if (!BGMLose.isPlaying)
                                BGMLose.Play();
                        }
                        break;
                    }
                    
                case LaunchFood.Status.WIN:
                    {
                        if (BGMWin)
                        {
                            BGMStart.Stop();
                            if (!BGMWin.isPlaying)
                                BGMWin.Play();
                        }
                        break;
                    }
                   
            }
	}
}
