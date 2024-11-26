using UnityEngine;

public class RepairRotate : Repairs
{
    [SerializeField] float rotationsRequired = 360f;
    float rotationProgress = 0f;

    Vector2 rotationCenter;
    Vector2 lastTouchDirection;

    bool isRotating = false;
    const float minRotationThreshold = 5f;

    private void Start()
    {
        rotationCenter = new Vector2(Screen.width / 2, Screen.height / 2);
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
            Debug.Log($"Progresso de rotação: {rotationProgress}/{rotationsRequired}");

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

    public override void ResetRepair()
    {
        base.ResetRepair();
        rotationProgress = 0f;
        isRotating = false;
        Debug.Log("Conserto resetado: RepairRotate");
    }
}