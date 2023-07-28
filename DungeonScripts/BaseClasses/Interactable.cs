using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool open = false;
    public bool interactionInProgress = false;
    public bool frontOnly = false;
    public bool canInteract = true;
    public bool automatic = false;
    public string interactText;

    public abstract IEnumerator OnInteract();

    public bool CheckIfPlayerInFront()
    {
        RaycastHit hit;

        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
        Debug.Log($"ORIGIN POINT: {origin}");
        Debug.Log($"TARGET POINT: {transform.forward}");
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        Vector3 centerPoint = boxCollider.bounds.center;
        Debug.Log("Center Point: " + centerPoint);

        // Cast a ray in the direction the object is facing
        if (Physics.Raycast(centerPoint, transform.forward, out hit, 2f))
        {
            Debug.DrawLine(centerPoint, hit.point, Color.red);
            // Check if game object is interactable
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                //Debug.Log("PLAYER IS IN FRONT");
                return true;
            }
        }

        return false;
    }
}
