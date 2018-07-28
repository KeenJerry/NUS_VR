using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseHintController : MonoBehaviour {
    public TextMesh[] hints;
    private float[] hintsInitPos;
    public float riseSpeed = 3;
    public float riseLength = 1;
    private Vector3 riseVector = new Vector3(0, 1, 0);
	
	// Update is called once per frame
	void Update () {
        hintsInitPos = new float[hints.Length];
		for (int i = 0; i < hints.Length; i++)
            if (hints[i].gameObject.activeSelf)
            {
                rise(hints[i], Time.deltaTime);
                if (hints[i].gameObject.transform.position.y - hintsInitPos[i] > riseLength) hints[i].gameObject.SetActive(false);
            }
                
	}

    public void hint(string content, Color color, Vector3 pos)
    {
        int i;
        for (i = 0; i < hints.Length; i++)
            if (!hints[i].gameObject.activeSelf)
                break;

        hintsInitPos[i] = pos.y + 1;
        hints[i].gameObject.transform.position = new Vector3(pos.x, pos.y+1, pos.z);
        hints[i].text = content;
        hints[i].color = color;
    }

    private void rise(TextMesh obj, float time)
    {
        obj.gameObject.transform.position += time * riseSpeed * riseVector;
    }
}
