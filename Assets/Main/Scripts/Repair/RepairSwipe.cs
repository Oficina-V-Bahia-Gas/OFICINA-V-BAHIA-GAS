using UnityEngine;

public class RepairSwipe : Repairs
{
    [SerializeField] private SwipeCameraManager swipeCameraManager;
    [SerializeField] private float distance = 10f;
    float swipeProgress = 0f;
    public float swipesRequired = 5f;

    Machines currentMachine;

    public override void StartRepair()
    {
        base.StartRepair();

        CharacterInfo characterInfo = FindObjectOfType<CharacterInfo>();
        if (characterInfo != null)
        {
            currentMachine = characterInfo.GetLastInteractedMachine();
        }

        if (swipeCameraManager != null && currentMachine != null)
        {
            Transform target = GetFirstChild(currentMachine);
            if (target != null)
            {
                swipeCameraManager.SetTargetTransform(target);
                swipeCameraManager.ActivateCamera();
            }
            else
            {
                Debug.LogWarning($"Nenhum filho encontrado na máquina {currentMachine.name}.");
            }
        }
        else
        {
            Debug.LogWarning("SwipeCameraManager ou máquina atual não configurada corretamente.");
        }
    }

    Transform GetFirstChild(Machines machine)
    {
        if (machine.transform.childCount > 0)
        {
            return machine.transform.GetChild(0);
        }
        return null;
    }

    private void Update()
    {
        if (!repairInProgress) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float dist = Vector2.Distance(Vector2.zero, touch.deltaPosition);

            if (dist >= distance)
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