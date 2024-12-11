using UnityEngine;

public abstract class Repairs : MonoBehaviour
{
    protected bool repairInProgress = false;
    protected bool repairCompleted = false;
    protected Machines currentMachine;

    public virtual void StartRepair()
    {
        ResetRepair();
        repairInProgress = true;
        repairCompleted = false;
        Debug.Log("Iniciando conserto.");

        CharacterInfo characterInfo = FindObjectOfType<CharacterInfo>();
        if (characterInfo != null)
        {
            currentMachine = characterInfo.GetLastInteractedMachine();
            if (currentMachine != null)
            {
                FaceMachine(characterInfo.gameObject, currentMachine.transform);
            }
        }
    }

    public virtual void FinishRepair()
    {
        if (!repairInProgress)
        {
            Debug.LogWarning("Conserto já finalizado ou não iniciado.");
            return;
        }

        repairInProgress = false;
        repairCompleted = true;

        if (HudInteraction.instance != null && HudInteraction.instance.repairManager != null)
        {
            Debug.Log("Chamando EndRepair no RepairManager.");
            HudInteraction.instance.repairManager.NotifyRepairComplete();
        }

        Debug.Log("Conserto concluído.");
        //ResetRepair();
    }

    public virtual void ResetRepair()
    {
        repairInProgress = false;
        repairCompleted = false;
        Debug.Log("Conserto resetado.");
    }

    void FaceMachine(GameObject player, Transform machineTransform)
    {
        Vector3 directionToMachine = (machineTransform.position - player.transform.position).normalized;
        directionToMachine.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToMachine);
        player.transform.rotation = targetRotation;
    }
}