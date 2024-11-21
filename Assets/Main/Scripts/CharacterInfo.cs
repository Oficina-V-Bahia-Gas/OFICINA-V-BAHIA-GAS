using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float interactionDistance = 4f;
    [SerializeField] LayerMask interactionLayer;

    public HudInteraction hudInteraction;

    Machines currentMachine;

    public static CharacterInfo instance;

    private void Start()
    {
        instance = this;
    }

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
        Ray _ray = new Ray(transform.position, transform.forward);
        RaycastHit _hit;

        Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.red);

        if (Physics.Raycast(_ray, out _hit, interactionDistance, interactionLayer))
        {
            Machines machine = _hit.collider.GetComponent<Machines>();

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
            currentMachine.OnUse = true;
        }
        else
        {
            Debug.LogWarning("Nenhuma máquina detectada ou HudInteraction não configurado.");
        }
    }
}