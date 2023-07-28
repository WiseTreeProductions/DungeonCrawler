using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStateManager : MonoBehaviour
{

    //TODO STUFF:
    //Need to figure out a way to correctly apply stat debuffs regardless of type of debuff
    //Maybe create a statchange status effect base type and make all status effect changes inherit it

    //Need to figure out a way to handle different types of status effect - maybe create a handler method for
    //each of them
    //Try creating a few different status effects to test

    //Find a better solution for handling units killing themselves
    
    //After that move on to AoE abilities

    public CombatManager combatManager;

    public Entity combatUnit;
    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        CASTING,
        DEAD
    }

    public TurnState currentState;

    protected float currentCooldown = 0f;
    protected float maxCooldown = 100f;
    protected float currentCastProgress = 0f;
    protected float maxCastProgress = 0f;
    public GameObject characterCard;
    public Position positionMarker;
    protected Image progressBar;
    protected Image castBar;
    public Text activeTag;
    public GameObject target;

    public GameObject battleEffects;
    protected CharacterPanelStats stats;

    protected bool alive = true;
    protected bool actionStarted = false;
    protected int turnsInCombat = 0;

    private float debuffCap = -50f;

    public List<BaseStatusEffect> currentEffects = new List<BaseStatusEffect>();

    public void SetPosition()
    {
        if (combatUnit.unitType == Entity.UnitType.Player)
        {
            this.gameObject.transform.position = characterCard.transform.position;
            characterCard.transform.position = positionMarker.transform.position;
        } 
        else
        {
            this.gameObject.transform.position = positionMarker.transform.position;
        }

        positionMarker.occupied = true;
        positionMarker.characterInSlot = this.gameObject;
    }

    public void UpdateProgressBar()
    {
        currentCooldown = currentCooldown + Time.deltaTime * combatUnit.speed;
        float progressToDisplay = currentCooldown / maxCooldown;
        progressBar.transform.localScale = new Vector3(Mathf.Clamp(progressToDisplay, 0, 1), progressBar.transform.localScale.y, progressBar.transform.localScale.z);
        stats.characterProgress.text = ((int)currentCooldown).ToString();
        if (currentCooldown >= maxCooldown)
        { 
            if (combatUnit.AI == Entity.AIType.Player)
            {
                currentState = TurnState.ADDTOLIST;
            }
            else
            {
                currentState = TurnState.CHOOSEACTION;
            }
        }
    }

    #region Spell casting mechanics

    //Start the process of casting a spell - add this unit to list of units that are casting spells
    //Set cast time to spell's cast time
    //Show cast bar in GUI
    public void InitiateSpellCasting()
    {
        for (int i = 0; i < combatManager.castingList.Count; i++)
        {
            if (combatManager.castingList[i].combtantGO == this.gameObject)
            {
                maxCastProgress = combatManager.castingList[i].chosenAttack.castTime;
                Debug.Log("max cast: " + maxCastProgress);
            }
        }

        characterCard.transform.Find("CastBarBG").gameObject.SetActive(true);
        characterCard.transform.Find("CastBar").gameObject.SetActive(true);
        characterCard.transform.Find("CastText").gameObject.SetActive(true);

        currentState = TurnState.CASTING;
    }

    //Updates the cast progress every tick, when casting finishes adds unit to list of units awaiting orders,
    //if the unit is player or makes it perform an action if it is AI
    public void UpdateCastBar()
    {
        currentCastProgress = currentCastProgress + Time.deltaTime * combatUnit.speed;
        float castProgressToDisplay = currentCastProgress / maxCastProgress;
        castBar.transform.localScale = new Vector3(Mathf.Clamp(castProgressToDisplay, 0, 1), castBar.transform.localScale.y, castBar.transform.localScale.z);
        stats.characterCastProgress.text = ((int)currentCastProgress).ToString();
        if (currentCastProgress >= maxCastProgress)
        {
            if (combatUnit.AI == Entity.AIType.Player)
            {
                currentState = TurnState.ADDTOLIST;
            }
            else
            {
                currentState = TurnState.CHOOSEACTION;
            }
            FinalizeSpellCasting();
        }
    }

    //Resets all cast timers for this unit and removes bar from GUI
    public void FinalizeSpellCasting()
    {
        characterCard.transform.Find("CastBarBG").gameObject.SetActive(false);
        characterCard.transform.Find("CastBar").gameObject.SetActive(false);
        characterCard.transform.Find("CastText").gameObject.SetActive(false);

        currentCastProgress = 0f;
        maxCastProgress = 0f;

    }

    //Interrupts casting that is in progress
    public void InterruptSpellCasting()
    {
        FinalizeSpellCasting();
        for (int i = 0; i < combatManager.castingList.Count; i++)
        {
            if (this.gameObject == combatManager.castingList[i].combtantGO)
            {
                combatManager.castingList.RemoveAt(i);
                break;
            }
        }
        currentCooldown = 70f;
        currentState = TurnState.PROCESSING;
    }

    #endregion

    public IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        //Animation plays for one second when turn bar is filled

        if (combatManager.performList[0].chosenAttack.targetable == true)
        {
            ShowEffect(target);

            yield return new WaitForSeconds(1);

            HideEffect();
        }

        Debug.Log("State before damage: " + currentState);

        PerformChosenAction();

        Debug.Log("State after damage: " + currentState);

        //Bandaid solution for handling death if unit kills itself
        if (currentState == TurnState.DEAD)
        {
            combatManager.performList.RemoveAt(0);
            actionStarted = false;
            combatManager.battleState = CombatManager.State.WAIT;
            currentCooldown = 0f;
            UpdateCharacterCard();
            yield break;
        }

        //Remove combatant from list of actions to be made
        combatManager.performList.RemoveAt(0);
        //Reset combat manager back to starting state so it can process other actions in list
        if (combatManager.battleState != CombatManager.State.WIN || combatManager.battleState != CombatManager.State.LOSE)
        {
            combatManager.battleState = CombatManager.State.WAIT;
            //Reset unit's state
            turnsInCombat++;
            currentState = TurnState.PROCESSING;
        }
        else
        {
            currentState = TurnState.WAITING;
        }

        actionStarted = false;

    }


    public void PerformChosenAction()
    {
        ActionHandler actionHandler = new ActionHandler();
        actionHandler.chosenAction = combatManager.performList[0].chosenAttack;

        combatUnit.currentMP = combatUnit.currentMP - combatManager.performList[0].chosenAttack.manaCost;
        if (combatManager.performList[0].chosenAttack.targetable == true)
        {
            if (combatManager.performList[0].chosenAttack.isOffensive == true)
            {
                if (CheckIfHit(combatManager.performList[0].chosenAttack))
                {
                    actionHandler.RollDamage();
                    actionHandler.unmitigatedDamage = actionHandler.unmitigatedDamage + combatUnit.damage;

                    actionHandler.applyMitigation(target);

                    DoDamage(actionHandler);
                    ApplyStatusEffects(actionHandler);
                    target.GetComponent<UnitStateManager>().UpdateStatValues();
                    Debug.Log("Target's def " + target.GetComponent<UnitStateManager>().combatUnit.physicalDefense);
                }
                else
                {
                    StartCoroutine(ShowActionResult("Miss"));
                }
            }
            else
            {
                actionHandler.RollHealing();
                actionHandler.unmitigatedHeaing = actionHandler.unmitigatedHeaing + combatUnit.damage;
                actionHandler.ApplyHealingModifiers(target);
                DoHealing(actionHandler);
                ApplyStatusEffects(actionHandler);
            }

            resetTurnTimer();
            UpdateCharacterCard();

        }

    }


    #region Battle mechanics

    //Checks if chosen attack passed the to-hit threshold on the target
    public bool CheckIfHit(BaseAttack chosenAttack)
    {
        float hitThreshold = 100f - chosenAttack.baseAccuracy - combatUnit.accuracy + target.GetComponent<UnitStateManager>().combatUnit.evasion;
        float hitRoll = Random.Range(0, 100);
        Debug.Log("Roll: " + hitRoll);
        Debug.Log("Threshold: " + hitThreshold);
        if (hitRoll >= hitThreshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Calls the target's takeDamage method to subtract the final caulcated damage from its HP
    public void DoDamage(ActionHandler actionHandler)
    {
        target.GetComponent<UnitStateManager>().TakeDamage(actionHandler.finalDamage);
    }


    //Substracts the final caulcated damage from this unit's HP
    //Shows how much damage was done
    //Interrupts spellcasting if unit was casting a spell
    //Updates visual information after damage was taken
    //Checks if unit is dead, if it is, calls appropriate method
    public void TakeDamage(float damageAmount)
    {
        //damageAmount = applyMitigation(damageAmount);
        combatUnit.currentHP -= damageAmount;
        StartCoroutine(ShowActionResult(damageAmount.ToString()));

        if (currentState == TurnState.CASTING)
        {
            InterruptSpellCasting();
        }

        UpdateCharacterCard();

        if (combatUnit.currentHP <= 0)
        {
            SetDeadStatus();
        }
    }

    //Sets the unit's status to dead, updates GUI
    public void SetDeadStatus()
    {
        combatUnit.currentHP = 0;
        currentState = TurnState.DEAD;

        ShowDeath();

    }


    //Calls the target's receiveHealing method to add calculated healing amount to HP
    public void DoHealing(ActionHandler actionHandler)
    {
        target.GetComponent<UnitStateManager>().ReceiveHealing(actionHandler.finalHealing);
    }


    //Adds the final caulcated healing to this unit's HP
    //Shows how much healing was done
    //If the healing would put the unit's HP above max, set it to max instead
    //Updates visual information after healing was done
    public void ReceiveHealing(float healingAmount)
    {
        Debug.Log("HEALING FOR " + healingAmount);
        combatUnit.currentHP += healingAmount;
        StartCoroutine(ShowActionResult(healingAmount.ToString()));
        if (combatUnit.currentHP > combatUnit.maxHP)
        {
            combatUnit.currentHP = combatUnit.maxHP;
        }
        UpdateCharacterCard();
    }

    //Adds the list of effects from the attack to this unit's list of current effects
    //For burn effects, calculates how much damage the burn will do after applying all mitigations
    public void ApplyStatusEffects(ActionHandler actionHandler)
    {
        Debug.Log("Number of effects of attack: " + combatManager.performList[0].chosenAttack.statusEffects.Count);
        for (int i = 0; i < combatManager.performList[0].chosenAttack.statusEffects.Count; i++)
        {
            if (combatManager.performList[0].chosenAttack.statusEffects[i].effectName == "Burn")
            {
                combatManager.performList[0].chosenAttack.statusEffects[i].damage = Mathf.RoundToInt(actionHandler.finalDamage * 0.2f);
                target.GetComponent<UnitStateManager>().currentEffects.Add(combatManager.performList[0].chosenAttack.statusEffects[i]);
            }
            else if (CheckEffectTag(BaseStatusEffect.Tag.StatChange, combatManager.performList[0].chosenAttack.statusEffects[i])) 
            {
                target.GetComponent<UnitStateManager>().ApplyStatDebuff(combatManager.performList[0].chosenAttack.statusEffects[i]);
            }
            else
            {
                target.GetComponent<UnitStateManager>().currentEffects.Add(combatManager.performList[0].chosenAttack.statusEffects[i]);
            }
        }
    }

    public void ApplyStatDebuff(BaseStatusEffect debuff)
    {
        Debug.Log(debuff.percentageModifier);
        foreach (BaseStatusEffect d in currentEffects)
        {
            if (debuff.targetStat.Equals(d.targetStat))
            {
                d.percentageModifier = d.percentageModifier + debuff.percentageModifier;
                if (d.percentageModifier < debuffCap)
                {
                    d.percentageModifier = debuffCap;
                }
                Debug.Log("Value of debuff " + debuff.percentageModifier);
                Debug.Log("Value of d " + d.percentageModifier);
                return;
            }
        }

        PhysicalDefDebuff test = new PhysicalDefDebuff(25);
        currentEffects.Add(test);
    }

    public bool CheckEffectTag(BaseStatusEffect.Tag tag, BaseStatusEffect effect)
    {
        foreach (BaseStatusEffect.Tag t in effect.tags)
        {
            if (tag == t)
            {
                return true;
            }
        }

        return false;
    }

    //Procs all effects that proc at the start of the turn like DoTs
    public void ProcTurnStartStatuses()
    {
        Debug.Log("Number of status effects: " + currentEffects.Count);
        for (int i = 0; i < currentEffects.Count; i++)
        {
            for (int j = 0; j < currentEffects[i].tags.Count; j++)
            {
                switch (currentEffects[i].tags[j])
                {
                    case (BaseStatusEffect.Tag.DoT):
                        {
                            TakeDamage(currentEffects[i].damage);
                            combatManager.logText.text = combatManager.logText.text + "\n" + currentEffects[i].effectName + " deals " + currentEffects[i].damage + " to " + this.combatUnit.entityName;
                            currentEffects[i].turnDuration--;
                            if (currentEffects[i].turnDuration == 0)
                            {
                                currentEffects.RemoveAt(i);
                            }
                            break;
                        }
                }
            }
        }
    }

    //After unit perform an attack, set its timer to the delay value of the attack
    public void resetTurnTimer()
    {
        Debug.Log($"Delay: {combatManager.performList[0].chosenAttack.delay}");
        currentCooldown = combatManager.performList[0].chosenAttack.delay;
    }



    //Work in progress for debuffing
    public void UpdateStatValues()
    {
        Debug.Log("I'm here now");
        foreach (BaseStatusEffect b in currentEffects)
        {
            switch (b.targetStat)
            {
                case ("PhysicalDefense"):
                    {
                        combatUnit.physicalDefense = combatUnit.originalPDefense + b.fixedModifier + (combatUnit.originalPDefense * b.percentageModifier / 100);
                        break;
                    }
            }
        }
    }

    //Currently unusued
    public void UpdateDebuffDurations()
    {
        foreach (BaseStatusEffect b in currentEffects)
        {
            b.turnDuration--;
            if (b.turnDuration == 0)
            {
                currentEffects.Remove(b);
            }
        }
    }

    #endregion


    #region GUI visualization

    //Updates the information (text, bars) on the unit's info card
    public void UpdateCharacterCard()
    {
        float hpToDisplay = (float)combatUnit.currentHP / (float)combatUnit.maxHP;
        stats.hpBar.transform.localScale = new Vector3(Mathf.Clamp(hpToDisplay, 0, 1), stats.hpBar.transform.transform.localScale.y, stats.hpBar.transform.transform.localScale.z);

        float mpToDisplay = (float)combatUnit.currentMP / (float)combatUnit.maxMP;
        stats.mpBar.transform.localScale = new Vector3(Mathf.Clamp(mpToDisplay, 0, 1), stats.mpBar.transform.transform.localScale.y, stats.mpBar.transform.transform.localScale.z);

        float progressToDisplay = currentCooldown / maxCooldown;
        stats.progressBar.transform.localScale = new Vector3(Mathf.Clamp(progressToDisplay, 0, 1), stats.progressBar.transform.transform.localScale.y, stats.progressBar.transform.transform.localScale.z);

        stats.characterHP.text = combatUnit.currentHP.ToString();
        stats.characterMP.text = combatUnit.currentMP.ToString();
        stats.characterProgress.text = ((int)currentCooldown).ToString();
    }

    //Shows that the target is dead on the screen
    //Currently different implementation for players and enemies because players are displayed using the
    //boxes on the bottom of the screen and enemies are sprites in the middle of the screen
    public void ShowDeath()
    {
        if (combatUnit.unitType == Entity.UnitType.Player)
        {
            characterCard.transform.Find("ActiveTag").GetComponent<Text>().enabled = true;
            characterCard.transform.Find("ActiveTag").GetComponent<Text>().text = "DEAD";
        }
        else
        {
            Sprite sp = Resources.Load("death.png") as Sprite;
            this.transform.Find("Targeter").GetComponent<SpriteRenderer>().sprite = sp;
            this.transform.Find("Targeter").gameObject.SetActive(true);
        }
    }

    //Shows the visual effect of the chosen attack on the target
    public void ShowEffect(GameObject target)
    {
        battleEffects.GetComponent<SpriteRenderer>().enabled = true;
        battleEffects.transform.position = target.transform.position;
    }

    //Hides the visual effect
    public void HideEffect()
    {
        battleEffects.GetComponent<SpriteRenderer>().enabled = false;
    }

    //For one second shows the result of the action (damage/healing amount, miss) above the target sprite
    //or character card
    public IEnumerator ShowActionResult(string result)
    {
        combatManager.damageText.GetComponent<Text>().text = result;
        combatManager.damageText.GetComponent<Text>().enabled = true;
        combatManager.damageText.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, 0);
        yield return new WaitForSeconds(1f);
        combatManager.damageText.GetComponent<Text>().enabled = false;

    }

    #endregion


}
