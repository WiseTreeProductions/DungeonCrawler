using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateManager : UnitStateManager
{

    // Start is called before the first frame update
    void Start()
    {
        currentState = TurnState.PROCESSING;
        combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        battleEffects = GameObject.Find("BattleEffects");
        //Sprite sp = Resources.Load("Resources/death.png") as Sprite;
        //Debug.Log(sp);
        currentCooldown = Random.Range(0, 30f);
        SetPosition();
        //this.gameObject.transform.position = positionMarker.transform.position;
        CreateCharacterCard();
        UpdateCharacterCard();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                {
                    if (combatManager.heroInput == CombatManager.HeroGUI.ACTIVATE && combatManager.battleState == CombatManager.State.WAIT)
                    {
                        UpdateProgressBar();
                    }
                    break;
                }
            case (TurnState.CHOOSEACTION):
                {
                    ProcTurnStartStatuses();
                    UpdateStatValues();
                    UpdateDebuffDurations();
                    if (currentState == TurnState.DEAD)
                    {
                        break;
                    }
                    ChooseAction();
                    if (currentState != TurnState.CASTING)
                    {
                        currentState = TurnState.WAITING;
                    }
                    break;
                }
            case (TurnState.WAITING):
                {
                    break;
                }
            case (TurnState.SELECTING):
                {
                    break;
                }
            case (TurnState.ACTION):
                {
                    StartCoroutine(TimeForAction());
                    break;
                }
            case (TurnState.CASTING):
                {
                    if (combatManager.heroInput == CombatManager.HeroGUI.ACTIVATE && combatManager.battleState == CombatManager.State.WAIT)
                    {
                        UpdateCastBar();
                    }
                    break;
                }
            case (TurnState.DEAD):
                {
                    if (!alive)
                    {
                        return;
                    }
                    else
                    {
                        this.gameObject.tag = "DeadEnemy";
                        combatManager.enemiesOnField.Remove(this.gameObject);
                        this.transform.Find("Targeter").gameObject.SetActive(false);
                        this.transform.Find("DeadMarker").gameObject.SetActive(true);
                        this.transform.Find("Sprite").gameObject.SetActive(false);

                        if (combatManager.enemiesOnField.Count > 0)
                        {
                            for (int i = 0; i < combatManager.performList.Count; i++)
                            {
                                if (i != 0)
                                {
                                    if (combatManager.performList[i].combtantGO == this.gameObject)
                                    {
                                        combatManager.performList.Remove(combatManager.performList[i]);
                                    }
                                    if (combatManager.performList[i].target == this.gameObject)
                                    {
                                        combatManager.performList[i].target = combatManager.enemiesOnField[Random.Range(0, combatManager.enemiesOnField.Count)];
                                    }
                                }
                            }
                        }

                        alive = false;

                        combatManager.battleState = CombatManager.State.CHECKIFALIVE;
                    }
                    break;
                }

        }
    }

    #region AI shit
    //All the enemy AI stuff goes in here

    protected virtual void ChooseAction()
    {
        TurnHandler combatantsAction = new TurnHandler();

        List<GameObject> viableTargetsForAllAttacks = new List<GameObject>();
        List<GameObject> viableTargetsForMidAndLong = new List<GameObject>();
        List<GameObject> viableTargetsForLongRange = new List<GameObject>();

        List<GameObject> viableTargets = new List<GameObject>();

        for (int i = 0; i < combatManager.playerCharactersOnField.Count; i++)
        {
            viableTargets.Add(combatManager.playerCharactersOnField[i]);
        }

        LookForViableTargets(viableTargetsForAllAttacks, viableTargetsForMidAndLong, viableTargetsForLongRange);

        if (combatManager.castingList.Count > 0)
        {
            for (int i = 0; i < combatManager.castingList.Count; i++)
            {
                if (this.gameObject == combatManager.castingList[i].combtantGO)
                {
                    combatantsAction = combatManager.castingList[i];
                    /*
                    for (int j = 0; j < combatManager.playerCharactersOnField.Count; j++)
                    {
                        if (combatantsAction.chosenAttack.minDamage + this.enemy.damage >= combatManager.playerCharactersOnField[j].GetComponent<CharacterStateManager>().playerCharacter.currentHP)
                        {
                            combatantsAction.target = combatManager.playerCharactersOnField[j];
                        }
                    }
                    */
                    
                    if (combatantsAction.target == null)
                    {
                        combatantsAction.target = PickTargetForChosenAttack(combatantsAction, viableTargetsForAllAttacks, viableTargetsForMidAndLong, viableTargetsForLongRange);
                    }

                    //Debug.Log(this.gameObject.name + "has chosen " + combatantsAction.chosenAttack.attackName + " and did " + (combatantsAction.chosenAttack.damage + enemy.damage) + " damage");
                    battleEffects.GetComponent<SpriteRenderer>().sprite = combatantsAction.chosenAttack.battleEffect;
                    combatManager.CollectActions(combatantsAction);
                    combatManager.castingList.RemoveAt(i);
                    return;
                }
            }
        }
        
        combatantsAction.activeCombatant = combatUnit.entityName;
        combatantsAction.Type = "Enemy";
        combatantsAction.combtantGO = this.gameObject;

        List<BaseAttack> usableAttacks = new List<BaseAttack>();
        prepareUsableAttacks(usableAttacks);

        //lookForTargetToKill(usableAttacks, combatantsAction);

        if (combatantsAction.chosenAttack != null)
        {
            battleEffects.GetComponent<SpriteRenderer>().sprite = combatantsAction.chosenAttack.battleEffect;
            combatManager.CollectActions(combatantsAction);
            return;
        }

        int randomNumber = Random.Range(0, usableAttacks.Count);
        combatantsAction.chosenAttack = usableAttacks[randomNumber];

        if (combatantsAction.chosenAttack.castTime > 0)
        {
            combatManager.castingList.Add(combatantsAction);
            InitiateSpellCasting();
            Debug.Log(currentState);
            return;
        }

        /*
        for (int i = 0; i < viableTargetsForShortRange.Count; i++)
        {
            Debug.Log(viableTargetsForShortRange[i].GetComponent<CharacterStateManager>().playerCharacter.displayName);
        }
        */

        combatantsAction.target = PickTargetForChosenAttack(combatantsAction, viableTargetsForAllAttacks, viableTargetsForMidAndLong, viableTargetsForLongRange);

        //Debug.Log(this.gameObject.name + "has chosen " + combatantsAction.chosenAttack.attackName + " and did " + (combatantsAction.chosenAttack.damage + enemy.damage) + " damage");
        battleEffects.GetComponent<SpriteRenderer>().sprite = combatantsAction.chosenAttack.battleEffect;

        //Debug.Log(combatantsAction.target.GetComponent<CharacterStateManager>().playerCharacter.displayName);
        //Debug.Log(combatantsAction.chosenAttack.attackName);


        combatManager.CollectActions(combatantsAction);
    }

    public void prepareUsableAttacks(List<BaseAttack> usableAttacks)
    {
        for (int i = 0; i < combatUnit.attacks.Count; i++)
        {
            if (combatUnit.attacks[i].manaCost <= combatUnit.currentMP)
            {
                usableAttacks.Add(combatUnit.attacks[i]);
            }
        }
    }

    public bool IsUsableOnTarget(BaseAttack chosenAttack, GameObject target)
    {
        if (target.GetComponent<CharacterStateManager>() != null)
        {
            if (chosenAttack.range == BaseAttack.Range.Short)
            {
                if (positionMarker.row == 1 && target.GetComponent<CharacterStateManager>().positionMarker.row == 1)
                {
                    return true;
                }
            }

            if (chosenAttack.range == BaseAttack.Range.Medium)
            {
                if (positionMarker.row == 1)
                {
                    return true;
                }
                else if (positionMarker.row == 2 && target.GetComponent<CharacterStateManager>().positionMarker.row == 1)
                {
                    return true;
                }
            }

            if (chosenAttack.range == BaseAttack.Range.Long)
            {
                return true;
            }
        } 
        else if (target.GetComponent<EnemyStateManager>() != null)
        {
            return true;
        }

        return false;
    }

    public bool IsPlayerFirstRowEmpty()
    {
        for (int i = 0; i < combatManager.playerCharactersOnField.Count; i++)
        {
            if (combatManager.playerCharactersOnField[i].gameObject.tag == "Player" && combatManager.playerCharactersOnField[i].GetComponent<CharacterStateManager>().positionMarker.row == 1)
            {
                return false;
            }
        }

        return true;
    }

    public void LookForViableTargets(List<GameObject> viableTargetsForAllAttacks, List<GameObject> viableTargetsForMidAndLong, List<GameObject> viableTargetsForLongRange)
    {
        if (positionMarker.row == 1)
        {
            for (int i = 0; i < combatManager.playerCharactersOnField.Count; i++)
            {
                if (combatManager.playerCharactersOnField[i].GetComponent<CharacterStateManager>().positionMarker.row == 1 || IsPlayerFirstRowEmpty())
                {
                    viableTargetsForAllAttacks.Add(combatManager.playerCharactersOnField[i]);
                    viableTargetsForMidAndLong.Add(combatManager.playerCharactersOnField[i]);
                    viableTargetsForLongRange.Add(combatManager.playerCharactersOnField[i]);
                }
                if (combatManager.playerCharactersOnField[i].GetComponent<CharacterStateManager>().positionMarker.row == 2)
                {
                    viableTargetsForMidAndLong.Add(combatManager.playerCharactersOnField[i]);
                    viableTargetsForLongRange.Add(combatManager.playerCharactersOnField[i]);
                }

                
            }
        }
        else if (positionMarker.row == 2)
        {
            for (int i = 0; i < combatManager.playerCharactersOnField.Count; i++)
            {
                if (combatManager.playerCharactersOnField[i].GetComponent<CharacterStateManager>().positionMarker.row == 1 || IsPlayerFirstRowEmpty())
                {
                    viableTargetsForMidAndLong.Add(combatManager.playerCharactersOnField[i]);
                    viableTargetsForLongRange.Add(combatManager.playerCharactersOnField[i]);
                }
                if (combatManager.playerCharactersOnField[i].GetComponent<CharacterStateManager>().positionMarker.row == 2)
                {
                    viableTargetsForLongRange.Add(combatManager.playerCharactersOnField[i]);
                }
            }
        }
    }

    public GameObject PickTargetForChosenAttack(TurnHandler combatantsAction, List<GameObject> viableTargetsForShortRange, List<GameObject> viableTargetsForMidRange, List<GameObject> viableTargetsForLongRange)
    {
        if (combatantsAction.chosenAttack.range == BaseAttack.Range.Short)
        {
            combatantsAction.target = viableTargetsForShortRange[Random.Range(0, viableTargetsForShortRange.Count)];
        }
        else if (combatantsAction.chosenAttack.range == BaseAttack.Range.Medium)
        {
            combatantsAction.target = viableTargetsForMidRange[Random.Range(0, viableTargetsForMidRange.Count)];
        }
        else
        {
            combatantsAction.target = viableTargetsForLongRange[Random.Range(0, viableTargetsForLongRange.Count)];
        }

        return combatantsAction.target;
    }


    public void lookForTargetToKill(List<BaseAttack> usableAttacks, TurnHandler combatantsAction)
    {
        for (int i = 0; i < usableAttacks.Count; i++)
        {
            for (int j = 0; j < combatManager.playerCharactersOnField.Count; j++)
            {
                if (usableAttacks[i].castTime == 0 && usableAttacks[i].minDamage + this.combatUnit.damage >= combatManager.playerCharactersOnField[j].GetComponent<CharacterStateManager>().combatUnit.currentHP)
                {
                    combatantsAction.chosenAttack = usableAttacks[i];
                    combatantsAction.target = combatManager.playerCharactersOnField[j];
                }
            }
        }
    }

    #endregion



    void CreateCharacterCard()
    {
        characterCard = Instantiate(characterCard, GameObject.Find("CombatUI").transform, false) as GameObject;
        //enemyCard.transform.position = this.gameObject.transform.position;
        characterCard.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 1, this.gameObject.transform.position.z);


        stats = characterCard.GetComponent<CharacterPanelStats>();
        stats.characterName.text = combatUnit.displayName;
        stats.characterHP.text = combatUnit.currentHP.ToString();
        stats.characterMP.text = combatUnit.currentMP.ToString();

        progressBar = stats.progressBar;
        castBar = stats.castBar;
    }

}
