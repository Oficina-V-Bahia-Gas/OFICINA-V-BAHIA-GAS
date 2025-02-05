using UnityEngine;

public class RepairRotate : Repairs
{
    public RepairsCameraManager repairCameraManager;
    [SerializeField] int totalRotationsRequired = 5;
    const float rotationsRequired = 360f;
    float rotationProgress = 0f;

    [SerializeField] RectTransform handleTransform;
    float rotationAngle = 0f;

    Vector2 rotationCenter;
    Vector2 lastTouchDirection;
    bool isRotating = false;

    public override void StartRepair(RepairManager _repairManager = null)
    {
        base.StartRepair(_repairManager);
        rotationProgress = 0f;
        rotationAngle = 0f;

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

    Transform GetFirstChild(Machines machine)
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
            switch (_touch.phase)
            {
                case TouchPhase.Began:
                    StartRotation(_touch.position);
                    break;
                case TouchPhase.Moved:
                    UpdateRotation(_touch.position);
                    break;
                case TouchPhase.Ended:
                    StopRotation();
                    break;
            }
        }

        if (handleTransform != null && isRotating)
        {
            handleTransform.localRotation = Quaternion.Euler(0, 0, -rotationAngle);
        }
    }

    void StartRotation(Vector2 touchPosition)
    {
        rotationCenter = touchPosition;
        lastTouchDirection = Vector2.right;
        isRotating = true;
    }

    void UpdateRotation(Vector2 touchPosition)
    {
        if (!isRotating) return;

        Vector2 _currentTouchDirection = (touchPosition - rotationCenter).normalized;
        float _angleDelta = Vector2.SignedAngle(lastTouchDirection, _currentTouchDirection);

        if (!float.IsNaN(_angleDelta))
        {
            rotationProgress += Mathf.Abs(_angleDelta);
            rotationAngle += _angleDelta;
            lastTouchDirection = _currentTouchDirection;
        }

        if (rotationProgress >= totalRotationsRequired * rotationsRequired)
        {
            FinishRepair();
        }
    }

    void StopRotation()
    {
        isRotating = false;
    }

    public override void FinishRepair()
    {
        base.FinishRepair();
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