using UnityEngine;

public class RepairTap : Repairs
{
    int tapCount = 0;
    public int tapsRequired = 20;
    public RepairsCameraManager tapCameraManager;

    public override void StartRepair()
    {
        base.StartRepair();

        CharacterInfo _characterInfo = FindObjectOfType<CharacterInfo>();
        if (_characterInfo != null)
        {
            currentMachine = _characterInfo.GetLastInteractedMachine();
        }

        if (tapCameraManager != null && currentMachine != null)
        {
            Transform _target = GetFirstChild(currentMachine);
            if (_target != null)
            {
                tapCameraManager.SetTargetTransform(_target);
                tapCameraManager.ActivateCamera();
            }
            else
            {
                Debug.LogWarning($"Nenhum filho encontrado na máquina {currentMachine.name}.");
            }
        }
        else
        {
            Debug.LogWarning("Câmera de reparo ou máquina atual não configurada corretamente.");
        }
    }

    Transform GetFirstChild(Machines _machine)
    {
        if (_machine != null && _machine.transform.childCount > 0)
        {
            return _machine.transform.GetChild(0);
        }
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
        if (tapCameraManager != null)
        {
            tapCameraManager.ClearTarget();
        }
    }

    public override void ResetRepair()
    {
        base.ResetRepair();
        tapCount = 0;
        Debug.Log("Resetando conserto: RepairTap");
    }
}