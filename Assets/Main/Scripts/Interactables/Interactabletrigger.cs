using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    private bool interact = false;
    [SerializeField] private InteractableClass interactable_object;

    private void Start()
    {
        if(GetComponentInParent<InteractableClass>())
        {
            interactable_object = GetComponentInParent<InteractableClass>();
        }
        else
        {
            Debug.LogWarning("Parent Object (" + transform.parent + ") of " + this + " must contain InteractableClass or other interactables. Interaction on this trigger is disabled...");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Interaction();
        }
    }

    void Interaction()
    {
        if (interact)
        {
            interactable_object.Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        interact = true;
    }

    private void OnTriggerExit(Collider other)
    {
        interact = false;
    }

    
}
