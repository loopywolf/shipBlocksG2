using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    List<Item> inventorySlot;
    private const int INVENTORY_SIZE = 12;
    public GameObject inventorySlotPrefab;
    public GameObject inventorySlotsParent;
    private static bool uiShowing = false;
    public GameObject inventoryUiPanel;
    //selected item
    private int selected = 0;   //first, by default

    //TODO once it works, removes the dummy entry

    private void Awake()
    {
        inventorySlot = new List<Item>(); //Item[INVENTORY_SIZE];        
    }//F
    
    internal void add(GameObject go)
    {
        //1. add item to internal data (inventory) 
        /*Item i = go.GetComponent<Item>();
        if (i == null)
        {
            Debug.Log("No Item");
            return;
        }//if
        Debug.Log("i:inventory adding");
        Debug.Log("i:go=" + go.name);*/
        if(go.GetComponent<MyTile>() == null ||
            !go.GetComponent<MyTile>().hasItem)
        {
            Debug.Log("Not an item.  cannot add");
            return;
        }

        //i.amount = 1;   //work on stacking later //TODO
        Item itemAdded = null;
        Sprite s = go.GetComponent<SpriteRenderer>().sprite; //would sprite name do for material?
        if (s != null)
        {
            Debug.Log("i:s=" + s.name);
            itemAdded = new Item(s);
            inventorySlot.Add(itemAdded);
            Debug.Log("added inventory item " + itemAdded.icon.name);
            refreshDisplay();
        }
        else
        {
            Debug.Log("Sprite not found for go=" + go.name);
        }//if s OK

        /*  //try to give item a sprite based on the parent
            s = go.transform.parent.GetComponent<SpriteRenderer>().sprite;
            i.icon = s;
            if(s!=null)
            Debug.Log("i:2nd s=" + s.name); */


        //throw new NotImplementedException();
    }//F

    private void refreshDisplay()
    {
        //2a. get container - got
        //2b. clear container
        foreach (Transform child in inventorySlotsParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }//for clear
        Debug.Log("removed children");
        //works to here!

        //2c. add one prefab per inventorySlot
        foreach (var inv in inventorySlot)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab, inventorySlotsParent.transform);
            //now I want to reference the Image component under Icon
            //Transform icon = newSlot.transform.Find("icon"); //this isn't working
            Transform icontf = newSlot.transform.GetChild(0); //clumsy... //TODO

            if (icontf != null)
            {
                Image iconImage = icontf.gameObject.GetComponent<Image>();
                if (iconImage != null)
                {
                    iconImage.sprite = inv.icon;
                    Debug.Log("display changed " + iconImage.sprite.name);
                }//if iconImage OK
            }//if icon OK
            //Image.sprite - remember */
        }//for 

    }//F

    public void switchInventoryOnOff()
    {
        //throw new NotImplementedException();
        uiShowing = !uiShowing;
        inventoryUiPanel.SetActive(uiShowing);
    }//F

}//class
