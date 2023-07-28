using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStateManager : UnitStateManager
{

    private Transform characterPanelSpacer;

    // Start is called before the first frame update
    void Start()
    {

        characterPanelSpacer = GameObject.Find("CombatUI").transform.Find("CharacterPanel").transform.Find("CharacterPanelSpacer");

        CreateCharacterCard();

        currentCooldown = Random.Range(0, 30f);
        currentState = TurnState.PROCESSING;
        combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        battleEffects = GameObject.Find("BattleEffects");
        characterCard.transform.Find("ActiveTag").GetComponent<Text>().enabled = false;

        //this.gameObject.transform.position = characterCard.transform.position;
        //characterCard.transform.position = positionMarker.transform.position;

        //positionMarker.occupied = true;

        SetPosition();


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
            case (TurnState.ADDTOLIST):
                {
                    ProcTurnStartStatuses();
                    combatManager.heroesToManage.Add(this.gameObject);
                    currentState = TurnState.WAITING;
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
                        this.gameObject.tag = "DeadHero";

                        combatManager.playerCharactersOnField.Remove(this.gameObject);

                        combatManager.heroesToManage.Remove(this.gameObject);

                        combatManager.actionPanel.SetActive(false);

                        if (combatManager.playerCharactersOnField.Count > 0)
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
                                        combatManager.performList[i].target = combatManager.playerCharactersOnField[Random.Range(0, combatManager.playerCharactersOnField.Count)];
                                    }
                                }
                            }
                        }

                        combatManager.battleState = CombatManager.State.CHECKIFALIVE;

                        alive = false;
                    }

                    break;
                }

        }
    }


    void CreateCharacterCard()
    {
        characterCard = Instantiate(characterCard) as GameObject;
        stats = characterCard.GetComponent<CharacterPanelStats>();
        stats.characterName.text = combatUnit.displayName;
        stats.characterHP.text = combatUnit.currentHP.ToString();
        stats.characterMP.text = combatUnit.currentMP.ToString();

        progressBar = stats.progressBar;
        castBar = stats.castBar;

        characterCard.transform.SetParent(characterPanelSpacer, false);
        this.gameObject.transform.SetParent(characterCard.transform, false);
        characterCard.GetComponent<EnemySelectButton>().enemy = this.gameObject;

    }



}
