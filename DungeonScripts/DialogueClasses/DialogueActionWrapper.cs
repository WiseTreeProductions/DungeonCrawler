using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueActionWrapper
{
    public enum ActionType
    {
        MovePlayer,
        None
        // Add more action types here
    }

    public ActionType actionType;
    public MovePlayer movePlayerAction;

    public DialogueAction GetAction()
    {
        switch (actionType)
        {
            case ActionType.MovePlayer:
                return new MovePlayer();
            case ActionType.None:
                return null;
            default:
                return null;
        }
    }
}