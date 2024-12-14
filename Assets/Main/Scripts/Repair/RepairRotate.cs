using UnityEngine;

public class RepairRotate : Repairs
{
    float rotationsRequired = 360f;
    float rotationProgress = 0f;

    Vector2 rotationCenter;
    Vector2 lastTouchDirection;

    bool isRotating = false;
    const float minRotationThreshold = 5f;

    public RepairsCameraManager rotateCameraManager;

    public override void StartRepair()
    {
        base.StartRepair();

        CharacterInfo _characterInfo = FindObjectOfType<CharacterInfo>();
        if (_characterInfo != null)
        {
            currentMachine = _characterInfo.GetLastInteractedMachine();
        }

        if (rotateCameraManager != null && currentMachine != null)
        {
            Transform _target = GetFirstChild(currentMachine);
            if (_target != null)
            {
                rotateCameraManager.SetTargetTransform(_target);
                rotateCameraManager.ActivateCamera();
            }
            else
            {
                Debug.LogWarning($"Nenhum filho encontrado na máquina {currentMachine.name}.");
            }
        }

        rotationCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        rotationProgress = 0f;
        isRotating = false;

        Debug.Log("Iniciando conserto de rotação.");
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

            switch (_touch.phase)
            {
                case TouchPhase.Began:
                    StartRotation(_touch.position);
                    break;

                case TouchPhase.Moved:
                    UpdateRotation(_touch.position);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    StopRotation();
                    break;
            }
        }
    }

    void StartRotation(Vector2 touchPosition)
    {
        lastTouchDirection = (touchPosition - rotationCenter).normalized;
        isRotating = true;
    }

    void UpdateRotation(Vector2 touchPosition)
    {
        if (!isRotating) return;

        Vector2 _currentTouchDirection = (touchPosition - rotationCenter).normalized;
        float _angleDelta = Vector2.SignedAngle(lastTouchDirection, _currentTouchDirection);

        if (Mathf.Abs(_angleDelta) >= minRotationThreshold)
        {
            rotationProgress += Mathf.Abs(_angleDelta);

            lastTouchDirection = _currentTouchDirection;

            if (rotationProgress >= rotationsRequired)
            {
                FinishRepair();
            }
        }
    }

    void StopRotation()
    {
        isRotating = false;
    }

    public override void FinishRepair()
    {
        base.FinishRepair();
        if (rotateCameraManager != null)
        {
            rotateCameraManager.ClearTarget();
        }

        Debug.Log("Conserto de rotação concluído.");
    }

    public override void ResetRepair()
    {
        base.ResetRepair();
        rotationProgress = 0f;
        isRotating = false;
        Debug.Log("Conserto resetado: RepairRotate");
    }
}
