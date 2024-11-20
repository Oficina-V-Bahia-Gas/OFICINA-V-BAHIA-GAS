using UnityEngine;

public class RepairRotate : Repairs
{
    float rotationProgress = 0f;
    public float rotationsRequired = 360f;

    Vector2 lastTouchPosition;
    bool isRotating = false;

    private void Update()
    {
        if (!repairInProgress) return;

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
    }

    void StartRotation(Vector2 touchPosition)
    {
        lastTouchPosition = touchPosition;
        isRotating = true;
    }

    void UpdateRotation(Vector2 touchPosition)
    {
        if (!isRotating) return;

        Vector2 delta = touchPosition - lastTouchPosition;
        float rotationDelta = Mathf.Abs(delta.x) + Mathf.Abs(delta.y);
        rotationProgress += rotationDelta;
        lastTouchPosition = touchPosition;

        Debug.Log($"Progresso de rotação: {rotationProgress}/{rotationsRequired}");

        if (rotationProgress >= rotationsRequired)
        {
            FinishRepair();
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
        Debug.Log("Resetando conserto: RepairRotate");
    }
}