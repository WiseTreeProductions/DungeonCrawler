using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Unit", menuName = "Units/New Enemy Unit")]
public class EnemyUnitData : UnitData
{
    public string description;
    public int knowledgeLevel;
    public int expReward;
    public int moneyReward;
    //public Map<float, Item>() lootTable;
}
