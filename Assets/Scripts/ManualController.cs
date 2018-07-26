using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualController : MonoBehaviour {
    public TextMesh title;
    public TextMesh food;
    public TextMesh num;
    public TextMesh page;

    public Transform foods;
    public Transform texts;
    public float showWidth = 10;
    private Transform camera;

    public LaunchFood launcher;

    private int pageIndex = -1;
    private int newPageIndex = -1;
    public float switchInterval = 0.5f;
    private bool canSwitch = false;
    private float bufferTime = 0;
    private bool isSwitched = true;

    private FoodSet.Manual[] manuals;

	// Use this for initialization
	void Start () {
        if (switchInterval <= 0) switchInterval = 0.5f;
        bufferTime = switchInterval - 0.05f;
        manuals = FoodSet.manuals;
        camera = GameObject.Find("VRCamera").transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (!canSwitch)
        {
            bufferTime += Time.deltaTime;
            if (bufferTime >= switchInterval)
            {
                bufferTime = 0;
                canSwitch = true;
                if (pageIndex < 0 || pageIndex >= manuals.Length)
                    setPage(0);
            }

            if (!isSwitched)
                if (bufferTime >= switchInterval / 2)
                {
                    setPage(newPageIndex);
                    isSwitched = true;
                }

            float opacity = System.Math.Abs(bufferTime / switchInterval * 2 - 1);
            SetOpacity(title, opacity);
            SetOpacity(food, opacity);
            SetOpacity(num, opacity);
            SetOpacity(page, opacity);
        }
	}

    private void SetOpacity(TextMesh text, float opacity)
    {
        Color color = text.color;
        color.a = opacity;
        text.color = color;
    }

    public void previewPage()
    {
        if (canSwitch)
            if (pageIndex > 0)
            {
                canSwitch = false;
                bufferTime = 0;
                newPageIndex = pageIndex - 1;
                isSwitched = false;
            }
    }

    public void nextPage()
    {
        if (canSwitch)
            if (pageIndex < manuals.Length - 1)
            {
                canSwitch = false;
                bufferTime = 0;
                newPageIndex = pageIndex + 1;
                isSwitched = false;
            }
    }

    public void chooseManual()
    {
        if (canSwitch)
            if (pageIndex >= 0 && pageIndex < manuals.Length)
            {
                launcher.setManual(pageIndex);
            }
    }

    private void setPage(int newPage)
    {
        if (newPage >= 0 && newPage < manuals.Length)
        {
            for (int i = 0; i < foods.childCount; i++)
            {
                GameObject child = foods.GetChild(i).gameObject;
                Destroy(child);
            }

            for (int i = 0; i < texts.childCount; i++)
            {
                GameObject child = texts.GetChild(i).gameObject;
                Destroy(child);
            }

            pageIndex = newPage;
            title.text = manuals[pageIndex].name;

            string detailStr = "";
            string numStr = "";
            for (int i = 0; i < manuals[pageIndex].foods.Length; i++)
            {
                Vector3 offset = new Vector3(0, 0, -showWidth/2 + (i+1)*showWidth/(manuals[pageIndex].foods.Length+1));
                GameObject tempFood;
                tempFood = Instantiate(manuals[pageIndex].foods[i].obj, foods);
                tempFood.transform.position = foods.position + offset;
                tempFood.transform.localScale = new Vector3(3, 3, 3);
                GameObject tempText;
                tempText = new GameObject();
                tempText.transform.parent = texts;
                tempText.transform.position = texts.position + offset;
                tempText.transform.LookAt(tempText.transform.position + tempText.transform.position - camera.position);
                tempText.AddComponent<TextMesh>();
                tempText.GetComponent<TextMesh>().text = "X" + manuals[pageIndex].nums[i];
                tempText.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
                tempText.GetComponent<TextMesh>().characterSize = 0.1f;
                tempText.GetComponent<TextMesh>().fontSize = 80;

                detailStr += manuals[pageIndex].foods[i].name + "\n";
                numStr += "X" + manuals[pageIndex].nums[i] + "\n";
            }
                 
            food.text = detailStr;
            num.text = numStr;

            page.text = (pageIndex + 1) + "/" + manuals.Length;
        }
    }
}
