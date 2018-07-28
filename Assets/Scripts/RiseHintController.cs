using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseHintController : MonoBehaviour {
    public TextMesh[] hints;
    private float[] hintsInitPos;
    public float riseSpeed = 3;
    public float riseLength = 1;
    private Vector3 riseVector = new Vector3(0, 1, 0);

    public enum HintType
    {
        ERROR, NORMAL, GOOD
    }
    public Color error = new Color(1, 0, 0);
    public Color normal = new Color(1, 1, 0);
    public Color good = new Color(0, 0, 1);


    private void Start()
    {
        hintsInitPos = new float[hints.Length];
    }

    // Update is called once per frame
    void Update () {
		for (int i = 0; i < hints.Length; i++)
            if (hints[i].gameObject.activeSelf)
            {
                rise(hints[i], Time.deltaTime);
                if (hints[i].gameObject.transform.position.y - hintsInitPos[i] > riseLength)
                {
                    Debug.Log(hints[i].gameObject.transform.position.y + " " + hintsInitPos[i] + " " + riseLength);
                    hints[i].gameObject.SetActive(false);
                }
            }
                
	}

    public void hint(string content, HintType type, Vector3 pos)
    {
        Color color = new Color(1, 1, 1, 1);
        switch(type)
        {
            case HintType.ERROR:
                color = error;
                break;
            case HintType.NORMAL:
                color = normal;
                break;
            case HintType.GOOD:
                color = good;
                break;
        }

        int i;
        for (i = 0; i < hints.Length; i++)
            if (!hints[i].gameObject.activeSelf)
                break;

        if (i != hints.Length)
        {
            hintsInitPos[i] = pos.y + 1;
            hints[i].gameObject.transform.position = new Vector3(pos.x, pos.y + 1, pos.z);
            hints[i].text = content;
            hints[i].color = color;
            hints[i].gameObject.SetActive(true);
        }
    }

    private void rise(TextMesh obj, float time)
    {
        obj.gameObject.transform.position += time * riseSpeed * riseVector;
    }
}
