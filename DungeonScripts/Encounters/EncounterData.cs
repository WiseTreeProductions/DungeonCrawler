using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterData : MonoBehaviour
{
    public string encounterName;
    public int maxEnemyCount = 10;
    public string battleScreen;
    public List<GameObject> possibleEnemies = new List<GameObject>();
}
