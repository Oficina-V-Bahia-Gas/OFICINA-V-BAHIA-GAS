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
                    Debug.Log("Ativando animações de reparo.");
                    playerAnimator.SetTrigger("StartRepair");
                    playerAnimator.SetBool("IsRepairing", true);
                }
                else
                {
                    Debug.LogWarning("Animator do jogador não encontrado.");
                }
            }
            else
            {
                Debug.LogWarning("Nenhuma máquina definida como última interagida.");
                return;
            }
        }
        else
        {
            Debug.LogWarning("CharacterInfo não encontrado.");
            return;
        }

        PlayAnimation("StartRepair");
    }

    public virtual void FinishRepair()
    {
        if (!repairInProgress) return;

        repairInProgress = false;
        repairCompleted = true;

        Debug.Log("Conserto concluído.");

        if (playerAnimator != null)
        {
            Debug.Log("Finalizando animações de reparo.");
            playerAnimator.SetTrigger("FinishRepair");
            playerAnimator.SetBool("IsRepairing", false);
        }

        if (HudInteraction.instance != null && HudInteraction.instance.repairManager != null)
        {
            HudInteraction.instance.repairManager.NotifyRepairComplete();
        }

        PlayAnimation("FinishRepair");
    }

    public virtual void ResetRepair()
    {
        repairInProgress = false;
        repairCompleted = false;

        Debug.Log("Conserto resetado.");

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsRepairing", false);
        }
    }

    private void FaceMachine(GameObject player, Transform machineTransform)
    {
        Vector3 directionToMachine = (machineTransform.position - player.transform.position).normalized;
        directionToMachine.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToMachine);
        player.transform.rotation = targetRotation;

        Debug.Log("Jogador virado em direção à máquina.");
    }

    protected void PausePlayerAnimation()
    {
        if (playerAnimator != null)
        {
            Debug.Log("Pausando animações do jogador.");
            playerAnimator.speed = 0; // Pausa o Animator
        }
    }

    protected void ResumePlayerAnimation()
    {
        if (playerAnimator != null)
        {
            Debug.Log("Retomando animações do jogador.");
            playerAnimator.speed = 1; // Retoma o Animator
        }
    }

    protected abstract void PlayAnimation(string animationName);
}