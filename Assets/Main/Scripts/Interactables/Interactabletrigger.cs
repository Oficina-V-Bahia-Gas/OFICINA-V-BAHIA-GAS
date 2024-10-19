using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    private bool interact = false;
    [SerializeField] private GameObject interactable_object;

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
            Debug.Log("Interaction");
            //interactable_object.
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
