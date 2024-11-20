using UnityEngine;
using UnityEngine.UI;
using static MachineDetrition;
using static Machines;

public class InteractableTrigger : MonoBehaviour
{
    IMachine interactableMachine;
    [SerializeField] MachineAction actionToPerform = MachineAction.Repair;
    [SerializeField] Slider repairProgressSlider;

    void Start()
    {
        if (repairProgressSlider != null)
        {
            repairProgressSlider.gameObject.SetActive(false);
        }
    }

    public void OnButtonClick()
    {
        Interaction();
    }

    void Interaction()
    {
        if (interactableMachine != null)
        {
            if (actionToPerform == MachineAction.Repair && interactableMachine.CanPerformAction(actionToPerform))
            {
                interactableMachine.PerformAction(actionToPerform);
                Debug.Log("Performing action: " + actionToPerform);

                if (repairProgressSlider != null)
                {
                    repairProgressSlider.gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.LogWarning("Cannot interact: Machine is not in a state to perform this action.");
            }
        }
        else
        {
            Debug.LogWarning("No interactable machine available.");
        }
    }

    public void UpdateRepairProgress(float _progress)
    {
        if (repairProgressSlider != null)
        {
            repairProgressSlider.value = _progress;

            if (_progress >= 1f)
            {
                repairProgressSlider.gameObject.SetActive(false);
            }
        }
    }

    public void SetInteractableMachine(IMachine _machine)
    {
        interactableMachine = _machine;
    }

    public void ClearInteractableMachine()
    {
        interactableMachine = null;
        if (repairProgressSlider != null)
        {
            repairProgressSlider.gameObject.SetActive(false);
        }
    }
}