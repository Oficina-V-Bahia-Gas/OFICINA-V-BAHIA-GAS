using UnityEngine;

public class SwipeCameraManager : MonoBehaviour
{
    private Transform target;

    public void SetTargetTransform(Transform newTarget)
    {
        target = newTarget;
    }

    public void ActivateCamera()
    {
        if (target != null)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
            gameObject.SetActive(true);
        }
    }

    public void ClearTarget()
    {
        target = null;
        gameObject.SetActive(false);
    }
}
