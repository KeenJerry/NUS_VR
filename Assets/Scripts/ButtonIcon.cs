using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonIcon : MonoBehaviour {
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Trigger");
    //    if (other.gameObject.GetComponent<Cut>() != null)
    //        transform.parent.gameObject.GetComponent<FruitButton>().trigger();
    //}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.GetComponent<Cut>() != null)
            transform.parent.gameObject.GetComponent<FruitButton>().trigger();
    }
}
