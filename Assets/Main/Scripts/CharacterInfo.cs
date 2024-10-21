using UnityEngine;
using static Machines;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 10;
    [SerializeField] float interactionDistance = 5f;
    [SerializeField] LayerMask interactionLayer;

    InteractableTrigger interactableTrigger;
    IMachine currentInteractableMachine;

    void Start()
    {
        interactableTrigger = FindObjectOfType<InteractableTrigger>();

        if (interactableTrigger == null)
        {
            Debug.LogWarning("No InteractableTrigger found in the scene.");
        }
    }

    void Update()
    {
        DetectInteractable();
    }

    public float GetWalkSpeed()
    {
        return _walkSpeed;
    }

    void DetectInteractable()
    {
        Ray _ray = new Ray(transform.position, transform.forward);
        RaycastHit _hit;

        Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.red);

        if (Physics.Raycast(_ray, out _hit, interactionDistance, interactionLayer))
        {
            IMachine machine = _hit.collider.GetComponent<IMachine>();

            if (machine != null)
            {
                currentInteractableMachine = machine;
                interactableTrigger.SetInteractableMachine(currentInteractableMachine);
                Debug.Log("Player can interact with: " + _hit.collider.name);
            }
        }
        else
        {
            currentInteractableMachine = null;
            interactableTrigger.ClearInteractableMachine();
        }
    }
}