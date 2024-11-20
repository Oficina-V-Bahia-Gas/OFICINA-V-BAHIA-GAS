using UnityEngine;

public abstract class Repairs : MonoBehaviour
{
    protected bool repairInProgress = false;
    protected bool consertoConcluido = false;

    public virtual void StartRepair()
    {
        repairInProgress = true;
        consertoConcluido = false;
        Debug.Log("Iniciando conserto.");
    }

    public virtual void FinishRepair()
    {
        repairInProgress = false;
        consertoConcluido = true;
        Debug.Log("Conserto concluído.");
    }

    public virtual void ResetRepair()
    {
        repairInProgress = false;
        consertoConcluido = false;
        Debug.Log("Conserto resetado.");
    }
}