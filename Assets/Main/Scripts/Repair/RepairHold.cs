using UnityEngine;

public class RepairHold : Repairs
{
    bool isHolding = false;
    float holdProgress = 0f;
    public float holdDuration = 5f;

    private void Update()
    {
        if (repairInProgress)
        {
            if (isHolding)
            {
                holdProgress += Time.deltaTime;
                if (holdProgress >= holdDuration)
                {
                    StopHolding();
                    FinishRepair();
                }
            }
            else
            {
                PausePlayerAnimation();
            }
        }
    }

    public void StartHolding()
    {
        if (!repairInProgress) return;

        isHolding = true;
        holdProgress = 0f;
        ResumePlayerAnimation();
    }

    public void StopHolding()
    {
        isHolding = false;
        PausePlayerAnimation();
    }

    protected override void PlayAnimation(string animationName)
    {
        currentMachine?.PlayAnimation(animationName);
    }
}
