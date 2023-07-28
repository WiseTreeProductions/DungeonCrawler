using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrateWall : Interactable
{
    public AudioSource audioSource;
    public AudioClip grateNoise;
    public GameObject grateObject;
    Vector3 openPosition;
    Vector3 closedPosition;

    void Start()
    {
        openPosition = grateObject.transform.position - grateObject.transform.up;
        closedPosition = grateObject.transform.position;
        interactText = "Open";
    }

    void Update()
    {
        CheckWaiting();
    }

    public override IEnumerator OnInteract()
    {
        if (!interactionInProgress)
        {
            //Debug.Log("AAAAAAAAA");
            audioSource.PlayOneShot(grateNoise);
            if (open)
            {
                while (grateObject.transform.position != closedPosition)
                {
                    grateObject.transform.position = Vector3.MoveTowards(grateObject.transform.position, closedPosition, 1f * Time.deltaTime);
                    open = false;
                    yield return null;
                    interactText = "Open";
                }
            }
            else
            {
                while (grateObject.transform.position != openPosition)
                {
                    grateObject.transform.position = Vector3.MoveTowards(grateObject.transform.position, openPosition, 1f * Time.deltaTime);
                    if (grateObject.transform.position == openPosition)
                    {
                        open = true;
                        interactText = "Close";
                    }
                    yield return null;
                }
            }
        }
    }

    private void CheckWaiting()
    {
        if (open && Vector3.Distance(grateObject.transform.position, openPosition) < 0.05f)
        {
            interactionInProgress = false;
            return;
        }
        else if (!open && Vector3.Distance(grateObject.transform.position, closedPosition) < 0.05f)
        {
            interactionInProgress = false;
            return;
        }
        else
        {
            interactionInProgress = true;
            return;
        }
    }
}