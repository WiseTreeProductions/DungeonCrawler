using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/New Item")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public string itemName;
    public string description;
    public enum ItemType
    {
        CONSUMABLE,
        EQUIPPABLE,
        MATERIAL,
        QUEST,
        JUNK
    }

    public ItemType itemType;
    public bool stackable;
    public int stackSize;

}
