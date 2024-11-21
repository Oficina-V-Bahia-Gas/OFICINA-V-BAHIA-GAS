using UnityEngine;

public class RepairHold : Repairs
{
    bool isHolding = false;
    float holdProgress = 0f;
    public float holdDuration = 5f;

    private void Update()
    {
        if (repairInProgress && isHolding)
        {
            holdProgress += Time.deltaTime;
            if (holdProgress >= holdDuration)
            {
                StopHolding();
                FinishRepair();
            }
        }
    }

    public void StartHolding()
    {
        Debug.Log("Início do Hold.");

        if (!repairInProgress) return;

        isHolding = true;
        holdProgress = 0f;
    }

    public void StopHolding()
    {
        isHolding = false;
        Debug.Log("Hold interrompido.");
    }

    public override void ResetRepair()
    {
        base.ResetRepair();
        holdProgress = 0f;
        Debug.Log("Resetando conserto: RepairHold");
    }
}