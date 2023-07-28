using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModType
{
    Flat,
    Percent,
}

public class StatModifier
{
    public readonly float value;
    public readonly StatModType type;
    public readonly int order;

    public StatModifier(float value, StatModType type, int order)
    {
        this.value = value;
        this.type = type;
        this.order = order;
    }

    public StatModifier(float value, StatModType type) : this (value, type, (int)type)
    {

    }
}
