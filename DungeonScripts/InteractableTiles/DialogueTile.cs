using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTile : Interactable
{
    public AudioSource audioSource;
    public AudioClip openSound;
    public GameObject npcRepresentation;
    public DungeonMaster dungeonMaster;
    public Dialogue dialogue;
    public Transform cameraTransform;

    void Start()
    {
        interactText = "Talk";
        cameraTransform = Camera.main.transform;
        automatic = true;
    }

    void LateUpdate()
    {
        npcRepresentation.transform.LookAt(cameraTransform);
    }

    public override IEnumerator OnInteract()
    {
        canInteract = false;
        dungeonMaster.dialogueHandler.StartDialogue(dialogue);
        while (dungeonMaster.dialogueHandler.dialoguePanel.activeSelf)
        {
            yield return null;
        }

        while (!dungeonMaster.player.GetComponent<PlayerControls>().waiting)
        {
            yield return null;
        }
        canInteract = true;
    }
}