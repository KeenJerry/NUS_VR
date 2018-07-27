using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMenu : MonoBehaviour {
    public SpriteRenderer[] imgs;
    public TextMesh[] texts;
    public MeshRenderer[] objs;

    public float minDis = 15;
    public float maxDis = 20;
    
    private float lastDis;

	// Use this for initialization
	void Start () {
        lastDis = (minDis + maxDis) / 2;
    }
	
	// Update is called once per frame
	void Update () {
        float dis = (transform.position - LookAtCamera.camera.transform.position).magnitude;
		if (lastDis >= minDis && lastDis <= maxDis)
        {
            float opacity;
            if (dis <= minDis) opacity = 1;
            else if (dis >= maxDis) opacity = 0;
            else opacity = (maxDis - dis) / (maxDis - minDis);
            
            foreach(SpriteRenderer img in imgs)
            {
                Color color = img.color;
                color.a = opacity;
                img.color = color;
            }
            foreach(TextMesh text in texts)
            {
                Color color = text.color;
                color.a = opacity;
                text.color = color;
            }
            foreach(MeshRenderer obj in objs)
            {
                Color color = obj.material.color;
                color.a = opacity;
                obj.material.color = color;
            }
        }
        lastDis = dis;
    }
}
