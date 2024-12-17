using UnityEngine;

public abstract class Repairs : MonoBehaviour
{
    protected bool repairInProgress = false;
    protected bool repairCompleted = false;
    protected Machines currentMachine;

    protected Animator playerAnimator; // Referência ao Animator do jogador

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
                playerAnimator = characterInfo.GetComponent<Animator>();

                if (playerAnimator != null)
                {
                    playerAnimator.SetTrigger("StartRepair");
                    playerAnimator.SetBool("IsRepairing", true);
                }
            }
        }

        PlayAnimation("StartRepair");
    }

    public virtual void FinishRepair()
    {
        if (!repairInProgress) return;

        repairInProgress = false;
        repairCompleted = true;

        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("FinishRepair");
            playerAnimator.SetBool("IsRepairing", false);
        }

        if (HudInteraction.instance != null && HudInteraction.instance.repairManager != null)
        {
            HudInteraction.instance.repairManager.NotifyRepairComplete();
        }

        PlayAnimation("FinishRepair");
        Debug.Log("Conserto concluído.");
    }

    public virtual void ResetRepair()
    {
        repairInProgress = false;
        repairCompleted = false;

        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("ResetRepair");
            playerAnimator.SetBool("IsRepairing", false);
        }

        PlayAnimation("ResetRepair");
        Debug.Log("Conserto resetado.");
    }

    void FaceMachine(GameObject player, Transform machineTransform)
    {
        Vector3 directionToMachine = (machineTransform.position - player.transform.position).normalized;
        directionToMachine.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToMachine);
        player.transform.rotation = targetRotation;
    }

    protected void PausePlayerAnimation()
    {
        if (playerAnimator != null)
        {
            playerAnimator.speed = 0;
        }
    }

    protected void ResumePlayerAnimation()
    {
        if (playerAnimator != null)
        {
            playerAnimator.speed = 1;
        }
    }

    protected abstract void PlayAnimation(string animationName);
}