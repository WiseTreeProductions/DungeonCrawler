using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public enum State 
    { 
        WAIT, 
        TAKEACTION, 
        PERFORMACTION,
        CHECKIFALIVE,
        WIN,
        LOSE
    }

    public State battleState;

    public enum HeroGUI
    {
        ACTIVATE,
        WAITING,
        ATTACK,
        SELECTENEMY,
        DONE
    }

    public HeroGUI heroInput;


    public Text logText;

    public List<TurnHandler> performList = new List<TurnHandler>();
    public List<TurnHandler> castingList = new List<TurnHandler>();
    public List<GameObject> playerCharactersOnField = new List<GameObject>();
    public List<GameObject> enemiesOnField = new List<GameObject>();
    public List<GameObject> heroesToManage = new List<GameObject>();

    private TurnHandler heroChoice;

    private List<GameObject> combatants = new List<GameObject>();

    private State state;

    public Transform spacer;

    public GameObject actionPanel;
    public GameObject spellPanel;

    public Transform actionSpacer;
    public Transform spellSpacer;

    public GameObject actionButton;
    public GameObject magicButton;

    private List<GameObject> actionButtons = new List<GameObject>();

    private List<GameObject> enemyButtons = new List<GameObject>();

    public List<Transform> spawnPoints = new List<Transform>();

    public GameObject infoPanel;
    public Text damageText;
    public GameObject tooltipPanel;
    public GameObject enemyInfoPanel;

    private bool targetting = false;
    private bool selectingPosition = false;

    public GameObject playerPositions;
    public GameObject enemyPositions;

    void Awake()
    {
        /*
        for (int i = 0; i < GameManager.manager.enemyCount; i++)
        {
            GameObject newEnemy = Instantiate(GameManager.manager.encounterEnemies[i], spawnPoints[i].position, Quaternion.identity) as GameObject;
            newEnemy.name = newEnemy.GetComponent<EnemyStateManager>().enemy.entityName + "_" + i + 1;
            newEnemy.GetComponent<EnemyStateManager>().enemy.entityName = newEnemy.name;
            enemiesOnField.Add(newEnemy);
        }
        */
    }


    // Start is called before the first frame update

    void Start()
    {
        //state = State.Start;
        battleState = State.WAIT;
        logText.text = logText.text + "COMBAT STARTS";

        enemiesOnField.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        playerCharactersOnField.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        heroInput = HeroGUI.ACTIVATE;

        actionPanel.SetActive(false);
        spellPanel.SetActive(false);

        DisableTargetting();

        /*
        GameObject[] playerCharacters = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(playerCharacters.Length);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(enemies.Length);

        for (int i = 0; i < playerCharacters.Length; i++)
        {
            combatants.Add(playerCharacters[i]);
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            combatants.Add(enemies[i]);
        }

        Debug.Log(combatants.Count);

        //state = State.Waiting;
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*
        while (state == State.Waiting)
        {
            combatants[0].GetComponent<PlayerEntity>().initiative++;
            Debug.Log(combatants[0].GetComponent<Entity>().initiative);

            if (combatants[0].GetComponent<PlayerEntity>().initiative >= 1000)
            {
                combatants[0].GetComponent<Entity>().initiative = 1000;
                Debug.Log(combatants[0].GetComponent<Entity>().initiative);
                state = State.PlayerTurn;
                logText.text = logText.text + "Player's turn";
                break;
            }
        }
        */

        switch (battleState)
        {
            case (State.WAIT):
                {
                    if(performList.Count > 0)
                    {
                        battleState = State.TAKEACTION;
                    }
                    break;
                }
            case (State.TAKEACTION):
                {
                    GameObject performer = GameObject.Find(performList[0].activeCombatant);
                    if (performList[0].Type == "Enemy")
                    {
                        EnemyStateManager ESM = performer.GetComponent<EnemyStateManager>();
                        Debug.Log(playerCharactersOnField.Count);

                        for (int i = 0; i < playerCharactersOnField.Count; i++)
                        {
                            if (performList[0].target == playerCharactersOnField[i])
                            {
                                ESM.target = performList[0].target;
                                ESM.currentState = EnemyStateManager.TurnState.ACTION;
                                break;
                            }
                            else if (i == playerCharactersOnField.Count - 1)
                            {
                                List<GameObject> viableTargetsForAllAttacks = new List<GameObject>();
                                List<GameObject> viableTargetsForMidAndLong = new List<GameObject>();
                                List<GameObject> viableTargetsForLongRange = new List<GameObject>();

                                ESM.LookForViableTargets(viableTargetsForAllAttacks, viableTargetsForMidAndLong, viableTargetsForLongRange);

                                //performList[0].target = playerCharactersOnField[Random.Range(0, playerCharactersOnField.Count)];
                                performList[0].target = ESM.PickTargetForChosenAttack(performList[0], viableTargetsForAllAttacks, viableTargetsForMidAndLong, viableTargetsForLongRange);
                                ESM.target = performList[0].target;
                                ESM.currentState = EnemyStateManager.TurnState.ACTION;
                            }
                        } 
                    } 
                    else if (performList[0].Type == "Hero")
                    {
                        CharacterStateManager CSM = performer.GetComponent<CharacterStateManager>();
                        CSM.target = performList[0].target;
                        CSM.currentState = CharacterStateManager.TurnState.ACTION;
                    }

                    battleState = State.PERFORMACTION;

                    break;
                }
            case (State.PERFORMACTION):
                {
                    break;
                }
            case (State.CHECKIFALIVE):
                {
                    if (playerCharactersOnField.Count < 1)
                    {
                        battleState = State.LOSE;
                    }
                    else if (enemiesOnField.Count < 1)
                    {
                        battleState = State.WIN;
                    }
                    else
                    {
                        clearActionPanel();
                        heroInput = HeroGUI.ACTIVATE;
                        battleState = State.WAIT;
                    }
                    break;
                }
            case (State.WIN):
                {
                    logText.text = logText.text + "\nYour party is victorious!";
                    for (int i = 0; i < playerCharactersOnField.Count; i++)
                    {
                        playerCharactersOnField[i].GetComponent<CharacterStateManager>().currentState = CharacterStateManager.TurnState.WAITING;
                    }

                    GameManager.manager.LoadSceneAfterBattle();
                    GameManager.manager.gameState = GameManager.GameStates.INTOWN;
                    GameManager.manager.encounterEnemies.Clear();

                    break;
                }
            case (State.LOSE):
                {
                    logText.text = logText.text + "\nYour party has been defeated!";
                    break;
                }
        }

        switch (heroInput)
        {
            case (HeroGUI.ACTIVATE):
                {
                    if (heroesToManage.Count > 0)
                    {
                        heroesToManage[0].GetComponent<CharacterStateManager>().characterCard.transform.Find("ActiveTag").GetComponent<Text>().enabled = true;
                           
                        heroChoice = new TurnHandler();

                        actionPanel.SetActive(true);
                        CreateAttackButtons();

                        for (int i = 0; i < castingList.Count; i++)
                        {
                            if (heroesToManage[0] == castingList[i].combtantGO)
                            {
                                //Debug.Log("Caster found");
                                UseCastedSpell(i);
                                actionPanel.SetActive(false);
                                heroInput = HeroGUI.WAITING;
                            }
                        }

                        heroInput = HeroGUI.WAITING;
                    }
                    break;
                }
            case (HeroGUI.WAITING):
                {
                    break;
                }
            case (HeroGUI.ATTACK):
                {
                    break;
                }
            case (HeroGUI.SELECTENEMY):
                {
                    break;
                }
            case (HeroGUI.DONE):
                {
                    HeroInputDone();
                    break;
                }
        }
    }

    public void CollectActions(TurnHandler input)
    {
        performList.Add(input);
    }

    public void DisableTargetting()
    {
        foreach (GameObject enemy in enemiesOnField)
        {
            enemy.transform.Find("Sprite").GetComponent<Button>().enabled = false;
            enemy.transform.Find("Sprite").GetComponent<EventTrigger>().enabled = false;
            enemy.transform.Find("Targeter").gameObject.SetActive(false);
        }
        foreach (GameObject player in playerCharactersOnField)
        {
            player.transform.parent.GetComponent<Button>().enabled = false;
            player.transform.parent.GetComponent<EventTrigger>().enabled = false;
        }
        infoPanel.transform.Find("Text").GetComponent<Text>().enabled = false;

        targetting = false;

    }

    public bool IsEnemyFirstRowEmpty()
    {
        for (int i = 0; i < enemiesOnField.Count; i++)
        {
            if (enemiesOnField[i].gameObject.tag == "Enemy" && enemiesOnField[i].GetComponent<EnemyStateManager>().positionMarker.row == 1)
            {
                return false;
            }
        }

        return true;
    }

    public void EnableTargetting()
    {
        if (heroChoice.chosenAttack.range == BaseAttack.Range.Short)
        {
            if (heroChoice.combtantGO.GetComponent<CharacterStateManager>().positionMarker.row == 1)
            {
                if (IsEnemyFirstRowEmpty())
                {
                    foreach (GameObject enemy in enemiesOnField)
                    {
                        enemy.transform.Find("Sprite").GetComponent<Button>().enabled = true;
                        enemy.transform.Find("Sprite").GetComponent<EventTrigger>().enabled = true;
                    }
                }
                else
                {
                    foreach (GameObject enemy in enemiesOnField)
                    {
                        if (enemy.GetComponent<EnemyStateManager>().positionMarker.row == 1)
                        {
                            enemy.transform.Find("Sprite").GetComponent<Button>().enabled = true;
                            enemy.transform.Find("Sprite").GetComponent<EventTrigger>().enabled = true;
                        }
                    }
                }
            }
        }
        else if (heroChoice.chosenAttack.range == BaseAttack.Range.Medium)
        {
            if (heroChoice.combtantGO.GetComponent<CharacterStateManager>().positionMarker.row == 1)
            {
                foreach (GameObject enemy in enemiesOnField)
                {
                    enemy.transform.Find("Sprite").GetComponent<Button>().enabled = true;
                    enemy.transform.Find("Sprite").GetComponent<EventTrigger>().enabled = true;
                }
            }
            else
            {
                if (IsEnemyFirstRowEmpty())
                {
                    foreach (GameObject enemy in enemiesOnField)
                    {
                        enemy.transform.Find("Sprite").GetComponent<Button>().enabled = true;
                        enemy.transform.Find("Sprite").GetComponent<EventTrigger>().enabled = true;
                    }
                }
                else
                {
                    foreach (GameObject enemy in enemiesOnField)
                    {
                        if (enemy.GetComponent<EnemyStateManager>().positionMarker.row == 1)
                        {
                            enemy.transform.Find("Sprite").GetComponent<Button>().enabled = true;
                            enemy.transform.Find("Sprite").GetComponent<EventTrigger>().enabled = true;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (GameObject enemy in enemiesOnField)
            {
                enemy.transform.Find("Sprite").GetComponent<Button>().enabled = true;
                enemy.transform.Find("Sprite").GetComponent<EventTrigger>().enabled = true;
            }
        }

        foreach (GameObject player in playerCharactersOnField)
        {
            player.transform.parent.GetComponent<Button>().enabled = true;
            player.transform.parent.GetComponent<EventTrigger>().enabled = true;
        }

        infoPanel.transform.Find("Text").GetComponent<Text>().enabled = true;
        infoPanel.transform.Find("Text").GetComponent<Text>().text = "Select target";

        targetting = true;

    }

    public void Attack()
    {
        heroChoice.activeCombatant = heroesToManage[0].name;
        heroChoice.combtantGO = heroesToManage[0];
        heroChoice.Type = "Hero";
        heroChoice.chosenAttack = heroesToManage[0].GetComponent<CharacterStateManager>().combatUnit.attacks[0];

        actionPanel.SetActive(false);
        EnableTargetting();
    }

    public void OpenSpells()
    {
        actionPanel.SetActive(false);
        spellPanel.SetActive(true);
    }

    public void Wait()
    {
        heroChoice.activeCombatant = heroesToManage[0].name;
        heroChoice.combtantGO = heroesToManage[0];
        heroChoice.Type = "Hero";
        heroChoice.chosenAttack = heroesToManage[0].GetComponent<CharacterStateManager>().combatUnit.Actions[0];

        actionPanel.SetActive(false);

        HeroInputDone();
    }

    public void InitiateSwitchPosition()
    {
        for (int i = 0; i < heroesToManage[0].GetComponent<CharacterStateManager>().positionMarker.adjacentPositions.Count; i++)
        {
            heroesToManage[0].GetComponent<CharacterStateManager>().positionMarker.adjacentPositions[i].GetComponent<Image>().enabled = true;
        }
        Debug.Log("SWITCHING");

        actionPanel.SetActive(false);
        selectingPosition = true;
    }

    public void CancelPositionSelect()
    {
        for (int i = 0; i < heroesToManage[0].GetComponent<CharacterStateManager>().positionMarker.adjacentPositions.Count; i++)
        {
            heroesToManage[0].GetComponent<CharacterStateManager>().positionMarker.adjacentPositions[i].GetComponent<Image>().enabled = false;
        }


        actionPanel.SetActive(true);
        selectingPosition = false;
    }

    public void ChangePosition(Position targetPosition)
    {
        GameObject heroToSwitch = heroesToManage[0];

        if (targetPosition.occupied == false)
        {
            Debug.Log("SWITCHING");
            CancelPositionSelect();
            targetPosition.occupied = false;
            heroToSwitch.GetComponent<CharacterStateManager>().positionMarker = targetPosition;
            heroToSwitch.GetComponent<CharacterStateManager>().SetPosition();

            heroChoice.activeCombatant = heroesToManage[0].name;
            heroChoice.combtantGO = heroesToManage[0];
            heroChoice.Type = "Hero";
            heroChoice.chosenAttack = heroesToManage[0].GetComponent<CharacterStateManager>().combatUnit.Actions[1];

            actionPanel.SetActive(false);

            HeroInputDone();
        }
        else
        {
            CancelPositionSelect();
            targetPosition.characterInSlot.GetComponent<CharacterStateManager>().positionMarker = heroToSwitch.GetComponent<CharacterStateManager>().positionMarker;
            targetPosition.characterInSlot.GetComponent<CharacterStateManager>().characterCard.transform.position = heroesToManage[0].GetComponent<CharacterStateManager>().positionMarker.transform.position;
            targetPosition.characterInSlot.transform.position = targetPosition.characterInSlot.GetComponent<CharacterStateManager>().characterCard.transform.position;


            heroToSwitch.GetComponent<CharacterStateManager>().positionMarker = targetPosition;
            heroToSwitch.GetComponent<CharacterStateManager>().SetPosition();

            heroChoice.activeCombatant = heroesToManage[0].name;
            heroChoice.combtantGO = heroesToManage[0];
            heroChoice.Type = "Hero";
            heroChoice.chosenAttack = heroesToManage[0].GetComponent<CharacterStateManager>().combatUnit.Actions[1];

            actionPanel.SetActive(false);

            HeroInputDone();
        }
    }

    public void CastSpell(BaseAttack chosenSpell)
    {
        if (chosenSpell.manaCost > heroesToManage[0].GetComponent<CharacterStateManager>().combatUnit.currentMP)
        {
            logText.text = logText.text + "\n NO MANA BigBrother";
        }
        else if (chosenSpell.castTime == 0)
        {
            heroChoice.activeCombatant = heroesToManage[0].name;
            heroChoice.combtantGO = heroesToManage[0];
            heroChoice.Type = "Hero";

            heroChoice.chosenAttack = chosenSpell;

            spellPanel.SetActive(false);
            EnableTargetting();
        }
        else
        {
            heroChoice.activeCombatant = heroesToManage[0].name;
            heroChoice.combtantGO = heroesToManage[0];
            heroChoice.Type = "Hero";
            heroChoice.chosenAttack = chosenSpell;

            spellPanel.SetActive(false);

            castingList.Add(heroChoice);
            heroChoice.combtantGO.GetComponent<CharacterStateManager>().InitiateSpellCasting();

            clearActionPanel();

            heroesToManage[0].GetComponent<CharacterStateManager>().characterCard.transform.Find("ActiveTag").GetComponent<Text>().enabled = false;
            heroesToManage.RemoveAt(0);
            heroInput = HeroGUI.ACTIVATE;
            //EnableTargetting();
        }

    }

    public void UseCastedSpell(int index)
    {
        heroChoice.activeCombatant = castingList[index].activeCombatant;
        heroChoice.combtantGO = castingList[index].combtantGO;
        heroChoice.Type = "Hero";
        heroChoice.chosenAttack = castingList[index].chosenAttack;
        castingList.RemoveAt(index);
        EnableTargetting();
    }


    public void SelectTarget(GameObject selectedEnemy)
    {
        heroChoice.target = selectedEnemy;
        heroInput = HeroGUI.DONE;
    }

    void HeroInputDone()
    {
        performList.Add(heroChoice);

        clearActionPanel();

        heroesToManage[0].GetComponent<CharacterStateManager>().characterCard.transform.Find("ActiveTag").GetComponent<Text>().enabled = false;
        heroesToManage.RemoveAt(0);
        heroInput = HeroGUI.ACTIVATE;
        DisableTargetting();
    }

    void clearActionPanel()
    {
        actionPanel.SetActive(false);
        spellPanel.SetActive(false);

        foreach (GameObject actBtn in actionButtons)
        {
            Destroy(actBtn);
        }

        actionButtons.Clear();
    }

    void CreateAttackButtons()
    {
        GameObject attackButton = Instantiate(actionButton) as GameObject;
        Text attackButtonText = attackButton.transform.Find("Text").GetComponent<Text>();
        attackButtonText.text = "Attack";
        attackButton.GetComponent<Button>().onClick.AddListener(() => Attack());
        attackButton.transform.SetParent(actionSpacer, false);
        attackButton.transform.SetSiblingIndex(0);
        actionButtons.Add(attackButton);

        GameObject spellButton = Instantiate(actionButton) as GameObject;
        Text spellButtonText = spellButton.transform.Find("Text").GetComponent<Text>();
        spellButtonText.text = "Spell";
        spellButton.GetComponent<Button>().onClick.AddListener(() => OpenSpells());
        spellButton.transform.SetParent(actionSpacer, false);
        spellButton.transform.SetSiblingIndex(1);
        actionButtons.Add(spellButton);

        GameObject waitButton = Instantiate(actionButton) as GameObject;
        Text waitButtonText = waitButton.transform.Find("Text").GetComponent<Text>();
        waitButtonText.text = "Wait";
        waitButton.GetComponent<Button>().onClick.AddListener(() => Wait());
        waitButton.transform.SetParent(actionSpacer, false);
        waitButton.transform.SetSiblingIndex(2);
        actionButtons.Add(waitButton);

        GameObject switchPositionButton = Instantiate(actionButton) as GameObject;
        Text switchPositionButtonText = switchPositionButton.transform.Find("Text").GetComponent<Text>();
        switchPositionButtonText.text = "Switch Position";
        switchPositionButton.GetComponent<Button>().onClick.AddListener(() => InitiateSwitchPosition());
        switchPositionButton.transform.SetParent(actionSpacer, false);
        switchPositionButton.transform.SetSiblingIndex(3);
        actionButtons.Add(switchPositionButton);

        if (heroesToManage[0].GetComponent<CharacterStateManager>().combatUnit.Spells.Count > 0)
        {
            foreach (BaseAttack spell in heroesToManage[0].GetComponent<CharacterStateManager>().combatUnit.Spells)
            {
                GameObject MagicButton = Instantiate(magicButton) as GameObject;
                Text magicButtonText = MagicButton.transform.Find("Text").GetComponent<Text>();

                magicButtonText.text = spell.attackName;
                AttackButton ATB = MagicButton.GetComponent<AttackButton>();
                ATB.spellToCast = spell;
                MagicButton.transform.SetParent(spellSpacer, false);
                actionButtons.Add(MagicButton);

            }
        }
        else
        {
            spellButton.GetComponent<Button>().interactable = false;
        }

        /*

        GameObject defendButton = Instantiate(actionButton) as GameObject;
        Text defendButtonText = defendButton.transform.Find("Text").GetComponent<Text>();
        defendButtonText.text = "Defend";
        //spellButton.GetComponent<Button>().onClick.AddListener(() => Attack());
        defendButton.transform.SetParent(actionSpacer, false);
        defendButton.transform.SetSiblingIndex(2);
        actionButtons.Add(defendButton);



        GameObject itemButton = Instantiate(actionButton) as GameObject;
        Text itemButtonText = itemButton.transform.Find("Text").GetComponent<Text>();
        itemButtonText.text = "Use Item";
        //spellButton.GetComponent<Button>().onClick.AddListener(() => Attack());
        itemButton.transform.SetParent(actionSpacer, false);
        itemButton.transform.SetSiblingIndex(4);
        actionButtons.Add(itemButton);

    */
    }

    public void PopulateInfoPanel(GameObject enemy)
    {
        enemyInfoPanel.transform.Find("Holder").gameObject.SetActive(true);
        enemyInfoPanel.transform.Find("Holder").Find("NamePanel").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.displayName;
        enemyInfoPanel.transform.Find("Holder").Find("HPPanel").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.currentHP +"/" + enemy.GetComponent<EnemyStateManager>().combatUnit.maxHP;
        enemyInfoPanel.transform.Find("Holder").Find("MPPanel").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.currentMP + "/" + enemy.GetComponent<EnemyStateManager>().combatUnit.maxMP;

        enemyInfoPanel.transform.Find("Holder").Find("DefenseBlock").Find("PhysicalDefenseText").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.physicalDefense.ToString();
        enemyInfoPanel.transform.Find("Holder").Find("DefenseBlock").Find("MagicalDefenseText").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.magicalDefense.ToString();
        enemyInfoPanel.transform.Find("Holder").Find("DefenseBlock").Find("EvasionText").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.evasion.ToString();

        enemyInfoPanel.transform.Find("Holder").Find("ResistanceBlock").Find("FireResistancePanel").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.fireResistance.ToString();
        enemyInfoPanel.transform.Find("Holder").Find("ResistanceBlock").Find("ColdResistancePanel").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.coldResistance.ToString();
        enemyInfoPanel.transform.Find("Holder").Find("ResistanceBlock").Find("LightningResistancePanel").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.lightningResistance.ToString();
        enemyInfoPanel.transform.Find("Holder").Find("ResistanceBlock").Find("EarthResistancePanel").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.earthResistance.ToString();
        enemyInfoPanel.transform.Find("Holder").Find("ResistanceBlock").Find("LightResistancePanel").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.lightResistance.ToString();
        enemyInfoPanel.transform.Find("Holder").Find("ResistanceBlock").Find("DarkResistancePanel").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.darkResistance.ToString();
        enemyInfoPanel.transform.Find("Holder").Find("ResistanceBlock").Find("ForceResistancePanel").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.forceResistance.ToString();
        enemyInfoPanel.transform.Find("Holder").Find("ResistanceBlock").Find("PhysicalResistancePanel").Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStateManager>().combatUnit.physicalResistance.ToString();
    }

    private void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetMouseButtonDown(1))
        {
            if (spellPanel.activeSelf == true)
            {
                spellPanel.SetActive(false);
                actionPanel.SetActive(true);
                tooltipPanel.transform.Find("Holder").gameObject.SetActive(false);
            }
            else if (targetting == true)
            {
                DisableTargetting();
                actionPanel.SetActive(true);
            }
            else if (selectingPosition == true)
            {
                CancelPositionSelect();
            }
        } 


    }


}
