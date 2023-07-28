using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposeWeakness : BaseAttack
{
    public ExposeWeakness()
    {
        attackName = "Expose Weakness";
        description = "Exposes weak points in the target's armor, lowering their physical defense";
        minDamage = 0f;
        maxDamage = 0f;
        manaCost = 10f;
        delay = 75f;
        castTime = 0f;
        baseAccuracy = 100;
        targetable = true;
        isOffensive = true;

        targetType = TargetType.SingleTarget;

        damageType = DamageType.Physical;
        damageElement = DamageElement.Physical;
        range = Range.Long;

        pierceValue = 0f;

        statusEffects.Add(new PhysicalDefDebuff(25f));
    }
}
