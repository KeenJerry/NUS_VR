using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PauseMenu : MonoBehaviour {
    public Transform location;
    public float speed = 20;
    public float maxDis = 20;
    public float minDis = 15;
    private Transform camera;
    private bool open = false;
    private Vector3 dir;

    public Transform cursor;
    public TextMesh volumnValue;

	// Use this for initialization
	void Start () {
        camera = GameObject.Find("VRCamera").transform;
	}

	// Update is called once per frame
	void Update () {
        if (applicationDown())
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

            Vector2 pos = touchValue();
            if (pos.magnitude > 0.5f)
            {
                float degree;
                if (pos.x == 0) {
                    if (pos.y > 0) degree = 90;
                    else degree = 270;
                } else {
                    degree = (float)(System.Math.Atan(pos.y / pos.x) / System.Math.PI * 180);
                    if (degree < 0) degree += 180;
                    if (pos.y < 0) degree += 180;
                }
                cursor.localEulerAngles = new Vector3(0, 0, degree - 90);

                // set value
                int volumn = (int)degree / 3;
                if (volumn < 20) volumn = 20 - volumn;
                else if (volumn <= 30) volumn = 0;
                else if (volumn <= 40) volumn = 100;
                else volumn = 140 - volumn;
                volumnValue.text = "" + volumn;
            }
        }
        else
        {
            if (dis < maxDis)
                transform.position += dir * speed * Time.deltaTime;
            else
                transform.position = new Vector3(0, maxDis, 0);
        }
	}

    bool applicationDown()
    {
        for (int i = 0; i < Player.instance.handCount; i++)
        {
            Hand hand = Player.instance.GetHand(i);
            if (hand.controller != null)
                if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu))
                    return true;
        }
        return false;
    }

    Vector2 touchValue()
    {
        for (int i = 0; i < Player.instance.handCount; i++)
        {
            Hand hand = Player.instance.GetHand(i);
            if (hand.controller != null)
                if (hand.controller.GetTouchDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    Vector2 pos = hand.controller.GetAxis();
                    if (pos != Vector2.zero)
                        return pos;
                }
        }
        return Vector2.zero;
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
