using UnityEngine;
using static Machines;

public class InteractableTrigger : MonoBehaviour
{
    IMachine interactableMachine;
    [SerializeField] MachineAction actionToPerform = MachineAction.Repair;

    public void OnButtonClick()
    {
        Interaction();
    }

    void Interaction()
    {
        if (interactableMachine != null)
        {
            interactableMachine.PerformAction(actionToPerform);
            Debug.Log("Performing action: " + actionToPerform);
        }
        else
        {
            Debug.LogWarning("No interactable machine available.");
        }
    }

    public void SetInteractableMachine(IMachine _machine)
    {
        interactableMachine = _machine;
    }

    public void ClearInteractableMachine()
    {
        interactableMachine = null;
    }
}