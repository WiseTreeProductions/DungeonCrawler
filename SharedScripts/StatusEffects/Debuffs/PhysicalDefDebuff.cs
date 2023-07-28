using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDefDebuff : BaseStatusEffect
{
    public PhysicalDefDebuff(float modifier)
    {
        effectName = "Physical Defense Reduction";
        description = "Reduces physical defense";

        tags.Add(BaseStatusEffect.Tag.Debuff);
        tags.Add(BaseStatusEffect.Tag.StatChange);
        tags.Add(BaseStatusEffect.Tag.Physical);

        turnDuration = 3;

        percentageModifier = -modifier;
        targetStat = "PhysicalDefense";

        applicationMethod = BaseStatusEffect.ApplicationMethod.Instant;

    }

}
