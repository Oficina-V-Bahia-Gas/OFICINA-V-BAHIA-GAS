using UnityEngine;

public class RepairTap : Repairs
{
    int tapCount = 0;
    public int tapsRequired = 20;

    public void OnTap()
    {
        if (repairInProgress)
        {
            tapCount++;
            Debug.Log($"Tap registrado: {tapCount}/{tapsRequired}");

            if (tapCount >= tapsRequired)
            {
                FinishRepair();
            }
        }
    }

    public override void ResetRepair()
    {
        base.ResetRepair();
        tapCount = 0;
        Debug.Log("Resetando conserto: RepairTap");
    }
}