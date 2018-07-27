using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FruitButton : MonoBehaviour {
    public int iconID = 0;
    public float scale = 1;
    public float speed = 180;
    public Vector3 iconOffset = new Vector3(0, 0, 0);
    public UnityEvent onTrigger;

    private GameObject icon;
	// Use this for initialization
	void Start () {
        if (iconID < 0 || iconID >= FoodSet.foods.Length) iconID = 0;
        icon = Instantiate(FoodSet.foods[iconID].obj, transform);
        icon.transform.localPosition = iconOffset;
        icon.transform.localScale = new Vector3(scale, scale, scale);
        icon.AddComponent<ButtonIcon>();
        icon.AddComponent<BoxCollider>();
        icon.GetComponent<BoxCollider>().isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
        icon.transform.localEulerAngles += new Vector3(0, speed * Time.deltaTime, 0);
	}

    public void trigger()
    {
        onTrigger.Invoke();
    }
}
