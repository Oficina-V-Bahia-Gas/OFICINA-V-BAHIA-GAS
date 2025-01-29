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
                    Debug.Log("Ativando animações de reparo do jogador.");
                    playerAnimator.SetTrigger("StartRepair");
                    playerAnimator.SetBool("IsRepairing", true);
                }
                else
                {
                    Debug.LogWarning("Animator do jogador não encontrado.");
                }

                Debug.Log("Ativando animações de reparo da máquina.");
                PlayMachineAnimation("MachineRepairStart");
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
            Debug.Log("Finalizando animações de reparo do jogador.");
            playerAnimator.SetTrigger("FinishRepair");
            playerAnimator.SetBool("IsRepairing", false);
        }

        if (currentMachine != null)
        {
            Debug.Log("Finalizando animações de reparo da máquina.");
            PlayMachineAnimation("MachineRepairFinish");
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

        if (currentMachine != null)
        {
            PlayMachineAnimation("MachineIdle");
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

        if (currentMachine != null)
        {
            Debug.Log("Pausando animações da máquina.");
            PauseMachineAnimation();
        }
    }

    protected void ResumePlayerAnimation()
    {
        if (playerAnimator != null)
        {
            Debug.Log("Retomando animações do jogador.");
            playerAnimator.speed = 1; // Retoma o Animator
        }

        if (currentMachine != null)
        {
            Debug.Log("Retomando animações da máquina.");
            ResumeMachineAnimation();
        }
    }

    protected void PlayMachineAnimation(string animationName)
    {
        if (currentMachine != null)
        {
            Animator machineAnimator = currentMachine.GetComponent<Animator>();
            if (machineAnimator != null)
            {
                machineAnimator.Play(animationName);
            }
            else
            {
                Debug.LogWarning($"Animator não encontrado na máquina {currentMachine.name}.");
            }
        }
    }

    protected void PauseMachineAnimation()
    {
        if (currentMachine != null)
        {
            Animator machineAnimator = currentMachine.GetComponent<Animator>();
            if (machineAnimator != null)
            {
                machineAnimator.speed = 0; // Pausa o Animator
            }
        }
    }

    protected void ResumeMachineAnimation()
    {
        if (currentMachine != null)
        {
            Animator machineAnimator = currentMachine.GetComponent<Animator>();
            if (machineAnimator != null)
            {
                machineAnimator.speed = 1; // Retoma o Animator
            }
        }
    }

    protected abstract void PlayAnimation(string animationName);
}