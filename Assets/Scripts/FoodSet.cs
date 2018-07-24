using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FoodSet : MonoBehaviour {
    public class Food
    {
        public int key;
        public string name;
        public GameObject obj;
    }

    public class Manual
    {
        public string name;
        public int key;
        public Food[] foods;
        public int[] nums;
    }

    static public Food[] foods;
    static public Manual[] manuals;

    public GameObject[] objs;

    void Awake() 
    {
        foods = loadFood(Application.dataPath + "/Data/foods.json");
        manuals = loadManual(Application.dataPath + "/Data/manuals.json");
    }

    // Use this for initialization
    void Start () {
        for (int i = 0; i < objs.Length; i++)
            foods[i].obj = objs[i];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private Food[] loadFood(string path)
    {
        StreamReader sr = new StreamReader(path);
        string str = sr.ReadToEnd();
        FoodJson[] rawData = JsonUtility.FromJson<FoodsJson>(str).foods;

        Food[] result = new Food[rawData.Length];

        for (int i = 0; i < rawData.Length; i++)
        {
            result[i] = new Food();
            result[i].name = rawData[i].name;
            result[i].key = rawData[i].key;
        }

        return result;
    }

    private Manual[] loadManual(string path)
    {
        StreamReader sr = new StreamReader(path);
        string str = sr.ReadToEnd();
        ManualJson[] rawData = JsonUtility.FromJson<ManualsJson>(str).manuals;
        Manual[] result = new Manual[rawData.Length];

        for (int i = 0; i < rawData.Length; i++)
        {
            result[i] = new Manual();
            result[i].name = rawData[i].name;
            result[i].key = rawData[i].key;
            result[i].foods = new Food[rawData[i].foods.Length];
            result[i].nums = new int[rawData[i].foods.Length];
            for (int j = 0; j < rawData[i].foods.Length; j++)
            {
                if (foods[rawData[i].foods[j].key].key == rawData[i].foods[j].key)
                    result[i].foods[j] = foods[rawData[i].foods[j].key];
                else
                    for (int k = 0; k < foods.Length; k++)
                        if (foods[k].key == rawData[i].foods[j].key)
                        {
                            result[i].foods[j] = foods[k];
                            break;
                        }
                result[i].nums[j] = rawData[i].foods[j].num;
            }
        }
            
        return result;
    }

    [System.Serializable]
    class FoodsJson
    {
        public FoodJson[] foods;
    }

    [System.Serializable]
    class FoodJson
    {
        public int key;
        public string name;
    }

    [System.Serializable]
    class SetJson
    {
        public int key;
        public int num;
    }

    [System.Serializable]
    class ManualsJson
    {
        public ManualJson[] manuals;
    }

    [System.Serializable]
    class ManualJson
    {
        public string name;
        public SetJson[] foods;
        public int key;
    }
}
