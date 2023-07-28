using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAttack : MonoBehaviour
{
    public string attackName;
    public string description;

    public float minDamage;
    public float maxDamage;
    public float manaCost;
    public float baseAccuracy;

    public float delay;
    public float castTime;

    public Sprite battleEffect;
    public Sprite damageEffect;

    public bool targetable;
    public bool isOffensive;

    public enum TargetType
    {
        NoTarget,
        SelfTarget,
        SingleTarget,
        MultiTarget,
        AoE,
        All
    }

    public int rowCount;
    public int columnCount;

    public TargetType targetType;

    public int targetableRows;
    public int targetableColumns;

    public enum DamageType
    {
        Physical,
        Magical,
        True
    }

    public DamageType damageType;

    public enum DamageElement
    {
        Fire,
        Cold,
        Lightning,
        Earth,
        Light,
        Dark,
        Force,
        Physical
    }

    public DamageElement damageElement;

    public enum Range
    {
        Short,
        Medium,
        Long
    }

    public Range range;

    public float pierceValue;

    public List<BaseStatusEffect> statusEffects = new List<BaseStatusEffect>();


}
