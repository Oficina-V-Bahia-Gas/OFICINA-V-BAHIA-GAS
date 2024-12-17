using UnityEngine;

public class RepairRotate : Repairs
{
    float rotationsRequired = 360f;
    float rotationProgress = 0f;

    Vector2 rotationCenter;
    Vector2 lastTouchDirection;
    bool isRotating = false;

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

    protected override void PlayAnimation(string animationName)
    {
        currentMachine?.PlayAnimation(animationName);
    }
}
