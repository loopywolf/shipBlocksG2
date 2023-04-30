using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTile : MonoBehaviour
{
    public char materialGiven = ' ';
    static GameObject gatherEffect;
    public bool hasItem = true;

    // Start is called before the first frame update
    void Start()
    {
        gatherEffect = Camera.main.GetComponent<CameraController>().gatherEffect; //has to work
    }//start

    // Update is called once per frame
    void Update()
    {
        
    }//update

    private void OnMouseUp()
    {
        //Debug.Log("clicked on a mytile! "+this.name+" material:"+this.materialGiven); //not sure I need this one
        //the player shoots at this tile with a "gatherer" bolt (to be replaced)
        
    }//F

    public void gather()
    {
        //gathers this tile to the player's inventory
        //1. spawn animation on the tile
        Instantiate(gatherEffect, transform.position, Quaternion.identity);
        //2. add tile to player's inventory
        Debug.Log("attempting to add item from " + gameObject.name);
        Camera.main.GetComponent<CameraController>().addToInventory(gameObject);
        //3. destroy this tile
        Destroy(gameObject);
    }//F

}//class
