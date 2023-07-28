using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DistantTrigger : Interactable
{
    public AudioSource audioSource;
    public AudioSource targetAudioSource;
    public AudioClip triggerSound;
    public AudioClip targetSound;
    public GameObject triggerTarget;
    public Camera targetCamera;

    public abstract IEnumerator ActivateDistantObject();
}
