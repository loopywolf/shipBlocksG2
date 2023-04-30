using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public GameObject followTarget;
    private Vector3 targetPosition;
    public float cameraSpeed;
    public GameObject powEffect;
    public GameObject currentPlayer;
    public GameObject DestructibleTileLayer;
    private Grid destructibleGrid;
    public GameObject TileLayer2;
    private Tilemap tiles2;
    public GameObject gatherEffect;
    //inventory
    Inventory myInventory;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = GameObject.Find("Player");  //this will be switchable, but for now is OK
        destructibleGrid = DestructibleTileLayer.GetComponent<Grid>();
        myInventory = GetComponent<Inventory>();
    }//Start

    // Update is called once per frame
    void Update()
    {
        //camera movement to follow the chr
        targetPosition = new Vector3(
            followTarget.transform.position.x,
            followTarget.transform.position.y,
            transform.position.z);
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            cameraSpeed * Time.deltaTime);

        if( Input.GetKeyUp( KeyCode.I))
        {
            GetComponent<Inventory>().switchInventoryOnOff();
        }//if

        //raycast clicking test
        if( Input.GetMouseButtonDown(1) ) {  //I think that's RMB / yes - OOh I have RMB
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2d, Vector2.zero);
            if (hit.collider != null) //not the firing code, note RMB
            {
                Debug.Log("RMB detected hit " + hit.collider.gameObject.name + " at " + mousePos2d);
                //this might be a grid or a tilemap
                //Tilemap tl = hit.collider.gameObject.GetComponent<Tilemap>();
                //Debug.Log("Tilemap " + tl);//see if this works
                //Grid g = hit.collider.gameObject.GetComponent<Grid>();
                //Debug.Log("Grid is " + g);//but there is only 1 GRID
                Tilemap tm = hit.collider.gameObject.GetComponent<Tilemap>();
                Debug.Log("Tilemap " + tm);//see if this works
                Grid g = tm.GetComponentInParent<Grid>();
                Debug.Log("Grid is " + g);//but there is only 1 GRID - works
                TileBase currentTile = tm.GetTile(new Vector3Int( (int)mousePos2d.x, (int)mousePos2d.y, 0));
                Debug.Log("currenTile("+(int)mousePos2d.x+","+(int)mousePos2d.y+") is " + currentTile.name); //this is not working
                //it works but not always              

                /* //Debug.Log("Tile is " + destructibleGrid.WorldToCell(mousePos2d)); -11,1,0
                tiles2 = TileLayer2.GetComponent<Tilemap>();
                Vector3Int stupidMousePos3 = new Vector3Int((int)mousePos.x, (int)mousePos.y, 0);
                Debug.Log("Tile is " + tiles2.GetTile(stupidMousePos3));
                Sprite s = tiles2.GetSprite(stupidMousePos3);
                Debug.Log("Sprite is " + s.name);
                //hit.collider.attachedRigidbody.AddForce(Vector2.up); */
            }//if
            else
            {
                Debug.Log("RMB mouse click at " + mousePos);
                GameObject go = GameObject.Find("Player");
                if (go != null) {
                    PlayerController pc = go.GetComponent<PlayerController>();
                    if (pc != null && false) //disabled
                        pc.SetDestination(mousePos);
                }//if
            }//else if
        }//if raycast

    }//Update

    /* public void addToInventory(char i)
    {
        //Debug.Log("adding " + i + " to inventory");
        //Camera.main.GetComponent<InventoryAP>().add(i);
    }//F */

    public void addToInventory(GameObject go)
    {
        //adds the item to the inventory
        //1. add item to inventory
        //2. refresh Inventory display
        Debug.Log("cc:asking inventory to add " + go.name);
        myInventory.add(go);
    }//F

}//class