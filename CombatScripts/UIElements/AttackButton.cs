using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    public BaseAttack spellToCast;
    public GameObject tooltipPanel;

    private void Start()
    {
        tooltipPanel = GameObject.Find("TooltipPanel");
    }

    public void CastSpell()
    {
        GameObject.Find("CombatManager").GetComponent<CombatManager>().CastSpell(spellToCast);
        tooltipPanel.transform.Find("Holder").gameObject.SetActive(false);
    }

    public void populateTooltip()
    {
        tooltipPanel.transform.Find("Holder").gameObject.SetActive(true);

        tooltipPanel.transform.Find("Holder").Find("NamePanel").Find("Text").GetComponent<Text>().text = spellToCast.attackName;
        tooltipPanel.transform.Find("Holder").Find("CostPanel").Find("Text").GetComponent<Text>().text = spellToCast.manaCost.ToString();
        tooltipPanel.transform.Find("Holder").Find("DamagePanel").Find("Text").GetComponent<Text>().text = spellToCast.minDamage.ToString() + " - " + spellToCast.maxDamage.ToString();
        tooltipPanel.transform.Find("Holder").Find("DelayPanel").Find("Text").GetComponent<Text>().text = spellToCast.delay.ToString();
        tooltipPanel.transform.Find("Holder").Find("DescriptionPanel").Find("Text").GetComponent<Text>().text = spellToCast.description;
    }

    public void depopulateTooltip()
    {
        tooltipPanel.transform.Find("Holder").gameObject.SetActive(false);
    }
}
