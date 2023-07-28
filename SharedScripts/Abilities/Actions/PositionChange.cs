using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChange : BaseAttack
{
    public PositionChange()
    {
        attackName = "Change Position";
        description = "Change the character's position on the battle field";
        delay = 50f;
        targetable = false;
        targetType = TargetType.NoTarget;

    }
}
