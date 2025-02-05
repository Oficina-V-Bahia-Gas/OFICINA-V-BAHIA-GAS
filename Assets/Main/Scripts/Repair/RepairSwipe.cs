using UnityEngine;

public class RepairSwipe : Repairs
{
    public RepairsCameraManager repairCameraManager;
    public float distance = 50f;
    float swipeProgress = 0f;
    public float swipesRequired = 20f;

    public override void StartRepair(RepairManager _repairManager = null)
    {
        base.StartRepair(_repairManager);

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

    private void Update()
    {
        if (!repairInProgress) return;

        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            float _distance = Vector2.Distance(Vector2.zero, _touch.deltaPosition);

            if (_distance >= distance)
            {
                swipeProgress++;
                Debug.Log($"Swipe registrado: {swipeProgress}/{swipesRequired}");

                if (swipeProgress >= swipesRequired)
                {
                    FinishRepair();
                }
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