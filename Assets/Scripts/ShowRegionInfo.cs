using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShowRegionInfo : MonoBehaviour {

    // Use this for initialization
    GameObject Info;

    public void ShowInfo()
    {
        for(int i = 0; i < Player.instance.handCount; i++)
        {
            Hand hand = Player.instance.GetHand(i);

            if(hand.controller != null)
            {
                if (hand.controller.GetTouchDown(Valve.VR.EVRButtonId.k_EButton_DPad_Down))
                {
                    Info.SetActive(true);
                    return;
                }
            }
        }
        Info.SetActive(false);
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ShowInfo();
	}
}
