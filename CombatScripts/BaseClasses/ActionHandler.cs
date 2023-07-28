using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionHandler
{
    public BaseAttack chosenAction;

    public float unmitigatedDamage;
    public float finalDamage;

    public float unmitigatedHeaing;
    public float finalHealing;

    public void RollDamage()
    {
        unmitigatedDamage = Mathf.RoundToInt(Random.Range(chosenAction.minDamage, chosenAction.maxDamage));
    }

    public void RollHealing()
    {
        unmitigatedHeaing = Mathf.RoundToInt(Random.Range(chosenAction.minDamage, chosenAction.maxDamage));
    }


    public void applyMitigation(GameObject target)
    {
        UnitStateManager targetUnit = target.GetComponent<UnitStateManager>();
        if (chosenAction.damageType == BaseAttack.DamageType.Physical)
        {
            finalDamage = unmitigatedDamage - targetUnit.combatUnit.physicalDefense;
        }
        else if (chosenAction.damageType == BaseAttack.DamageType.Magical)
        {
            finalDamage = unmitigatedDamage - targetUnit.combatUnit.magicalDefense;
        }

        switch (chosenAction.damageElement)
        {
            case (BaseAttack.DamageElement.Fire):
                {
                    finalDamage = finalDamage - finalDamage * (targetUnit.combatUnit.fireResistance / 100f);
                    break;
                }
            case (BaseAttack.DamageElement.Cold):
                {
                    finalDamage = finalDamage - finalDamage * (targetUnit.combatUnit.coldResistance / 100f);
                    break;
                }
            case (BaseAttack.DamageElement.Lightning):
                {
                    finalDamage = finalDamage - finalDamage * (targetUnit.combatUnit.lightningResistance / 100f);
                    break;
                }
            case (BaseAttack.DamageElement.Earth):
                {
                    finalDamage = finalDamage - finalDamage * (targetUnit.combatUnit.earthResistance / 100f);
                    break;
                }
            case (BaseAttack.DamageElement.Light):
                {
                    finalDamage = finalDamage - finalDamage * (targetUnit.combatUnit.lightResistance / 100f);
                    break;
                }
            case (BaseAttack.DamageElement.Dark):
                {
                    finalDamage = finalDamage - finalDamage * (targetUnit.combatUnit.darkResistance / 100f);
                    break;
                }
            case (BaseAttack.DamageElement.Force):
                {
                    finalDamage = finalDamage - finalDamage * (targetUnit.combatUnit.forceResistance / 100f);
                    break;
                }
            case (BaseAttack.DamageElement.Physical):
                {
                    finalDamage = finalDamage - finalDamage * (targetUnit.combatUnit.physicalResistance / 100f);
                    break;
                }
        }

        finalDamage = Mathf.RoundToInt(finalDamage);
    }

    public void ApplyHealingModifiers(GameObject target)
    {
        finalHealing = unmitigatedHeaing;
    }

}
