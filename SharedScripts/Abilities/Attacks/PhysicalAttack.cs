using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalAttack : BaseAttack
{
    public PhysicalAttack()
    {
        attackName = "Attack";
        description = "Attack with your equipped weapon";
        minDamage = 8f;
        maxDamage = 12f;
        manaCost = 0f;
        delay = 50f;
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
