using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : ScriptableObject
{
    //Identification
    public static int unitDataID;
    public string unitName;

    //Main stats
    public int baseStrength;
    public int baseDexterity;
    public int baseAgility;
    public int basePerception;
    public int baseEndurance;
    public int baseIntelligence;
    public int baseMind;

    public int modifiedStrength;
    public int modifiedDexterity;
    public int modifiedAgility;
    public int modifiedPerception;
    public int modifiedEndurance;
    public int modifiedIntelligence;
    public int modifiedMind;

    public int currentStrength;
    public int currentDexterity;
    public int currentAgility;
    public int currentPerception;
    public int currentEndurance;
    public int currentIntelligence;
    public int currentMind;

    public enum Type
    {
        HUMANOID,
        BEAST,
        PLANT,
        CONSTRUCT,
        UNDEAD,
        SPIRIT,
        ELEMENTAL,
        DEMON,
        OUTSIDER
    }

    public List<Type> types = new List<Type>();

    //Combat stats
    //HP/MP
    public int baseMaxHitPoints;
    public int modifiedMaxHitPoints;
    public int currentMaxHitPoints;
    public int currentHitPoints;

    public int baseMaxEnergy;
    public int modifiedMaxEnergy;
    public int currentMaxEnergy;
    public int currentEnergy;

    //Offense
    public int baseAttackPower;
    public int modifiedAttackPower;
    public int currentAttackPower;

    public int baseSpellPower;
    public int modifiedSpellPower;
    public int currentSpellPower;

    public int baseAccuracy;
    public int modifiedAccuracy;
    public int currentAccuracy;

    //Defense
    public int basePhysicalDefense;
    public int modifiedPhysicalDefense;
    public int currentPhysicalDefense;

    public int baseMagicDefense;
    public int modifiedMagicDefense;
    public int currentMagicDefense;

    public int baseEvasion;
    public int modifiedEvasion;
    public int currentEvasion;

    //Damage resistance
    public int baseSlashResistance;
    public int modifiedSlashResistance;
    public int currentSlashResistance;

    public int basePierceResistance;
    public int modifiedPierceResistance;
    public int currentPierceResistance;

    public int baseBluntResistance;
    public int modifiedBluntResistance;
    public int currentBluntResistance;

    public int baseFireResistance;
    public int modifiedFireResistance;
    public int currentFireResistance;

    public int baseColdResistance;
    public int modifiedColdResistance;
    public int currentColdResistance;

    public int baseLightningResistance;
    public int modifiedLightningResistance;
    public int currentLightningResistance;

    public int baseEarthResistance;
    public int modifiedEarthResistance;
    public int currentEarthResistance;

    public int baseAstralResistance;
    public int modifiedAstralResistance;
    public int currentAstralResistance;

    public int baseDivineResistance;
    public int modifiedDivineResistance;
    public int currentDivineResistance;

    public int baseDarkResistance;
    public int modifiedDarkResistance;
    public int currentDarkResistance;

    public int baseVoidResistance;
    public int modifiedVoidResistance;
    public int currentVoidResistance;

    //Status resistance
    public int baseWoundResistance;
    public int modifiedWoundResistance;
    public int currentWoundResistance;

    public int baseBiologicalResistance;
    public int modifiedBiologicalResistance;
    public int currentBiologicalResistance;

    public int baseElementalResistance;
    public int modifiedElementalResistance;
    public int currentElementalResistance;

    public int baseMentalResistance;
    public int modifiedMentalResistance;
    public int currentMentalResistance;

    public int baseMagicalResistance;
    public int modifiedMagicalResistance;
    public int currentMagicalResistance;

    public int baseCurseResistance;
    public int modifiedCurseResistance;
    public int currentCurseResistance;

    //Speed
    public int baseSpeed;
    public int modifiedSpeed;
    public int currentSpeed;

    public int baseCastSpeed;
    public int modifiedCastSpeed;
    public int currentCastSpeed;

    //Misc
    public float baseMaxCarryWeight;
    public float modifiedMaxCarryWeight;
    public float currentMaxCarryWeight;
    public float currentCarryWeight;

    public float damageReceivedModifier;
    public float healingReceivedModifier;

    //public List<Effect> permanentEffects = new List<Effect>();
    //public List<Effect> longtermEffects = new List<Effect>();
    //public List<Effect> combatEffects = new List<Effect>();

    public void PrintAllInfo()
    {
        Debug.Log($"{unitName}. STR {baseStrength}, DEX {baseDexterity}, AGI {baseAgility}, PER {basePerception}, END {baseEndurance}, INT {baseIntelligence}, MND {baseMind}." +
            $"HP {baseMaxHitPoints}/{currentHitPoints}, MP: {currentEnergy}/{baseMaxEnergy}, " +
            $"ATK: {currentAttackPower}, SP: {currentSpellPower}, DEF: {currentPhysicalDefense}, MDEF: {currentMagicDefense}, EVA: {currentEvasion}");
    }


}
