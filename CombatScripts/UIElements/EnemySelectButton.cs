using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemySelectButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject enemy;

    public void SelectEnemy()
    {
        GameObject.Find("CombatManager").GetComponent<CombatManager>().SelectTarget(enemy);
    }

    public void HighlightEnemy()
    {
        enemy.transform.Find("Targeter").gameObject.SetActive(true);
    }

    public void UnHighlightEnemy()
    {
        enemy.transform.Find("Targeter").gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameObject combatManager = GameObject.Find("CombatManager");
            combatManager.GetComponent<CombatManager>().PopulateInfoPanel(enemy);
        }
    }

}
