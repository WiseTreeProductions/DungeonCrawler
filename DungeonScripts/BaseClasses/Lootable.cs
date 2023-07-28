using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Lootable : Interactable
{
    public bool isLooted;
    public List<Item> items;
    public int gold;
}
