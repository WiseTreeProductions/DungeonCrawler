using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : BaseAttack
{
    public Wait()
    {
        attackName = "Wait";
        description = "Wait for a better opportunity";
        delay = 70f;
        targetable = false;
        targetType = TargetType.NoTarget;

    }
}
