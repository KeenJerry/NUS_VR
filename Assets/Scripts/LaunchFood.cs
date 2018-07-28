using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LaunchFood : MonoBehaviour {
    private FoodSet.Manual chosen = null;
    private int manualIndex;
    private int[] statisticsCount = null;
    private TextMesh[] statisticsCountText = null;
    private int statisticsMissCount;
    private int statisticsErrorCount;
    private int statisticsScoreCount;
    private int statisticsBombCount;
    private int scoreMultiple = 1;
    private int levelUpCount = 0;
    public enum Status
    {
        FREE, WAITING, PREPARE, LAUNCH, PAUSE, END, WIN, LOSE
    };
    public Status status = Status.FREE;

    public Transform foods;
    public Transform pieces;
    public Transform statisticsFoods;
    private GameObject[] statisticsFoodsObj = null;
    public Transform statisticsText;
    private GameObject[] statisticsTextsObj = null;
    public TextMesh statisticsMiss;
    public TextMesh statisticsError;
    public TextMesh statisticsScore;
    public TextMesh statisticsBomb;

    public int prepareCount = 3;
    public float launchInterval = 1f;
    private float bufferTime;
    private int second;

    private GameObject[] foodPool;
    private GameObject[] bombPool;
    private GameObject[] piecePool;
    private Vector3[] pieceVelocity;
    private Vector3 geo = new Vector3(0, -9.8f, 0);
    private int poolCap = 5;
    private int bombNum = 10;
    private int pieceEach = 3;

    public GameObject startButton;
    public TextMesh startButtonText;

    public RiseHintController riseHint;

    public float statisticsHeight = 11;
    public float moveSpeed = 10;
    public float rotateSpeed = 180;
    public float trajectoryLength = 64;
    public float potHeight = 0;
    public int maxBomb = 3;

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
                piecePool[k].tag = "Untagged";
                piecePool[k].SetActive(false);
                piecePool[k].transform.localScale = new Vector3(3, 3, 3);
            }
        bombPool = new GameObject[bombNum];
        for (int i = 0; i < bombNum; i++)
        {
            bombPool[i] = Instantiate(FoodSet.bomb, foods);
            bombPool[i].SetActive(false);
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
            case Status.WAITING:
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
                        startButtonText.text = "" + (3 - second);
                    }
                    if (second == prepareCount)
                    {
                        status = Status.LAUNCH;
                        startButton.SetActive(false);
                        bufferTime = 0;
                        second = 0;
                    }
                }
                break;
            case Status.LAUNCH:
                if (applicationDown())
                {
                    status = Status.PAUSE;
                    CuttingHelpController.show = true;
                }
                else
                {
                    bufferTime += Time.deltaTime;
                    int originSecond = second;
                    second = (int)(bufferTime / launchInterval);
                    if (originSecond != second) launchFood();

                    for (int i = 0; i < foodPool.Length; i++)
                    {
                        GameObject food = foodPool[i];
                        if (food.activeSelf)
                        {
                            food.transform.position += new Vector3(0, 0, Time.deltaTime * moveSpeed);
                            food.transform.localEulerAngles += new Vector3(0, Time.deltaTime * rotateSpeed, 0);
                            if (food.transform.localPosition.z > trajectoryLength)
                            {
                                food.SetActive(false);
                                // some punish
                                int index = inManual(i / poolCap);
                                if (index != -1 && statisticsCount[index] < chosen.nums[index])
                                {
                                    statisticsMissCount++;
                                    if (statisticsScoreCount > 0)
                                        statisticsScoreCount -= 10;
                                    scoreMultiple = 1;
                                    levelUpCount = 0;
                                    ShowStatisticsInfo();
                                }
                            }
                        }
                    }
                    for (int i = 0; i < bombNum; i++)
                    {
                        GameObject bomb = bombPool[i];
                        if (bomb.activeSelf)
                        {
                            bomb.transform.position += new Vector3(0, 0, Time.deltaTime * moveSpeed);
                            bomb.transform.localEulerAngles += new Vector3(0, Time.deltaTime * rotateSpeed, 0);
                            if (bomb.transform.localPosition.z > trajectoryLength)
                                bomb.SetActive(false);
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
                }
                break;
            case Status.WIN:
                foreach (GameObject food in foodPool)
                    food.SetActive(false);
                foreach (GameObject bomb in bombPool)
                    bomb.SetActive(false);
                foreach (GameObject piece in piecePool)
                    piece.SetActive(false);
                startButtonText.text = "Win!";
                startButton.GetComponent<FruitButton>().resetIcon();
                startButton.SetActive(true);
                break;
            case Status.LOSE:
                foreach (GameObject food in foodPool)
                    food.SetActive(false);
                foreach (GameObject bomb in bombPool)
                    bomb.SetActive(false);
                foreach (GameObject piece in piecePool)
                    piece.SetActive(false);
                startButtonText.text = "Lose!";
                startButton.GetComponent<FruitButton>().resetIcon();
                startButton.SetActive(true);
                break;
            case Status.END:
                clearPreviewStatistics();
                statisticsFoods.gameObject.SetActive(false);
                statisticsText.gameObject.SetActive(false);
                statisticsMiss.gameObject.SetActive(false);
                statisticsError.gameObject.SetActive(false);
                statisticsScore.gameObject.SetActive(false);
                statisticsBomb.gameObject.SetActive(false);
                break;
            case Status.PAUSE:
                if (applicationDown())
                {
                    status = Status.LAUNCH;
                    // hide menu
                    CuttingHelpController.show = false;
                }
                break;
        }
	}

    private void ShowStatisticsInfo()
    {
        statisticsMiss.text = "Miss: " + statisticsMissCount;
        statisticsScore.text = "Score: " + statisticsScoreCount;
        statisticsError.text = "Error: " + statisticsErrorCount;
        statisticsBomb.text = "Bomb: " + statisticsBombCount;
    }

    private void launchFood()
    {
        List<GameObject> freeFood = new List<GameObject>();

        // collect
        for (int i = 0; i < foodPool.Length; i++)
            if (!foodPool[i].activeSelf)
            {
                int dupulicateCount = 1;
                int index = i / poolCap;
                if (inManual(index) != -1) dupulicateCount += FoodSet.foods.Length / 2;
                for (int j = 0; j < dupulicateCount; j++)
                    freeFood.Add(foodPool[i]);
            }
        for (int i = 0; i < bombNum; i++)
            if (!bombPool[i].activeSelf)
                for (int j = 0; j < FoodSet.foods.Length; j++)
                    freeFood.Add(bombPool[i]);

        int maxLaunch = 3;
        GameObject[] choiceList = new GameObject[maxLaunch];
        int[] posList = new int[maxLaunch];
        for (int i = 0; i < maxLaunch; i++)
        {
            choiceList[i] = freeFood[UnityEngine.Random.Range(0, freeFood.Count)];
            posList[i] = UnityEngine.Random.Range(0, 9);
        }

        for (int i = 0; i < maxLaunch; i++) {
            int j;
            for (j = 0; j < i; j++)
            {
                if (choiceList[j] == choiceList[i]) break;
                if (posList[i] == posList[j]) break;
            }
            if (j == i)
            {
                choiceList[i].transform.localPosition = new Vector3(posList[i] % 3 - 1, posList[i] / 3 - 1, 0) * 3;
                choiceList[i].SetActive(true);
            }
        }
    }

    public void setManual(int manualIndex)
    {
        if (status == Status.FREE || status == Status.WAITING)
            if (manualIndex >= 0 && manualIndex < FoodSet.manuals.Length)
            {
                chosen = FoodSet.manuals[manualIndex];
                this.manualIndex = manualIndex;

                clearPreviewStatistics();


                // create statistics
                int count = chosen.foods.Length;
                statisticsCount = new int[count];
                statisticsCountText = new TextMesh[count];
                statisticsFoodsObj = new GameObject[count];
                statisticsTextsObj = new GameObject[count];
                for (int i = 0; i < count; i++)
                {
                    statisticsCount[i] = 0;
                    Vector3 offset = new Vector3(-statisticsHeight / 2 + (i + 1) * statisticsHeight / (count + 1), 0, 0);
                    GameObject tempFood;
                    tempFood = Instantiate(chosen.foods[i].obj, statisticsFoods);
                    tempFood.transform.localPosition = offset;
                    tempFood.transform.localScale = new Vector3(5, 5, 5);
                    statisticsFoodsObj[i] = tempFood;
                    GameObject tempText;
                    tempText = new GameObject();
                    tempText.transform.parent = statisticsText;
                    tempText.transform.localPosition = offset;
                    tempText.transform.localEulerAngles = new Vector3(0, 0, 0);
                    tempText.AddComponent<TextMesh>();
                    statisticsTextsObj[i] = tempText;
                    statisticsCountText[i] = tempText.GetComponent<TextMesh>();
                    statisticsCountText[i].text = 0 + "/" + chosen.nums[i];
                    statisticsCountText[i].anchor = TextAnchor.MiddleCenter;
                    statisticsCountText[i].characterSize = 0.2f;
                    statisticsCountText[i].fontSize = 60;
                }

                statisticsMissCount = 0;
                statisticsErrorCount = 0;
                statisticsBombCount = 0;
                statisticsScoreCount = 0;
                ShowStatisticsInfo();

                statisticsFoods.gameObject.SetActive(true);
                statisticsText.gameObject.SetActive(true);
                statisticsMiss.gameObject.SetActive(true);
                statisticsError.gameObject.SetActive(true);
                statisticsScore.gameObject.SetActive(true);
                statisticsBomb.gameObject.SetActive(true);

                startButtonText.text = "Start";
                startButton.GetComponent<FruitButton>().resetIcon();
                startButton.SetActive(true);
                CuttingHelpController.show = true;

                status = Status.WAITING;
            }
    }

    private void clearPreviewStatistics()
    {
        if (statisticsFoodsObj != null)
        {
            for (int i = statisticsFoodsObj.Length - 1; i >= 0; i--)
                Destroy(statisticsFoodsObj[i]);
            statisticsFoodsObj = null;
        }
        if (statisticsTextsObj != null)
        {
            for (int i = statisticsTextsObj.Length - 1; i >= 0; i--)
                Destroy(statisticsTextsObj[i]);
            statisticsTextsObj = null;
        }
    }

    public void cutFood(GameObject food)
    {
        if (status != Status.LAUNCH) return;
        if (food == null) return;
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
                    pieceVelocity[index * pieceEach + j] = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), 0);
                    piecePool[index * pieceEach + j].SetActive(true);
                }

                // statistics
                int indexInManual = inManual(index);
                if (indexInManual == -1)
                {
                    statisticsErrorCount++;
                    scoreMultiple = 1;
                    riseHint.hint("Error!", RiseHintController.HintType.ERROR, food.transform.position);
                }
                else
                {
                    statisticsCount[indexInManual]++;
                    statisticsCountText[indexInManual].text = statisticsCount[indexInManual] + "/" + chosen.nums[indexInManual];

                    statisticsScoreCount += 10 * scoreMultiple;
                    levelUpCount += 1;
                    if (levelUpCount == scoreMultiple)
                    {
                        levelUpCount = 0;
                        scoreMultiple += 1;
                        riseHint.hint("Level Up!", RiseHintController.HintType.GOOD, food.transform.position);
                    }
                    else
                    {
                        riseHint.hint("Combo X"+levelUpCount, RiseHintController.HintType.NORMAL, food.transform.position);
                    }

                    checkNum();
                }
                ShowStatisticsInfo();

                break;
            }
    }

    private void checkNum()
    {
        for (int i = 0; i < chosen.foods.Length; i++)
            if (statisticsCount[i] < chosen.nums[i]) return;

        status = Status.WIN;
    }

    public void cutBomb(GameObject bomb)
    {
        if (status != Status.LAUNCH) return;
        if (bomb == null) return;
        for (int i = 0; i < bombNum; i++)
            if (bombPool[i] == bomb)
            {
                bomb.SetActive(false);
                statisticsBombCount++;
                ShowStatisticsInfo();
                scoreMultiple = 1;
                levelUpCount = 0;
                if (statisticsBombCount >= maxBomb)
                {
                    status = Status.LOSE;
                }
                riseHint.hint("Bomb!", RiseHintController.HintType.ERROR, bomb.transform.position);
                break;
            }
    }

    private int inManual(int index)
    {
        for (int i = 0; i < chosen.foods.Length; i++)
            if (chosen.foods[i].key == FoodSet.foods[index].key)
                return i;
        return -1;
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

    public void StartGame()
    {
        if (status == Status.WAITING)
        {
            status = Status.PREPARE;
            startButtonText.text = "3";
            CuttingHelpController.show = false;
        }
        else if (status == Status.WIN || status == Status.LOSE)
        {
            status = Status.END;
            startButtonText.text = "Restart";
            startButton.GetComponent<FruitButton>().resetIcon();
        }
        else if (status == Status.END)
        {
            status = Status.WAITING;
            setManual(manualIndex);
            status = Status.PREPARE;
            startButtonText.text = "3";
            bufferTime = 0;
            second = 0;
            CuttingHelpController.show = false;
        }
    }

    

    // test
    public void CallMenu()
    {
        if (status == Status.LAUNCH)
        {
            status = Status.PAUSE;
            CuttingHelpController.show = true;
        }
        else if (status == Status.PAUSE)
        {
            status = Status.LAUNCH;
            // hide menu
            CuttingHelpController.show = false;
        }
    }

    public void CutCloseFood()
    {
        GameObject close = null;
        float dis = 0;
        for (int i = 0; i < foodPool.Length; i++)
            if (foodPool[i].activeSelf)
                if (foodPool[i].transform.position.z > dis)
                {
                    close = foodPool[i];
                    dis = foodPool[i].transform.position.z;
                }
        for (int i = 0; i < bombNum; i++)
            if (bombPool[i].activeSelf)
                if (bombPool[i].transform.position.z > dis)
                {
                    close = bombPool[i];
                    dis = bombPool[i].transform.position.z;
                }
        cutFood(close);
        cutBomb(close);
    }
}
