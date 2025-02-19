using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float interactionDistance = 8f;
    [SerializeField] LayerMask interactionLayer;

    public HudInteraction hudInteraction;
    Machines currentMachine;
    Outline lastHighlightedObject;

    public static CharacterInfo instance;
    Accessibility accessibility;

    private bool tutorial;
    private Tutorial tutorialScript;
    private Machines allowedMachine;
    public bool checkFixTutorial;

    void Start()
    {
        instance = this;
        accessibility = FindObjectOfType<Accessibility>();
    }

    void Update()
    {
        DetectInteractable();

        if (accessibility != null && !accessibility.IsOutlineEnabled())
        {
            DisableLastOutline();
        }

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
                if (accessibility != null && accessibility.IsOutlineEnabled() && !_machine.OnUse)
                    EnableOutline(_machine.gameObject);
                else
                    DisableLastOutline();
                return; 
            }
        }


        currentMachine = null;
        DisableLastOutline();
    }

    void EnableOutline(GameObject obj)
    {
        Outline _outline = obj.GetComponent<Outline>();
        if (_outline != null)
        {
            if (lastHighlightedObject != null && lastHighlightedObject != _outline)
                lastHighlightedObject.enabled = false;

            _outline.enabled = true;
            lastHighlightedObject = _outline;
        }
    }

    void DisableLastOutline()
    {
        if (lastHighlightedObject != null)
        {
            lastHighlightedObject.enabled = false;
            lastHighlightedObject = null;
        }
    }

    void DisableAllOutlines()
    {
        Outline[] _outlines = FindObjectsOfType<Outline>();
        foreach (Outline _outline in _outlines)
            _outline.enabled = false;
    }

    public void OpenHud()
    {
        DisableAllOutlines();

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

    public void SetTutorial(bool b = false, Tutorial t = null)
    {
        tutorial = b;
        if (tutorialScript == null || t != null)
            tutorialScript = t;
    }

    public void SetAllowedMachine(Machines m = null) => allowedMachine = m;
}