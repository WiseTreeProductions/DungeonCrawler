using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutWall : Interactable
{
    public SpriteRenderer holeSprite;

    public Sprite holeStage1;
    public Sprite holeStage2;
    public Sprite holeStage3;

    public Vector3 targetPosition;
    public GameObject targetWall;
    
    public bool activated = false;
    public bool oneWay = false;
    public bool isEntrance;

    public Dialogue closedShortcutDialogue;
    public Dialogue openSHortcutDialogue;

    public DungeonMaster dungeonMaster;

    public AudioSource audioSource;
    public AudioClip shortCutCreationSound;
    public AudioClip shortCutPassSound;

    void Start()
    {
        interactText = "Inspect";
    }
    public override IEnumerator OnInteract()
    {
        interactionInProgress = true;
        if (!activated)
        {
            if (isEntrance)
            {
                dungeonMaster.dialogueHandler.StartDialogue(openSHortcutDialogue);
            }
            else
            {
                dungeonMaster.dialogueHandler.StartDialogue(closedShortcutDialogue);
            }
            interactionInProgress = false;
        }
        else
        {
            audioSource.PlayOneShot(shortCutPassSound);
            yield return dungeonMaster.guiManager.FadeAlpha(1f);
            yield return dungeonMaster.player.GetComponent<PlayerControls>().TeleportToTarget(targetPosition);
            yield return dungeonMaster.guiManager.FadeAlpha(0f);
            interactionInProgress = false;
        }
    }

    public void TransformIntoPassage(GameObject wall)
    {
        ShortcutWall wallScript = wall.GetComponent<ShortcutWall>();
        wallScript.activated = true;
        wallScript.holeSprite.sprite = holeStage3;
        wallScript.interactText = "Pass through";
    }

    public IEnumerator ShortcutCreation()
    {
        interactionInProgress = true;
        audioSource.PlayOneShot(shortCutCreationSound);
        TransformIntoPassage(gameObject);
        TransformIntoPassage(targetWall);
        yield return dungeonMaster.guiManager.FadeAlpha(1f);
        yield return dungeonMaster.player.GetComponent<PlayerControls>().TeleportToTarget(targetPosition);
        yield return dungeonMaster.guiManager.FadeAlpha(0f);
        interactionInProgress = false;
    }

}
