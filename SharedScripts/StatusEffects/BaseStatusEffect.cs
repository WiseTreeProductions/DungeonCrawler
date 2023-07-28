using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseStatusEffect
{
    public string effectName;
    public string description;

    public enum Tag
    {
        DoT,
        Buff,
        Debuff,
        StatChange,
        Physical,
        Mental,
        Biological,
        Magical,
        Curse
    }

    public enum ApplicationMethod
    {
        Instant,
        StartOfTurn
    }

    public List<Tag> tags = new List<Tag>();
    public ApplicationMethod applicationMethod;

    public float damage;
    public float fixedModifier;
    public float percentageModifier;
    public string targetStat;

    public int turnDuration;
    public float timeDuration;

    public Sprite Icon;
    public Sprite Effect;

}
