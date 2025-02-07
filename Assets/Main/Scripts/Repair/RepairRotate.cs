using UnityEngine;
using UnityEngine.UI;

public class RepairRotate : Repairs
{
    [Header("Configurações de Reparação")]
    int totalRotationsRequired = 2;
    const float degreesPerRotation = 360f;
    float rotationProgress = 0f;
    float rotationAngle = 0f;

    [Header("Sensibilidade")]
    [SerializeField, Tooltip("Multiplicador para aumentar a sensibilidade da rotação.")]
    float rotationSensitivity = 10.0f;

    [Header("Referências")]
    [SerializeField] RectTransform handleTransform;
    [SerializeField] RepairsCameraManager repairCameraManager;

    Vector2 rotationCenter;
    Vector2 lastTouchDirection;
    bool isRotating = false;

    public override void StartRepair(RepairManager _repairManager = null)
    {
        base.StartRepair(_repairManager);
        rotationProgress = 0f;
        rotationAngle = 0f;

        if (handleTransform != null)
        {
            rotationCenter = handleTransform.position;
        }
        else
        {
            rotationCenter = Vector2.zero;
        }

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
            }
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
        if (!repairInProgress)
            return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            StartRotation(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            UpdateRotation(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopRotation();
        }
#endif

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartRotation(touch.position);
                    break;
                case TouchPhase.Moved:
                    UpdateRotation(touch.position);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    StopRotation();
                    break;
            }
        }

        if (handleTransform != null)
        {
            handleTransform.localRotation = Quaternion.Euler(0, 0, -rotationAngle);
        }
    }

    private void StartRotation(Vector2 touchPosition)
    {
        AudioManager.instance.Play("Squeak");
        isRotating = true;
        if (handleTransform != null)
        {
            rotationCenter = handleTransform.position;
        }
        else
        {
            rotationCenter = touchPosition;
        }
        lastTouchDirection = (touchPosition - rotationCenter).normalized;
    }

    private void UpdateRotation(Vector2 touchPosition)
    {
        if (!isRotating)
            return;

        Vector2 currentTouchDirection = (touchPosition - rotationCenter).normalized;
        float angleDelta = Vector2.SignedAngle(lastTouchDirection, currentTouchDirection);

        if (angleDelta < 0)
        {
            float effectiveDelta = -angleDelta * rotationSensitivity;
            rotationProgress += effectiveDelta;
            rotationAngle += effectiveDelta;
        }

        lastTouchDirection = currentTouchDirection;

        if (rotationProgress >= totalRotationsRequired * degreesPerRotation)
        {
            FinishRepair();
        }
    }

    private void StopRotation()
    {
        AudioManager.instance.Stop("Squeak");
        isRotating = false;
    }

    public override void FinishRepair()
    {
        base.FinishRepair();

        AudioManager.instance.Stop("Squeak");
        rotationProgress = 0f;
        rotationAngle = 0f;

        if (repairCameraManager != null)
        {
            repairCameraManager.ClearTarget();
        }

        if (handleTransform != null)
        {
            handleTransform.localRotation = Quaternion.identity;
        }
    }
}