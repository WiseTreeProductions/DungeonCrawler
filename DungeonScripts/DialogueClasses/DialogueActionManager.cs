using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActionManager : MonoBehaviour
{
    #region Shared Variables
    public GameObject playerObject;
    public DungeonMaster dungeonMaster;
    public GUIManager guiManager;
    public DialogueHandler dialogueHandler;
    public PlayerControls player;
    #endregion
    #region Guard Dialogue Variables
    public GameObject GuardTile;
    public Dialogue newGuardDialogue;
    #endregion
    #region Shortcut Wall Variables
    public GameObject shortCutEntrance;
    public GameObject shortCutEntrance2;
    #endregion

    void Start()
    {
        player = playerObject.GetComponent<PlayerControls>();
    }

    #region Guard Dialogue Interactions
    public void GuardDialogueMoveBack()
    {
        dialogueHandler.EndDialogue();
        player.MoveBack();
    }

    public void GuardDialogueAskForInformation()
    {
        dialogueHandler.NavigateToSpecificPage(2);
    }

    public void GuardDialogueIgnoreWarning()
    {
        dialogueHandler.NavigateToSpecificPage(3);
        StartCoroutine(GuardDialogueIgnoreWarningCoroutine());
    }

    public IEnumerator GuardDialogueIgnoreWarningCoroutine()
    {
        while (dungeonMaster.dialogueHandler.dialoguePanel.activeSelf)
        {
            yield return null;
        }
        GuardTile.GetComponent<DialogueTile>().automatic = false;
        GuardTile.GetComponent<DialogueTile>().dialogue = newGuardDialogue;
    }
    #endregion

    #region Shortcut Wall Interactions

    public void ShortCutWallCreation()
    {
        dialogueHandler.EndDialogue();
        StartCoroutine(shortCutEntrance.GetComponent<ShortcutWall>().ShortcutCreation());
    }

    public void ShortCutWallCreation2()
    {
        dialogueHandler.EndDialogue();
        StartCoroutine(shortCutEntrance2.GetComponent<ShortcutWall>().ShortcutCreation());
    }

    #endregion

    #region General Interactions
    public void CancelDialogue()
    {
        dialogueHandler.EndDialogue();
    }

    public void EmptyMethod()
    {
        Debug.Log("THIS IS AN EMPTY METHOD");
    }
    #endregion


    public Action GetMethodToExecute(Response.ActionType actionType)
    {
        switch (actionType)
        {
            case Response.ActionType.AcceptGuardRequest:
                return GuardDialogueMoveBack;
            case Response.ActionType.AskGuardWhy:
                return GuardDialogueAskForInformation;
            case Response.ActionType.DeclineGuardRequest:
                return GuardDialogueIgnoreWarning;
            case Response.ActionType.CreateWallShortcut:
                return ShortCutWallCreation;
            case Response.ActionType.CreateWallShortcut2:
                return ShortCutWallCreation;
            case Response.ActionType.CancelDialogue:
                return CancelDialogue;
            case Response.ActionType.None:
                return EmptyMethod;
            default:
                return null;
        }
    }

}
