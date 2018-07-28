using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShowRegionInfo : MonoBehaviour {

    // Use this for initialization
    public List<GameObject> Info;
    public List<GameObject> Block;
    

    public void ShowInfo()
    {
        
        for(int i = 0; i < Player.instance.handCount; i++)
        {
            Hand hand = Player.instance.GetHand(i);

            if(hand.controller != null)
            {
                if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                   foreach(GameObject info in Info)
                    {
                        info.SetActive(true);
                    }
                    return;
                }
                if (hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    foreach (GameObject info in Info)
                    {
                        info.SetActive(false);
                    }
                    return;
                }
            }
        }

        for (int i = 0; i < Player.instance.handCount; i++)
        {
             Hand hand = Player.instance.GetHand(i);

            if(hand.controller != null)
            {
                if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                   foreach(GameObject block in Block)
                    {
                        
                    }
                    return;
                }
                if (hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    foreach (GameObject info in Info)
                    {
                        info.SetActive(false);
                    }
                    return;
                }
            }
        }
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ShowInfo();
	}
}
