using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public bool IsInteracting { get; private set; }

    public void StartInteraction()
    {
        IsInteracting = true;
        Debug.Log("Player interaction started.");
    }

    public void StopInteraction()
    {
        IsInteracting = false;
        Debug.Log("Player interaction stopped.");
    }
}