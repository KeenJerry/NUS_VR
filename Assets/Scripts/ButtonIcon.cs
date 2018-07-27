using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonIcon : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Cut>() != null)
            transform.parent.gameObject.GetComponent<FruitButton>().trigger();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Cut>() != null)
            transform.parent.gameObject.GetComponent<FruitButton>().trigger();
    }
}
