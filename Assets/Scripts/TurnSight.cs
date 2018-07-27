using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class TurnSight : MonoBehaviour {

    // Use this for initialization
    public float LeftOffset;
    public float RightOffset;
    public float speed;

    public int GetPadValue()
    {
        for(int i = 0; i < Player.instance.handCount; i++)
        {
            Hand hand = Player.instance.GetHand(i);


            if (hand.controller != null)
            {

                Vector2 value = hand.controller.GetAxis();
                if (value.x > 0.8f)
                    return 1;
                if (value.x < -0.8f)
                    return -1;
            }

        }
        return 0;
    }

	void Start () {
        LeftOffset = RightOffset = 0.0f;
        speed = 90.0f;
	}
	
	// Update is called once per frame
	void Update () {
        int value = GetPadValue();
        transform.localEulerAngles += new Vector3(0.0f, value * speed * Time.deltaTime, 0.0f);
	}
}
