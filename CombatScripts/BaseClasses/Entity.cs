using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Entity
{
    public string entityName;
    public string displayName;

    public enum Type
    {
        HUMANOID,
        BEAST,
        PLANT,
        CONSTRUCT,
        UNDEAD,
        SPIRIT,
        ELEMENTAL,
        DEMON,
        OUTSIDER
    }

    public Type type;

    public enum UnitType
    {
        Player,
        Enemy
    }

    public UnitType unitType;

    public enum AIType
    {
        Player,
        Balanced,
        Agressive,
        Defensive,
        Assassin,
        Healer
    }

    public AIType AI;

    public float maxHP;
    public float currentHP;

    public float maxMP;
    public float currentMP;

    public float damage;
    public float speed;

    //RNG shit
    public float accuracy;
    public float evasion;

    //Defenses
    public float physicalDefense;
    public float originalPDefense;

    public float magicalDefense;
    public float originalMDefense;

    //Damage resistances
    public float fireResistance;
    public float coldResistance;
    public float lightningResistance;
    public float earthResistance;
    public float lightResistance;
    public float darkResistance;
    public float forceResistance;
    public float physicalResistance;

    public float initiative;

    public List<BaseAttack> attacks = new List<BaseAttack>();
    public List<BaseAttack> Spells = new List<BaseAttack>();
    public List<BaseAttack> Actions = new List<BaseAttack>();



}
