using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : BaseAttack
{
    public FireBolt()
    {
        attackName = "Fire Bolt";
        description = "Shoot a bolt of fire at the target";
        minDamage = 70f;
        maxDamage = 80f;
        manaCost = 15f;
        delay = 50f;
        castTime = 30f;
        targetable = true;
        isOffensive = true;

        targetType = TargetType.SingleTarget;

        damageType = DamageType.Magical;
        damageElement = DamageElement.Fire;
        range = Range.Long;

        pierceValue = 0f;

        statusEffects.Add(new Burn(this));
    }
}
