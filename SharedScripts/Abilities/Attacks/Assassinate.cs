using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassinate : BaseAttack
{
    public Assassinate()
    {
        attackName = "Assassinate";
        description = "Quickly close the distance to the target and strike their weak point, ignoring their armor";
        minDamage = 50f;
        maxDamage = 70f;
        manaCost = 15f;
        delay = 40f;
        castTime = 0f;
        targetable = true;
        isOffensive = true;

        targetType = TargetType.SingleTarget;

        damageType = DamageType.Physical;
        damageElement = DamageElement.Physical;
        range = Range.Long;

        pierceValue = 100f;
    }
}
