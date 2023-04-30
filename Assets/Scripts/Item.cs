using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    //anything with this component is an item and can be picked up
    //amount=1 //one day, but for now, nothing important here
    public Sprite icon;

    public Item(Sprite s)
    {
        icon = s;
    }//+

}//class
