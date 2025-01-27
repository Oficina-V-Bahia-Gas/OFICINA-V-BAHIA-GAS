using UnityEngine;

public class RepairHold : Repairs
{
    public RepairsCameraManager repairCameraManager;
    bool isHolding = false;
    float holdProgress = 0f;
    public float holdDuration = 5f;

    public override void StartRepair()
    {
        base.StartRepair();

        CharacterInfo characterInfo = FindObjectOfType<CharacterInfo>();
        if (characterInfo != null)
        {
            currentMachine = characterInfo.GetLastInteractedMachine();
            if (currentMachine != null)
            {
                Transform targetTransform = GetFirstChild(currentMachine);
                if (targetTransform != null && repairCameraManager != null)
                {
                    repairCameraManager.SetTargetTransform(targetTransform);
                    Debug.Log("Câmera configurada com sucesso para a máquina.");
                }
                else
                {
                    Debug.LogWarning("Target ou CameraManager não configurados corretamente.");
                }
            }
            else
            {
                Debug.LogWarning("Nenhuma máquina definida como última interagida.");
            }
        }
        else
        {
            Debug.LogWarning("CharacterInfo não encontrado.");
        }
    }

    private Transform GetFirstChild(Machines machine)
    {
        if (machine != null && machine.transform.childCount > 0)
        {
            return machine.transform.GetChild(0); // Retorna o primeiro filho da máquina
        }

        Debug.LogWarning("A máquina não possui filhos ou é nula.");
        return null;
    }

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

    public override void FinishRepair()
    {
        base.FinishRepair();
        if (repairCameraManager != null)
        {
            repairCameraManager.ClearTarget();
        }
    }

    protected override void PlayAnimation(string animationName)
    {
        currentMachine?.PlayAnimation(animationName);
    }
}
