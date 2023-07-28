using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "New Player Unit", menuName = "Units/New Player Unit")]
public class PlayerUnitData : UnitData
{
    public Sprite portraitSmall;
    public Sprite portraitLarge;

    //public CharacterClass class;

    public int currentExp = 0;
    public int neededExp = 100;

    public int level = 1;

    public int statPoints = 0;
    public int skillPoints = 0;

    //public Ability abilities;

    public Equipment mainHandSlot;
    public Equipment offHandSlot;
    public Equipment torsoSlot;
    public Equipment legSlot;
    public Equipment feetSlot;
    public Equipment armSlot;
    public Equipment backSlot;
    public Equipment accessory1Slot;
    public Equipment accessory2Slot;
    public Equipment accessory3Slot;
    public Equipment accessory4Slot;
    public Equipment accessory5Slot;

    public List<Equipment> equipment = new List<Equipment>();

    public void CreateCharacter(string name, Sprite portraitSmall, Sprite portraitLarge, int strength, int dexterity, int agility, int perception, int endurance, int intelligence, int mind)
    {
        unitName = name;
        this.portraitSmall = portraitSmall;
        this.portraitLarge = portraitLarge;

        baseStrength = strength;
        modifiedStrength = baseStrength;
        currentStrength = baseStrength;

        baseDexterity = dexterity;
        modifiedDexterity = baseDexterity;
        currentDexterity = baseDexterity;

        baseAgility = agility;
        modifiedAgility = baseAgility;
        currentAgility = baseAgility;

        basePerception = perception;
        modifiedPerception = basePerception;
        currentPerception = basePerception;

        baseEndurance = endurance;
        modifiedEndurance = baseEndurance;
        currentEndurance = baseEndurance;

        baseIntelligence = intelligence;
        modifiedIntelligence = baseIntelligence;
        currentIntelligence = baseIntelligence;

        baseMind = mind;
        modifiedMind = baseMind;
        currentMind = baseMind;

        //Later: BASE CLASS HP/MP VALUES

        types.Add(Type.HUMANOID);

        baseMaxHitPoints = 20 + endurance * 3 + strength;
        modifiedMaxHitPoints = baseMaxHitPoints;
        currentMaxHitPoints = baseMaxHitPoints;
        currentHitPoints = baseMaxHitPoints;

        baseMaxEnergy = 10 + mind * 3 + intelligence;
        modifiedMaxEnergy = baseMaxEnergy;
        currentMaxEnergy = baseMaxEnergy;
        currentEnergy = baseMaxEnergy;

        baseAttackPower = 2 * strength;
        modifiedAttackPower = baseAttackPower;
        currentAttackPower = baseAttackPower;

        baseSpellPower = 2 * intelligence;
        modifiedSpellPower = baseSpellPower;
        currentSpellPower = baseSpellPower;

        baseAccuracy = 60 + dexterity;
        modifiedAccuracy = baseAccuracy;
        currentAccuracy = baseAccuracy;

        basePhysicalDefense = endurance * 2;
        modifiedPhysicalDefense = basePhysicalDefense;
        currentPhysicalDefense = basePhysicalDefense;

        baseMagicDefense = mind * 4;
        modifiedMagicDefense = baseMagicDefense;
        currentMagicDefense = baseMagicDefense;

        baseEvasion = agility * 3;
        modifiedEvasion = baseEvasion;
        currentEvasion = baseEvasion;

        //Make it depend on class later
        baseSlashResistance = 0;
        modifiedSlashResistance = baseSlashResistance;
        currentSlashResistance = baseSlashResistance;

        basePierceResistance = 0;
        modifiedPierceResistance = basePierceResistance;
        currentPierceResistance = basePierceResistance;

        baseBluntResistance = 0;
        modifiedBluntResistance = baseBluntResistance;
        currentBluntResistance = baseBluntResistance;

        baseFireResistance = 0;
        modifiedFireResistance = baseFireResistance;
        currentFireResistance = baseFireResistance;

        baseColdResistance = 0;
        modifiedColdResistance = baseColdResistance;
        currentColdResistance = baseColdResistance;

        baseLightningResistance = 0;
        modifiedLightningResistance = baseLightningResistance;
        currentLightningResistance = baseLightningResistance;

        baseEarthResistance = 0;
        modifiedEarthResistance = baseEarthResistance;
        currentEarthResistance = baseEarthResistance;

        baseAstralResistance = 0;
        modifiedAstralResistance = baseAstralResistance;
        currentAstralResistance = baseAstralResistance;

        baseDivineResistance = 0;
        modifiedDivineResistance = baseDivineResistance;
        currentDivineResistance = baseDivineResistance;

        baseDarkResistance = 0;
        modifiedDarkResistance = baseDarkResistance;
        currentDarkResistance = baseDarkResistance;

        baseVoidResistance = 0;
        modifiedVoidResistance = baseVoidResistance;
        currentVoidResistance = baseVoidResistance;

        baseWoundResistance = 0;
        modifiedWoundResistance = baseWoundResistance;
        currentWoundResistance = baseWoundResistance;

        baseBiologicalResistance = 0;
        modifiedBiologicalResistance = baseBiologicalResistance;
        currentBiologicalResistance = baseBiologicalResistance;

        baseElementalResistance = 0;
        modifiedElementalResistance = modifiedElementalResistance;
        currentElementalResistance = modifiedElementalResistance;

        baseMentalResistance = 0;
        modifiedMentalResistance = baseMentalResistance;
        currentMentalResistance = baseMentalResistance;

        baseMagicalResistance = 0;
        modifiedMagicalResistance = baseMagicalResistance;
        currentMagicalResistance = baseMagicalResistance;

        baseCurseResistance = 0;
        modifiedCurseResistance = baseCurseResistance;
        currentCurseResistance = baseCurseResistance;

        baseSpeed = 10 + agility;
        modifiedSpeed = baseSpeed;
        currentSpeed = baseSpeed;

        baseCastSpeed = 0;
        modifiedCastSpeed = baseCastSpeed;
        currentCastSpeed = baseCastSpeed;

        baseMaxCarryWeight = 10 * strength;
        modifiedMaxCarryWeight = baseMaxCarryWeight;
        currentMaxCarryWeight = baseMaxCarryWeight;
        currentCarryWeight = 0;

        damageReceivedModifier = 0;
        healingReceivedModifier = 0;

    }

    public void UpdateEquipment()
    {
        equipment.Clear();
        equipment.Add(mainHandSlot);
        equipment.Add(offHandSlot);
        equipment.Add(torsoSlot);
        equipment.Add(legSlot);
        equipment.Add(feetSlot);
        equipment.Add(armSlot);
        equipment.Add(backSlot);
        equipment.Add(accessory1Slot);
        equipment.Add(accessory2Slot);
        equipment.Add(accessory3Slot);
        equipment.Add(accessory4Slot);
        equipment.Add(accessory5Slot);
    }

    public void RecalculateStats()
    {
        Debug.Log("KEK");
        ResetToBaseStats();

        foreach (Equipment item in equipment)
        {
            Debug.Log(item);
            //Debug.Log($"{item.itemName} is equipped");
            ModifyStatsFromEquipment(item);
        }
    }

    public void ResetToBaseStats()
    {
        modifiedMaxHitPoints = baseMaxHitPoints;
        modifiedMaxEnergy = baseMaxEnergy;
        modifiedStrength = baseStrength;
        modifiedDexterity = baseDexterity;
        modifiedAgility = baseAgility;
        modifiedPerception = basePerception;
        modifiedEndurance = baseEndurance;
        modifiedIntelligence = baseIntelligence;
        modifiedMind = baseMind;
        modifiedAttackPower = baseAttackPower;
        modifiedSpellPower = baseSpellPower;
        modifiedPhysicalDefense = basePhysicalDefense;
        modifiedMagicDefense = baseMagicDefense;
        modifiedAccuracy = baseAccuracy;
        modifiedEvasion = baseEvasion;
        modifiedSpeed = baseSpeed;
        modifiedCastSpeed = baseCastSpeed;

        modifiedSlashResistance = baseSlashResistance;
        modifiedPierceResistance = basePierceResistance;
        modifiedBluntResistance = baseBluntResistance;
        modifiedFireResistance = baseFireResistance;
        modifiedColdResistance = baseColdResistance;
        modifiedEarthResistance = baseEarthResistance;  
        modifiedLightningResistance = baseLightningResistance;
        modifiedDivineResistance = baseDivineResistance;
        modifiedDarkResistance = baseDarkResistance;
        modifiedAstralResistance = baseAstralResistance;
    }

    public void ModifyStatsFromEquipment(Equipment item)
    {
        modifiedMaxHitPoints += item.maxHitPoints;
        modifiedMaxEnergy += item.maxEnergy;
        modifiedStrength += item.strength;
        modifiedDexterity += item.dexterity;
        modifiedAgility += item.agility;
        modifiedPerception += item.perception;
        modifiedEndurance += item.endurance;
        modifiedIntelligence += item.intelligence;
        modifiedMind += item.mind;
        modifiedAttackPower += item.physicalAttack;
        modifiedSpellPower += item.magicalAttack;
        modifiedPhysicalDefense += item.physicalDefense;
        modifiedMagicDefense += item.magicalDefense;
        modifiedAccuracy += item.accuracy;
        modifiedEvasion += item.evasion;
        modifiedSpeed += item.speed;
        modifiedCastSpeed += item.castSpeed;

        modifiedSlashResistance += item.slashResistance;
        modifiedPierceResistance += item.pierceResistance;
        modifiedBluntResistance += item.bluntResistance;
        modifiedFireResistance += item.fireResistance;
        modifiedColdResistance += item.coldResistance;
        modifiedLightningResistance += item.lightningResistance;
        modifiedDivineResistance += item.divineResistance;
        modifiedDarkResistance += item.darkResistance;
        modifiedAstralResistance += item.astralResistance;
    }

    public void EquipItem(Equipment item)
    {
        Debug.Log($"{item.itemName} is {item.equipmentSlot} slot");
        switch (item.equipmentSlot)
        {
            case (Equipment.EquipmentSlot.HAND):
                {
                    Holdable handItem = (Holdable)item;
                    if (!handItem.twoHanded)
                    {
                        if (handItem.offHandOnly)
                        {
                            offHandSlot = handItem;
                        }
                        else
                        {
                            if (mainHandSlot == null)
                            {
                                mainHandSlot = handItem;
                            }
                            else if (mainHandSlot != null && offHandSlot == null)
                            {
                                offHandSlot = handItem;
                            }
                            else if (mainHandSlot != null && offHandSlot != null)
                            {
                                mainHandSlot = handItem;
                            }
                        }
                    }
                    else
                    {
                        mainHandSlot = handItem;
                    }
                    break;
                }
            case (Equipment.EquipmentSlot.TORSO):
                {
                    torsoSlot = item;
                    break;
                }
            case (Equipment.EquipmentSlot.LEGS):
                {
                    legSlot = item;
                    break;
                }
            case (Equipment.EquipmentSlot.FEET):
                {
                    feetSlot = item;
                    break;
                }
            case (Equipment.EquipmentSlot.ARMS):
                {
                    armSlot = item;
                    break;
                }
            case (Equipment.EquipmentSlot.BACK):
                {
                    backSlot = item;
                    break;
                }
            case (Equipment.EquipmentSlot.ACCESSORY):
                {
                    if (accessory1Slot == null)
                    {
                        accessory1Slot = item;
                    }
                    else
                    {
                        if (accessory2Slot == null)
                        {
                            accessory2Slot = item;
                        }
                        else
                        {
                            if (accessory3Slot == null)
                            {
                                accessory3Slot = item;
                            }
                            else
                            {
                                if (accessory4Slot == null)
                                {
                                    accessory4Slot = item;
                                }
                                else
                                {
                                    if (accessory5Slot == null)
                                    {
                                        accessory5Slot = item;
                                    }
                                    else
                                    {
                                        accessory1Slot = item;
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
        UpdateEquipment();
        RecalculateStats();
    }
}
