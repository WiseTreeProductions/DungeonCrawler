using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWithChest : Lootable
{
    public AudioSource audioSource;
    public AudioClip openSound;
    public GameObject chestObject;
    public Animator animationController;
    public DungeonMaster dungeonMaster;
    public Dialogue dialogue;

    void Start()
    {
        frontOnly = true;
        interactText = "Open";
    }


    public override IEnumerator OnInteract()
    {
        interactionInProgress = true;
        //Play the animation
        audioSource.PlayOneShot(openSound);
        animationController.SetBool("playOpen", true);
        Debug.Log(animationController.GetCurrentAnimatorStateInfo(0).normalizedTime);
        yield return new WaitForSeconds(1f);
        while (animationController.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            yield return null;
        }

        foreach (Item item in items)
        {
            dungeonMaster.playerParty.LootItem(item);
        }


        foreach (Item item in dungeonMaster.playerParty.partyInventory)
        {
            Debug.Log(item.itemName);
        }

        if (gold > 0)
        {
            dungeonMaster.playerParty.LootGold(gold);
        }
        dungeonMaster.dialogueHandler.StartDialogue(dialogue);
        interactionInProgress = false;
        canInteract = false;
    }
}