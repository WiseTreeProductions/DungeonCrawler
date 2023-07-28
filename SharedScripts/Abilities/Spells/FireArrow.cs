using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : BaseAttack
{
    public FireArrow()
    {
        attackName = "Fire Arrow";
        description = "Shoot an arrow of fire at the target";
        minDamage = 40f;
        maxDamage = 60f;
        manaCost = 10f;
        delay = 0f;
        castTime = 0f;
        targetable = true;
        isOffensive = true;

        targetType = TargetType.SingleTarget;

        damageType = DamageType.Magical;
        damageElement = DamageElement.Fire;
        range = Range.Medium;

        pierceValue = 0f;
    }
}
