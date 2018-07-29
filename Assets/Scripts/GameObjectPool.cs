using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{

    // Use this for initialization
    public int MaxNum = 10;
    public GameObject Prototype;
    private int LeftNum;
    public List<GameObject> GameObjectList;

    void Start()
    {
        LeftNum = MaxNum;
        GameObjectList = new List<GameObject>(MaxNum);
        for(int i = 0; i < MaxNum; i++)
        {
            GameObjectList.Add(Instantiate(Prototype));
            GameObjectList[i].SetActive(false);
        }
    }

    public GameObject GetGameObject()
    {
        if (LeftNum == 0)
            return null;
        else
        {
            foreach (GameObject Object in GameObjectList)
            {
                if (Object.activeSelf == false)
                {
                    Object.SetActive(true);
                    LeftNum--;
                    return Object;
                }
                else
                {
                    continue;
                }
            }
            return null;
        }
    }

    public void ReturnObject()
    {
        LeftNum++;
    }
    private void Update()
    {
        
    }

}
