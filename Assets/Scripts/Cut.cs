using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut : MonoBehaviour {

    // Use this for initialization
    private GameObject Trajectory;
    public AudioSource CutAudio;
    public AudioSource BombAudio;
    public GameObject PoolManager;
    private GameObjectPool Pool;
    // public GameObject ExplodeInstance;
	void Start () {
        PoolManager = GameObject.FindGameObjectWithTag("pool");
        Pool = PoolManager.GetComponent<GameObjectPool>();
        Trajectory = GameObject.Find("Trajectory");
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("food"))
        {   
            if(Trajectory)
                Trajectory.GetComponent<LaunchFood>().cutFood(other.gameObject);
            // music
            if(CutAudio)
                CutAudio.PlayOneShot(CutAudio.clip);
        }
        if (other.CompareTag("bomb"))
        {
            if (Trajectory)
                Trajectory.GetComponent<LaunchFood>().cutBomb(other.gameObject);
            // music
            if (BombAudio)
            {
                BombAudio.PlayOneShot(BombAudio.clip);
                GameObject ExplodeInstance = Pool.GetGameObject();
                ExplodeInstance.transform.localPosition = other.transform.position;
                ExplodeInstance.GetComponentInChildren<ParticleSystem>().Play();
            }
        }
    }
}
