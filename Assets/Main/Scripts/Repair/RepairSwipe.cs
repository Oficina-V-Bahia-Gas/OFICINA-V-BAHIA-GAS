using UnityEngine;

public class RepairSwipe : Repairs
{
    public RepairsCameraManager swipeCameraManager;
    public float distance = 50f;
    float swipeProgress = 0f;
    public float swipesRequired = 20f;

    public override void StartRepair()
    {
        base.StartRepair();

        CharacterInfo _characterInfo = FindObjectOfType<CharacterInfo>();
        if (_characterInfo != null)
        {
            currentMachine = _characterInfo.GetLastInteractedMachine();
        }

        if (swipeCameraManager != null && currentMachine != null)
        {
            Transform _target = GetFirstChild(currentMachine);
            if (_target != null)
            {
                swipeCameraManager.SetTargetTransform(_target);
                swipeCameraManager.ActivateCamera();
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
        if (swipeCameraManager != null)
        {
            swipeCameraManager.ClearTarget();
        }
    }
}