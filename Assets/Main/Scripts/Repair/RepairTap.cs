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
            ResumePlayerAnimation();
            Debug.Log($"Tap registrado: {tapCount}/{tapsRequired}");

            if (tapCount >= tapsRequired)
            {
                FinishRepair();
            }
        }
    }

    private void Update()
    {
        if (repairInProgress && tapCount < tapsRequired)
        {
            PausePlayerAnimation();
        }
    }

    protected override void PlayAnimation(string animationName)
    {
        currentMachine?.PlayAnimation(animationName);
    }
}
