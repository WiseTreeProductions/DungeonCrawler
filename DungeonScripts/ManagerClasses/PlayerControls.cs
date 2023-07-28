using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public bool smoothTransition = false;
    public float transitionSpeed = 10f;
    public float transitionRotationSpeed = 500f;

    public Vector3 targetGridPosition;
    Vector3 previousGridPosition;
    public Vector3 targetRotation;

    public AudioSource audioSource;
    public AudioClip stepClip;
    public AudioClip bumpClip;

    public CameraControls cameraControls;

    public bool canInteract;
    public GameObject interactableObject;

    public DungeonMaster dungeonMaster;

    public bool waiting
    {
        get
        {
            if ((Vector3.Distance(transform.position, targetGridPosition) == 0) && (Vector3.Distance(transform.eulerAngles, targetRotation) == 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        targetGridPosition = transform.position;
        smoothTransition = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (dungeonMaster.gameState == DungeonMaster.GameState.IDLE)
        {
            if (Input.GetKeyUp(KeyCode.W))
            {
                MoveForward();
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                MoveLeft();
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                MoveRight();
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                MoveBack();
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                RotateLeft();
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                RotateRight();
            }

            CheckIfCanInteract();

            if (Input.GetKeyUp(KeyCode.Space))
            {

                if (canInteract)
                {
                    Debug.Log("INTERACTING");
                    Interact();
                }
                else
                {
                    Debug.Log("NOTHING TO INTERACT WITH");
                }
            }
        }
    }

    public void RotateLeft()
    {
        if (waiting)
        {
            targetRotation -= Vector3.up * 90f;
            StartCoroutine(RotateToTarget());
        }
    }

    public void RotateRight()
    {
        if (waiting)
        {
            targetRotation += Vector3.up * 90f;
            StartCoroutine(RotateToTarget());
        }
    }

    public void MoveForward()
    {
        if (!MovementBlocked(transform.forward))
        {
            if (waiting)
            {
                targetGridPosition += transform.forward;
                StartCoroutine(MoveToTarget());
            }
        }
        else
        {
            Debug.Log("RAMMING INTO WALL");
            Debug.Log(transform.position);
            Debug.Log(transform.position + transform.forward);
            StartCoroutine(BumpIntoWall(transform.forward));
        }
    }

    public void MoveLeft()
    {
        if (!MovementBlocked(-transform.right))
        {
            if (waiting)
            {
                targetGridPosition -= transform.right;
                StartCoroutine(MoveToTarget());
            }
        }
        else
        {
            Debug.Log("RAMMING INTO WALL");
            Debug.Log(transform.position);
            Debug.Log(transform.position - transform.right);
            StartCoroutine(BumpIntoWall(-transform.right));

        }
    }

    public void MoveRight()
    {
        if (!MovementBlocked(transform.right))
        {
            if (waiting)
            {
                targetGridPosition += transform.right;
                StartCoroutine(MoveToTarget());
            }
        }
        else
        {
            Debug.Log("RAMMING INTO WALL");
            Debug.Log(transform.position);
            Debug.Log(transform.position + transform.right);
            StartCoroutine(BumpIntoWall(transform.right));

        }
    }

    public void MoveBack()
    {
        if (!MovementBlocked(-transform.forward))
        {
            if (waiting)
            {
                targetGridPosition -= transform.forward;
                StartCoroutine(MoveToTarget());
            }
        }
        else
        {
            Debug.Log("RAMMING INTO WALL");
            Debug.Log(transform.position);
            Debug.Log(transform.position + transform.forward);
            StartCoroutine(BumpIntoWall(-transform.forward));
        }
    }

    private IEnumerator MoveToTarget()
    {
        Debug.Log($"ORIGIN: {transform.position}, TARGET: {targetGridPosition}");
        audioSource.PlayOneShot(stepClip);

        while (!waiting)
        {
            if (!smoothTransition)
            {
                transform.position = targetGridPosition;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetGridPosition, Time.deltaTime * transitionSpeed);
            }
            
            yield return null;
        }
    }

    public IEnumerator TeleportToTarget(Vector3 targetPosition)
    {
        targetGridPosition = targetPosition;
        transform.position = targetGridPosition;
        yield return null;
    }

    public IEnumerator TeleportToTarget(Vector3 targetPosition, Vector3 rotation)
    {
        targetGridPosition = targetPosition;
        targetRotation = rotation;
        if (targetRotation.y > 270f && targetRotation.y < 361f)
        {
            targetRotation.y = 0f;
        }

        if (targetRotation.y < 0f)
        {
            targetRotation.y = 270f;
        }
        transform.position = targetGridPosition;
        transform.rotation = Quaternion.Euler(targetRotation);
        yield return null;
    }

    public IEnumerator SlideForward()
    {
        yield return null;
    }

    private IEnumerator RotateToTarget()
    {
        if (targetRotation.y > 270f && targetRotation.y < 361f)
        {
            targetRotation.y = 0f;
        }

        if (targetRotation.y < 0f)
        {
            targetRotation.y = 270f;
        }

        while (!waiting)
        {
            if (!smoothTransition)
            {
                transform.rotation = Quaternion.Euler(targetRotation);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * transitionRotationSpeed);
            }
            
            yield return null;
        }
    }

    private IEnumerator BumpIntoWall(Vector3 direction)
    {
        Vector3 bumpTarget = transform.position + direction / 8f;
        Vector3 initialPosition = transform.position;
        cameraControls.Shake();
        audioSource.PlayOneShot(bumpClip);
        if (smoothTransition)
        {
            // Move the GameObject to the target position
            while (transform.position != bumpTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, bumpTarget, transitionSpeed * Time.deltaTime);
                yield return null;
            }

            // Move the GameObject back to the initial position
            while (transform.position != initialPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, initialPosition, transitionSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

    private bool MovementBlocked(Vector3 direction)
    {
        RaycastHit hit;

        // Cast a ray in the direction the object is facing
        if (Physics.Raycast(transform.position, direction, out hit, 1f))
        {
            // Check if the object hit is tagged as "wall"
            if (hit.collider.CompareTag("Wall"))
            {
                Debug.Log("MOVEMENT BLOCKED");
                return true;
            }

            if (hit.collider.CompareTag("Obstacle"))
            {
                if (!hit.collider.gameObject.GetComponent<Interactable>().open)
                {
                    Debug.Log("MOVEMENT BLOCKED");
                    return true;
                }
            }
        }

        Debug.Log("MOVEMENT UNBLOCKED");
        return false;
    }

    private void CheckIfCanInteract()
    {
        RaycastHit hit;

        // Cast a ray in the direction the object is facing
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            //Debug.Log(hit.collider.gameObject.name);
            // Check if game object is interactable
            if (hit.collider.gameObject.GetComponent<Interactable>())
            {
                if (hit.collider.GetComponent<Interactable>().canInteract)
                {
                    //Debug.Log("CAN INTERACT");
                    interactableObject = hit.collider.gameObject;
                    if (interactableObject.GetComponent<Interactable>().frontOnly)
                    {
                        if (interactableObject.GetComponent<Interactable>().CheckIfPlayerInFront())
                        {
                            dungeonMaster.guiManager.UpdateInteractText(interactableObject.GetComponent<Interactable>().interactText);
                            canInteract = true;
                            return;
                        }
                    }
                    else
                    {
                        dungeonMaster.guiManager.UpdateInteractText(interactableObject.GetComponent<Interactable>().interactText);
                        canInteract = true;
                        return;
                    }
                }
            }
        }

        // Cast a ray below the object
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1f))
        {
            // Check if game object is interactable
            if (hit.collider.gameObject.GetComponent<Interactable>())
            {
                if (hit.collider.GetComponent<Interactable>().canInteract)
                {
                    //Debug.Log("CAN INTERACT");
                    canInteract = true;
                    interactableObject = hit.collider.gameObject;
                    dungeonMaster.guiManager.UpdateInteractText(interactableObject.GetComponent<Interactable>().interactText);
                    if (interactableObject.GetComponent<Interactable>().automatic)
                    {
                        Interact();
                    }
                    return;
                }
            }
        }

        //Debug.Log("CANT INTERACT");
        canInteract = false;
        interactableObject = null;
    }

    public void Interact()
    {
        StartCoroutine(interactableObject.GetComponent<Interactable>().OnInteract());
    }
}
