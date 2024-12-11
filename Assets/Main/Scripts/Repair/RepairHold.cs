using UnityEngine;

public class RepairHold : Repairs
{
    bool isHolding = false;
    float holdProgress = 0f;
    public float holdDuration = 5f;
    public RepairsCameraManager holdCameraManager;

    public override void StartRepair()
    {
        base.StartRepair();

        CharacterInfo _characterInfo = FindObjectOfType<CharacterInfo>();
        if (_characterInfo != null)
        {
            currentMachine = _characterInfo.GetLastInteractedMachine();
        }

        if (holdCameraManager != null && currentMachine != null)
        {
            Transform _target = GetFirstChild(currentMachine);
            if (_target != null)
            {
                holdCameraManager.SetTargetTransform(_target);
                holdCameraManager.ActivateCamera();
            }
            else
            {
                Debug.LogWarning($"Nenhum filho encontrado na máquina {currentMachine.name}.");
            }
        }
    }

    Transform GetFirstChild(Machines _machine)
    {
        if (_machine.transform.childCount > 0)
        {
            return _machine.transform.GetChild(0);
        }
        return null;
    }

    private void Update()
    {
        if (repairInProgress && isHolding)
        {
            holdProgress += Time.deltaTime;
            if (holdProgress >= holdDuration)
            {
                StopHolding();
                FinishRepair();
            }
        }
    }

    public void StartHolding()
    {
        if (!repairInProgress) return;

        isHolding = true;
        holdProgress = 0f;
    }

    public void StopHolding()
    {
        isHolding = false;
    }

    public override void FinishRepair()
    {
        base.FinishRepair();
        if (holdCameraManager != null)
        {
            holdCameraManager.ClearTarget();
        }
    }

    public override void ResetRepair()
    {
        base.ResetRepair();
        holdProgress = 0f;
        Debug.Log("Resetando conserto: RepairHold");
    }
}