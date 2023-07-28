using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Response
{
    public string text;
    public enum ActionType
    {
        AcceptGuardRequest,
        DeclineGuardRequest,
        AskGuardWhy,
        CreateWallShortcut,
        CreateWallShortcut2,
        CancelDialogue,
        None
    }
    public ActionType type;

    public Action methodToExecute;

    public void ExecuteMethod()
    {
        if (methodToExecute != null)
        {
            methodToExecute.Invoke();
        }
    }

}
