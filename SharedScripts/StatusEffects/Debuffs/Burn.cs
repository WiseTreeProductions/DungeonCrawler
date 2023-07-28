using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : BaseStatusEffect
{
    public Burn(BaseAttack spell)
    {
        effectName = "Burn";
        description = "Does damage every turn";

        tags.Add(BaseStatusEffect.Tag.Debuff);
        tags.Add(BaseStatusEffect.Tag.DoT);
        tags.Add(BaseStatusEffect.Tag.Physical);

        turnDuration = 2;

        applicationMethod = BaseStatusEffect.ApplicationMethod.StartOfTurn;

    }

}
