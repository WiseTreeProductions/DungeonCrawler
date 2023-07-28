using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerAttack : BaseAttack
{
    public PowerAttack()
    {
        attackName = "Power Attack";
        description = "Perform a strong but draining attack";
        minDamage = 20f;
        maxDamage = 40f;
        manaCost = 5f;
        delay = 30f;
        castTime = 0f;
        targetable = true;
        isOffensive = true;

        targetType = TargetType.SingleTarget;

        damageType = DamageType.Physical;
        damageElement = DamageElement.Physical;
        range = Range.Short;

        pierceValue = 0f;
    }
}
