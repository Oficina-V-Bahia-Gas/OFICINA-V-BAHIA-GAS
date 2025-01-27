using UnityEngine;

public class RepairRotate : Repairs
{
    public RepairsCameraManager repairCameraManager;
    float rotationsRequired = 360f;
    float rotationProgress = 0f;

    Vector2 rotationCenter;
    Vector2 lastTouchDirection;
    bool isRotating = false;

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

        if (!isRotating)
        {
            PausePlayerAnimation();
        }
    }

    void StartRotation(Vector2 touchPosition)
    {
        lastTouchDirection = (touchPosition - rotationCenter).normalized;
        isRotating = true;
        ResumePlayerAnimation();
    }

    void UpdateRotation(Vector2 touchPosition)
    {
        Vector2 _currentTouchDirection = (touchPosition - rotationCenter).normalized;
        float _angleDelta = Vector2.SignedAngle(lastTouchDirection, _currentTouchDirection);

        rotationProgress += Mathf.Abs(_angleDelta);
        lastTouchDirection = _currentTouchDirection;

        if (rotationProgress >= rotationsRequired)
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
