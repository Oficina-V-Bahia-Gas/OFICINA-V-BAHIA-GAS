using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float interactionDistance = 4f;
    [SerializeField] LayerMask interactionLayer;

    public HudInteraction hudInteraction;

    Machines currentMachine;

    void Update()
    {
        DetectInteractable();
    }

    public float GetWalkSpeed()
    {
        return walkSpeed;
    }

    public Machines GetLastInteractedMachine()
    {
        return currentMachine;
    }

    void DetectInteractable()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.red);

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            Machines machine = hit.collider.GetComponent<Machines>();

            if (machine != null && machine != currentMachine)
            {
                currentMachine = machine;
            }
        }
        else
        {
            currentMachine = null;
        }
    }

    public void OpenHud()
    {
        if (currentMachine != null && hudInteraction != null)
        {
            if (hudInteraction.IsHudConfiguredFor(currentMachine))
            {
                Debug.Log("HUD já está configurada para esta máquina.");
                return;
            }

            hudInteraction.ConfigureHud(currentMachine);
        }
        else
        {
            Debug.LogWarning("Nenhuma máquina detectada ou HudInteraction não configurado.");
        }
    }
}