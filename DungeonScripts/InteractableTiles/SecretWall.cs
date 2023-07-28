using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretWall : DistantTrigger
{
    public GameObject button;

    public Vector3 targetPosition;
    public Vector3 targetWallPosition;

    void Start()
    {
        interactText = "Push";
        targetPosition = new Vector3(button.transform.localPosition.x, 0.41f, button.transform.localPosition.z);
        targetWallPosition = new Vector3(triggerTarget.transform.localPosition.x, -0.4f, triggerTarget.transform.localPosition.z);
    }
    public override IEnumerator OnInteract()
    {
        interactionInProgress = true;
        targetAudioSource.PlayOneShot(targetSound);
        while (button.transform.localPosition != targetPosition)
        {
            button.transform.localPosition = Vector3.MoveTowards(button.transform.localPosition, targetPosition, 1f * Time.deltaTime);
            open = false;
            yield return null;
        }
        canInteract = false;

        yield return StartCoroutine(ActivateDistantObject());
        interactionInProgress = false;
    }

    public override IEnumerator ActivateDistantObject()
    {
        targetCamera.gameObject.SetActive(true);
        targetAudioSource.PlayOneShot(targetSound);
        while (triggerTarget.transform.localPosition != targetWallPosition)
        {
            triggerTarget.transform.localPosition = Vector3.MoveTowards(triggerTarget.transform.localPosition, targetWallPosition, 1f * Time.deltaTime);
            yield return null;
        }
        targetCamera.gameObject.SetActive(false);
    }
}
