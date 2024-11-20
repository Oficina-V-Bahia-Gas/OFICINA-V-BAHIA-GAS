using UnityEngine;

public class RepairTap : Repairs
{
    int tapCount = 0;
    public int tapsRequired = 10;

    public void OnTap()
    {
        if (repairInProgress)
        {
            tapCount++;
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