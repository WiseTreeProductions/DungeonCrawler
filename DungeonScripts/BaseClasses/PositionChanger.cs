using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PositionChanger : Interactable
{
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    public GameObject player;
    public AudioSource audioSource;
    public AudioClip movementSound;

}
