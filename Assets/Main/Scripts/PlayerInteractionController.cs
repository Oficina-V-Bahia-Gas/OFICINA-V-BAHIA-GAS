using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public bool IsInteracting { get; private set; }

    public void StartInteraction() => IsInteracting = true;

    public void StopInteraction() => IsInteracting = false;
}