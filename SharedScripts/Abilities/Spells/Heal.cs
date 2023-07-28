using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : BaseAttack
{

    public Heal()
    {
        attackName = "Heal";
        description = "Heal the target";
        minDamage = 50f;
        maxDamage = 50f;
        manaCost = 15f;
        delay = 50f;
        castTime = 20f;
        targetable = true;
        isOffensive = false;

        targetType = TargetType.SingleTarget;

        damageType = DamageType.Magical;
        damageElement = DamageElement.Light;
        range = Range.Long;

        pierceValue = 0f;

    }
}
