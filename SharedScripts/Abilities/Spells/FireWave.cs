using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWave : BaseAttack
{
    public FireWave()
    {
        attackName = "Fire Wave";
        description = "Cover a row with a wave of fire";
        minDamage = 30f;
        maxDamage = 50f;
        manaCost = 30f;
        delay = 0f;
        castTime = 20f;
        targetable = true;
        isOffensive = true;

        targetType = TargetType.AoE;

        rowCount = 1;
        columnCount = 5;

        damageType = DamageType.Magical;
        damageElement = DamageElement.Fire;
        range = Range.Long;

        pierceValue = 0f;
    }
}
