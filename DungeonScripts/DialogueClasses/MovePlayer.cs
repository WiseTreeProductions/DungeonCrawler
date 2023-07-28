using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class MovePlayer : DialogueAction
{
    public Vector3 coordinate;
    public GameObject player;

    public override void Execute()
    {
        // Code to open the door
        player.GetComponent<PlayerControls>().MoveBack();
    }
}
