using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchFood : MonoBehaviour {
    private FoodSet.Manual chosen = null;
    private enum Status
    {
        FREE, PREPARE, LAUNCH, PAUSE, END
    };
    private Status status = Status.FREE;

    public Transform foods;
    public Transform pieces;

    public int prepareCount = 3;
    public float launchInterval = 1f;
    private float bufferTime;
    private int second;

    private GameObject[] foodPool;
    private GameObject[] piecePool;
    private Vector3[] pieceVelocity;
    private Vector3 geo = new Vector3(0, -9.8f, 0);
    private int poolCap = 5;
    private int pieceEach = 3;

    public float moveSpeed = 10;
    public float rotateSpeed = 180;
    public float trajectoryLength = 64;
    public float potHeight = 0;

    // Use this for initialization
    void Start () {
        if (prepareCount <= 0) prepareCount = 3;
        if (launchInterval <= 0) launchInterval = 0.5f;
        foodPool = new GameObject[FoodSet.foods.Length * poolCap];
        for (int i = 0; i < FoodSet.foods.Length; i++)
            for (int j = 0; j < poolCap; j++)
            {
                int k = i * poolCap + j;
                foodPool[k] = Instantiate(FoodSet.foods[i].obj, foods);
                foodPool[k].SetActive(false);
                foodPool[k].transform.localScale = new Vector3(5, 5, 5);
            }
        piecePool = new GameObject[FoodSet.foods.Length * pieceEach];
        pieceVelocity = new Vector3[FoodSet.foods.Length * pieceEach];
        for (int i = 0; i < FoodSet.foods.Length; i++)
            for (int j = 0; j < pieceEach; j++)
            {
                int k = i * pieceEach + j;
                piecePool[k] = Instantiate(FoodSet.foods[i].piece, pieces);
                piecePool[k].SetActive(false);
                piecePool[k].transform.localScale = new Vector3(3, 3, 3);
            }
    }
	
	// Update is called once per frame
	void Update () {
		switch(status)
        {
            case Status.FREE:
                bufferTime = 0;
                second = 0;
                break;
            case Status.PREPARE:
                {
                    int originsecond = second;
                    bufferTime += Time.deltaTime;
                    second = (int)bufferTime;
                    if (originsecond != second)
                    {
                        Debug.Log(second);
                        // show count
                    }
                    if (second == prepareCount)
                    {
                        status = Status.LAUNCH;
                        bufferTime = 0;
                        second = 0;
                    }
                }
                break;
            case Status.LAUNCH:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    status = Status.PAUSE;
                    // show menu
                }
                else
                {
                    bufferTime += Time.deltaTime;
                    int originSecond = second;
                    second = (int)(bufferTime / launchInterval);
                    if (originSecond != second) launchFood();

                    foreach (GameObject food in foodPool)
                        if (food.activeSelf)
                        {
                            food.transform.position += new Vector3(0, 0, Time.deltaTime * moveSpeed);
                            food.transform.localEulerAngles += new Vector3(0, Time.deltaTime * rotateSpeed, 0);
                            if (food.transform.localPosition.z > trajectoryLength)
                            {
                                food.SetActive(false);
                                // some punish
                            }
                        }
                    for (int i = 0; i < piecePool.Length; i++)
                        if (piecePool[i].activeSelf)
                        {
                            GameObject piece = piecePool[i];
                            piece.transform.position += pieceVelocity[i] * Time.deltaTime;
                            piece.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
                            pieceVelocity[i] += geo * Time.deltaTime;
                            if (piece.transform.position.y < potHeight)
                                piece.SetActive(false);
                        }

                    // test
                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        float front = 45;
                        int index = -1;
                        for (int i = 0; i < foodPool.Length; i++)
                            if (foodPool[i].activeSelf)
                                if (foodPool[i].transform.position.z > front)
                                {
                                    front = foodPool[i].transform.position.z;
                                    index = i;
                                }
                        if (index != -1) cutFood(foodPool[index]);
                    }
                }
                break;
            case Status.END:
                foreach (GameObject food in foodPool)
                    food.SetActive(false);
                status = Status.FREE;
                break;
            case Status.PAUSE:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    status = Status.LAUNCH;
                    // hide menu
                }
                break;
        }
	}

    private void launchFood()
    {
        int[] freeFoodIndex = new int[foodPool.Length];
        int freeFoodCount = 0;
        for (int i = 0; i < foodPool.Length; i++)
            if (!foodPool[i].activeSelf)
                freeFoodIndex[freeFoodCount++] = i;

        int choice = freeFoodIndex[Random.Range(0, freeFoodCount)];
        foodPool[choice].transform.localPosition = new Vector3(Random.Range(-3, 4), Random.Range(-3, 4), 0);
        foodPool[choice].SetActive(true);
    }

    public void setManual(int manualIndex)
    {
        if (status == Status.FREE)
            if (manualIndex >= 0 && manualIndex < FoodSet.manuals.Length)
            {
                chosen = FoodSet.manuals[manualIndex];
                status = Status.PREPARE;
            }
    }

    public void cutFood(GameObject food)
    {
        for (int i = 0; i < foodPool.Length; i++)
            if (foodPool[i] == food)
            {
                food.SetActive(false);

                // show piece
                Vector3 pos = food.transform.position;
                int index = i / poolCap;
                for (int j = 0; j < pieceEach; j++)
                {
                    piecePool[index * pieceEach + j].transform.position = new Vector3(pos.x, pos.y, pos.z);
                    pieceVelocity[index * pieceEach + j] = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
                    piecePool[index * pieceEach + j].SetActive(true);
                }
                break;
            }
    }
}
