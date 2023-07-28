using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurnHandler
{

    public string activeCombatant;
    public string Type;
    public GameObject combtantGO;
    public GameObject target;
    public List<GameObject> targets = new List<GameObject>();

    public BaseAttack chosenAttack;

    public int priority;

}
