using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HudInteraction : MonoBehaviour
{
    public TextMeshProUGUI machineNameText;
    public Slider durabilitySlider;
    public Slider gasFlowSlider;
    public TextMeshProUGUI gasFlowText;
    public TextMeshProUGUI statusMessageText;
    public Machines currentMachine;
    public RepairManager repairManager;

    [SerializeField] GameObject PauseMenuButton;

    CanvasGroup hudCanvasGroup;
    public bool isHudOpen = false;

    public static HudInteraction instance;

    private void Start()
    {
        instance = this;

        hudCanvasGroup = GetComponent<CanvasGroup>();
        if (hudCanvasGroup == null)
            Debug.LogError("CanvasGroup não encontrado no HudInteraction.");
    }

    private void Update()
    {
        if (isHudOpen && currentMachine != null)
        {
            UpdateHud();

            if (currentMachine.needsRepair && !currentMachine.repairActive && !repairManager.IsRepairInProgress())
            {
                repairManager.RaffleRepair();
                currentMachine.ActivateRepair();
            }
        }
    }

    public void ConfigureHud(Machines _machine)
    {
        if (_machine == null)
        {
            Debug.LogError("Máquina inválida passada para ConfigureHud.");
            return;
        }

        if (_machine == currentMachine && isHudOpen)
        {
            Debug.Log("HUD já está configurada para esta máquina.");
            return;
        }

        currentMachine = _machine;

        UpdateMachineInfo();

        if (currentMachine.needsRepair)
        {
            if (!repairManager.IsRepairInProgress())
            {
                currentMachine.ActivateRepair();
                Debug.Log("Novo conserto sorteado.");
            }
            else
            {
                Debug.Log("Conserto já está ativo para esta máquina.");
            }
        }
        else
        {
            Debug.Log("A máquina não precisa de conserto.");
            repairManager.ResetCanvas();
        }

        OpenHud();
    }


    void UpdateHud()
    {
        if (durabilitySlider != null && currentMachine != null)
        {
            durabilitySlider.value = currentMachine.currentDurability;

            if (currentMachine.GetComponent<GasFlow>() != null)
            {
                gasFlowSlider.enabled = true;
                gasFlowText.text = "Fluxo de Gás:";
                gasFlowSlider.value = currentMachine.GetComponent<GasFlow>().currentFlow;
            }
            else
            {
                gasFlowSlider.enabled = false;
                gasFlowText.text = "Máquina não tem passagem de gás.";
            }

            if (currentMachine.onCooldown)
                durabilitySlider.value = durabilitySlider.maxValue;

            if (currentMachine.needsRepair)
                statusMessageText.text = "";
            else
            {
                statusMessageText.text = "A máquina está em perfeito estado!";
                repairManager.ResetCanvas();
            }
        }
    }

    void UpdateMachineInfo()
    {
        machineNameText.text = currentMachine.machineType.ToString();
        durabilitySlider.maxValue = currentMachine.maxDurability;
        durabilitySlider.value = currentMachine.currentDurability;

        if (currentMachine.GetComponent<GasFlow>() != null)
        {
            gasFlowSlider.enabled = true;
            gasFlowText.text = "Fluxo de Gás:";
            gasFlowSlider.value = currentMachine.GetComponent<GasFlow>().currentFlow;
        }
        else
        {
            gasFlowSlider.enabled = false;
            gasFlowText.text = "Máquina não tem passagem de gás.";
        }

        if (currentMachine.needsRepair)
            statusMessageText.text = "";
        else
        {
            durabilitySlider.value = durabilitySlider.maxValue;
            statusMessageText.text = "A máquina está em perfeito estado!";
        }

        if (currentMachine.onCooldown)
            durabilitySlider.value = durabilitySlider.maxValue;
    }

    public void OpenHud()
    {
        if (hudCanvasGroup != null && !isHudOpen)
        {
            AudioManager.instance.Stop("Close");
            AudioManager.instance.Play("Open");
            PauseMenuButton.gameObject.SetActive(false);
            hudCanvasGroup.alpha = 1;
            hudCanvasGroup.interactable = true;
            hudCanvasGroup.blocksRaycasts = true;
            isHudOpen = true;
        }
    }

    public void CloseHud()
    {
        if (hudCanvasGroup != null && isHudOpen)
        {
            AudioManager.instance.Stop("Electric Hum");
            AudioManager.instance.Stop("Squeak");
            AudioManager.instance.Stop("Brush");

            AudioManager.instance.Stop("Open");
            AudioManager.instance.Play("Close");
            hudCanvasGroup.alpha = 0;
            hudCanvasGroup.interactable = false;
            hudCanvasGroup.blocksRaycasts = false;
            isHudOpen = false;
            PauseMenuButton.gameObject.SetActive(true);
        }

        repairManager.StopRepair();

        currentMachine.OnUse = false;
        currentMachine.SetCanvasActivated(false);

        currentMachine = null;
    }

    public bool IsHudConfiguredFor(Machines _machine)
    {
        return currentMachine == _machine && isHudOpen;
    }
}