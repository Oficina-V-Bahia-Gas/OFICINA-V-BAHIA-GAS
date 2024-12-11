using UnityEngine;

public abstract class Repairs : MonoBehaviour
{
    protected bool repairInProgress = false;
    protected bool repairCompleted = false;

    public virtual void StartRepair()
    {
        ResetRepair();
        repairInProgress = true;
        repairCompleted = false;
        Debug.Log("Iniciando conserto.");
    }

    public virtual void FinishRepair()
    {
        if (!repairInProgress)
        {
            Debug.LogWarning("Conserto j� finalizado ou n�o iniciado.");
            return;
        }

        repairInProgress = false;
        repairCompleted = true;

        if (HudInteraction.instance != null && HudInteraction.instance.repairManager != null)
        {
            Debug.Log("Chamando EndRepair no RepairManager.");
            HudInteraction.instance.repairManager.NotifyRepairComplete();
        }

        Debug.Log("Conserto conclu�do.");
        //ResetRepair();
    }

    public virtual void ResetRepair()
    {
        repairInProgress = false;
        repairCompleted = false;
        Debug.Log("Conserto resetado.");
    }
}