using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Holdable", menuName = "Item/New Holdable Equipment")]
public class Holdable : Equipment
{
    public enum WeaponRange
    {
        SHORT,
        MEDIUM,
        LONG
    }

    public WeaponRange weaponRange;
    public bool twoHanded;
    public bool offHandOnly;


}
