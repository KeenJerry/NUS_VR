using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FruitButton : MonoBehaviour {
    public int iconID = 0;
    public float scale = 1;
    public float speed = 180;
    public float maxPieceTime = 1;
    public Vector3 iconOffset = new Vector3(0, 0, 0);
    public UnityEvent onTrigger;

    private GameObject icon;
    private GameObject[] piece;
    private float[] pieceTime;
    private Vector3[] pieceVelocity;
    private Vector3 geo = new Vector3(0, -9.8f, 0);
	// Use this for initialization
	void Start () {
        if (iconID < 0 || iconID >= FoodSet.foods.Length) iconID = 0;
        createIcon();
        resetIcon();
	}
	
	// Update is called once per frame
	void Update () {
        if (icon.activeSelf)
            icon.transform.localEulerAngles += new Vector3(0, speed * Time.deltaTime, 0);

        for (int i = 0; i < 3; i++)
            if (piece[i].activeSelf)
            {
                piece[i].transform.localPosition += pieceVelocity[i] * Time.deltaTime;
                pieceVelocity[i] += geo * Time.deltaTime;
                pieceTime[i] += Time.deltaTime;
                if (pieceTime[i] > maxPieceTime) piece[i].SetActive(false);
            }
	}

    public void trigger()
    {
        icon.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            piece[i].transform.position = icon.transform.position;
            pieceVelocity[i] = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            pieceTime[i] = 0;
            piece[i].SetActive(true);
        }
        onTrigger.Invoke();
    }

    private void createIcon()
    {
        icon = Instantiate(FoodSet.foods[iconID].obj, transform);
        icon.transform.localPosition = iconOffset;
        icon.transform.localScale = new Vector3(scale, scale, scale);
        icon.AddComponent<ButtonIcon>();
        icon.AddComponent<BoxCollider>();
        icon.GetComponent<BoxCollider>().isTrigger = true;

        piece = new GameObject[3];
        pieceVelocity = new Vector3[3];
        pieceTime = new float[3];
        for (int i = 0; i < 3; i++)
        {
            piece[i] = Instantiate(FoodSet.foods[iconID].piece, transform);
            piece[i].tag = "Untagged";
            piece[i].transform.localScale = new Vector3(scale * 0.7f, scale * 0.7f, scale * 0.7f);
        }
    }

    public void resetIcon()
    {
        icon.SetActive(true);
        for (int i = 0; i < 3; i++) piece[i].SetActive(false);
    }
}
