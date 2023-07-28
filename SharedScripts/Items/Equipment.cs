using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wearable", menuName = "Item/New Wearable Equipment")]
public class Equipment : Item
{
    public Equipment()
    {
        itemType = ItemType.EQUIPPABLE;
    }

    public enum EquipmentSlot
    {
        HAND,
        TORSO,
        LEGS,
        FEET,
        ARMS,
        BACK,
        ACCESSORY
    }

    public EquipmentSlot equipmentSlot;

    public int maxHitPoints;
    public int maxEnergy;

    public int strength;
    public int dexterity;
    public int agility;
    public int perception;
    public int endurance;
    public int intelligence;
    public int mind;

    public int physicalAttack;
    public int magicalAttack;
    public int physicalDefense;
    public int magicalDefense;

    public int accuracy;
    public int speed;
    public int castSpeed;
    public int evasion;

    public int slashResistance;
    public int pierceResistance;
    public int bluntResistance;
    public int fireResistance;
    public int coldResistance;
    public int earthResistance;
    public int lightningResistance;
    public int divineResistance;
    public int darkResistance;
    public int astralResistance;


    //public abstract void OnEquip();
    //public abstract void OnUnequip();

}
