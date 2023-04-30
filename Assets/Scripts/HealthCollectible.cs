using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        //Do something to gain this object
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.addItem("box");
            Destroy(gameObject);
        }//if
        Debug.Log("Object that touched box " + other);
    }//onTriggerEnter2D

}//class
