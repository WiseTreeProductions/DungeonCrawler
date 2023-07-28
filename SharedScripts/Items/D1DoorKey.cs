using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D1DoorKey : Item
{
    public D1DoorKey()
    {
        itemName = "Rusty key";
        description = "An old looking key.";
        itemType = ItemType.QUEST;
        icon = Resources.Load<Sprite>("ItemIcons/RustyKeyIcon");
    }
}
