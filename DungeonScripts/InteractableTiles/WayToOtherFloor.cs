using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WayToOtherFloor : PositionChanger
{
    public GUIManager guiManager;
    public GameObject targetStairs;
    public string destinationName;


    // Start is called before the first frame update
    void Start()
    {
        frontOnly = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator OnInteract()
    {
        interactionInProgress = true;
        audioSource.PlayOneShot(movementSound);
        yield return guiManager.FadeAlpha(1f);
        yield return player.GetComponent<PlayerControls>().TeleportToTarget(targetPosition, DeterminePlayerRotation());
        yield return guiManager.FadeAlpha(0f);
        //StartCoroutine(guiManager.ShowAreaNotifier(destinationName));
        guiManager.PlayAreaNameCoroutine(destinationName);
        interactionInProgress = false;
    }

    private Vector3 DeterminePlayerRotation()
    {
        Vector3 direction = targetPosition - targetStairs.transform.position;
        Debug.Log($"DIRECTION {direction} -DIRECTION {-direction}");
        Quaternion rotation = Quaternion.LookRotation(direction);
        Vector3 rotationVector = rotation.eulerAngles;
        Debug.Log($"ROTATION: {rotationVector}");
        rotationVector.x = 0;
        rotationVector.z = 0;
        return rotationVector;
    }

}
