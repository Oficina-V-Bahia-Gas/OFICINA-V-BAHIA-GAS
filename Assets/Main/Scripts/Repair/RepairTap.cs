using UnityEngine;

public class RepairTap : Repairs
{
    public RepairsCameraManager repairCameraManager;
    int tapCount = 0;
    public int tapsRequired = 20;

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
            return machine.transform.GetChild(0);
        }

        Debug.LogWarning("A máquina não possui filhos ou é nula.");
        return null;
    }

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

    public override void FinishRepair()
    {
        base.FinishRepair();
        if (repairCameraManager != null)
        {
            repairCameraManager.ClearTarget();
        }
    }
}