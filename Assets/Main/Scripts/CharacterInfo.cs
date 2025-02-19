using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float interactionDistance = 8f;
    [SerializeField] LayerMask interactionLayer;

    public HudInteraction hudInteraction;
    Machines currentMachine;

    [SerializeField] GameObject interactionButton;

    public static CharacterInfo instance;

    bool tutorial;
    Tutorial tutorialScript;
    Machines allowedMachine;
    public bool checkFixTutorial;

    void Start() => instance = this;

    void Update()
    {
        DetectInteractable();

        if (checkFixTutorial)
        {
            if ((currentMachine == allowedMachine || allowedMachine == null) && currentMachine.onCooldown)
            {
                tutorialScript.FixReturn();
                checkFixTutorial = false;
            }
        }
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
            Machines _machine = _hit.collider.GetComponent<Machines>();
            if (_machine != null)
            {
                currentMachine = _machine;
                if (interactionButton != null)
                    interactionButton.SetActive(true);
                return;
            }
        }
        currentMachine = null;
        if (interactionButton != null)
            interactionButton.SetActive(false);
    }

    public void OpenHud()
    {
        if (tutorial && currentMachine != allowedMachine && currentMachine != null)
        {
            tutorialScript.MachineInteractError(allowedMachine);
            return;
        }

        if (currentMachine != null && hudInteraction != null)
        {
            if (hudInteraction.IsHudConfiguredFor(currentMachine))
            {
                Debug.Log("HUD já está configurada para esta máquina.");
                return;
            }

            if (tutorial)
                tutorialScript.MachineReturn();

            hudInteraction.ConfigureHud(currentMachine);
            currentMachine.OnUse = true;
        }
        else
            Debug.LogWarning("Nenhuma máquina detectada ou HudInteraction não configurado.");
    }

    public void EndInteractionOnCurrentMachine()
    {
        if (currentMachine != null)
            currentMachine.OnUse = false;
    }

    public void SetTutorial(bool _b = false, Tutorial _t = null)
    {
        tutorial = _b;
        if (tutorialScript == null || _t != null)
            tutorialScript = _t;
    }

    public void SetAllowedMachine(Machines m = null) => allowedMachine = m;
}