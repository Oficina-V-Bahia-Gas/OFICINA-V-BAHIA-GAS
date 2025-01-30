using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float interactionDistance = 8f;
    [SerializeField] LayerMask interactionLayer;
    //[SerializeField] GameObject interactionButton;

    public HudInteraction hudInteraction;
    private Machines currentMachine;
    private Outline lastHighlightedObject;

    public static CharacterInfo instance;
    private Accessibility accessibility;

    void Start()
    {
        instance = this;
        accessibility = FindObjectOfType<Accessibility>();

        //if (interactionButton != null)
        //{
        //    interactionButton.SetActive(false);
        //}
    }

    void Update()
    {
        DetectInteractable();

        if (accessibility != null && !accessibility.IsOutlineEnabled())
        {
            DisableLastOutline();
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
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.red);

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            Machines machine = hit.collider.GetComponent<Machines>();

            if (machine != null)
            {
                currentMachine = machine;
                //if (interactionButton != null)
                //{
                //    interactionButton.SetActive(true);
                //}

                if (accessibility != null && accessibility.IsOutlineEnabled())
                {
                    EnableOutline(machine.gameObject);
                }
                else
                {
                    DisableLastOutline();
                }
            }
        }
        else
        {
            currentMachine = null;
            //if (interactionButton != null)
            //{
            //    interactionButton.SetActive(false);
            //}

            DisableLastOutline();
        }
    }

    void EnableOutline(GameObject obj)
    {
        Outline outline = obj.GetComponent<Outline>();
        if (outline != null)
        {
            if (lastHighlightedObject != null && lastHighlightedObject != outline)
            {
                lastHighlightedObject.enabled = false;
            }

            outline.enabled = true;
            lastHighlightedObject = outline;
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