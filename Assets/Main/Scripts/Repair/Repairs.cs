using UnityEngine;

public abstract class Repairs : MonoBehaviour
{
    protected bool repairInProgress = false;
    protected bool repairCompleted = false;
    protected Machines currentMachine;
    protected Animator playerAnimator;
    protected RepairManager repairManager;

    public virtual void StartRepair(RepairManager _repairManager = null)
    {
        if(_repairManager != null)
        {
            Debug.LogError("check");
            repairManager = _repairManager;
        }

        ResetRepair();
        repairInProgress = true;
        repairCompleted = false;

        CharacterInfo characterInfo = FindObjectOfType<CharacterInfo>();

        if (characterInfo != null)
        {
            Machines newMachine = characterInfo.GetLastInteractedMachine();
            

            if (newMachine != null)
            {
                if (currentMachine != null && currentMachine != newMachine)
                {
                    currentMachine.StopRepairAnimation();
                }

                currentMachine = newMachine;
                FaceMachine(characterInfo.gameObject, currentMachine.transform);
                playerAnimator = characterInfo.GetComponent<Animator>();

                if (playerAnimator != null)
                {
                    playerAnimator.SetBool("IsRepairing", true);
                }

                currentMachine.StartRepairAnimation();
            }
        }
    }

    public virtual void FinishRepair()
    {
        if (!repairInProgress) return;

        repairInProgress = false;
        repairCompleted = true;

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsRepairing", false);
        }

        if (currentMachine != null)
        {
            currentMachine.StopRepairAnimation();
            currentMachine = null;
        }

        if (repairManager != null)
        {
            Debug.LogError("check");
            repairManager.NotifyRepairComplete();
        }
    }

    public virtual void ResetRepair()
    {
        repairInProgress = false;
        repairCompleted = false;

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsRepairing", false);
        }

        if (currentMachine != null)
        {
            currentMachine.StopRepairAnimation();
        }
    }

    private void FaceMachine(GameObject player, Transform machineTransform)
    {
        Vector3 directionToMachine = (machineTransform.position - player.transform.position).normalized;
        directionToMachine.y = 0;
        player.transform.rotation = Quaternion.LookRotation(directionToMachine);
    }
}