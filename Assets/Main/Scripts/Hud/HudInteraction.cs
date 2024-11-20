using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HudInteraction : MonoBehaviour
{
    public TextMeshProUGUI machineNameText;
    public Slider durabilitySlider;
    public TextMeshProUGUI statusMessageText;
    public Machines currentMachine;
    public RepairManager repairManager;

    CanvasGroup hudCanvasGroup;
    bool isHudOpen = false;

    private void Start()
    {
        hudCanvasGroup = GetComponent<CanvasGroup>();
        if (hudCanvasGroup == null)
        {
            Debug.LogError("CanvasGroup n�o encontrado no HudInteraction.");
        }
    }

    private void Update()
    {
        if (isHudOpen && currentMachine != null)
        {
            UpdateHud();

            if (currentMachine.needsRepair && !currentMachine.repairActive && !repairManager.IsRepairInProgress())
            {
                SortearConserto();
            }
        }
    }

    public void ConfigureHud(Machines machine)
    {
        if (machine == null)
        {
            Debug.LogError("M�quina inv�lida passada para ConfigureHud.");
            return;
        }

        if (machine == currentMachine && isHudOpen)
        {
            Debug.Log("HUD j� est� configurada para esta m�quina.");
            return;
        }

        currentMachine = machine;

        UpdateMachineInfo();

        if (currentMachine.needsRepair)
        {
            if (!repairManager.IsRepairInProgress())
            {
                repairManager.RaffleRepair();
                currentMachine.ActivateRepair();
                Debug.Log("Novo conserto sorteado.");
            }
            else
            {
                Debug.Log("Conserto j� est� ativo para esta m�quina.");
            }
        }
        else
        {
            Debug.Log("A m�quina n�o precisa de conserto.");
            repairManager.ResetCanvas();
        }

        OpenHud();
    }


    void UpdateHud()
    {
        if (durabilitySlider != null && currentMachine != null)
        {
            durabilitySlider.value = currentMachine.currentDurability;

            if (currentMachine.needsRepair)
            {
                statusMessageText.text = "";
            }
            else
            {
                statusMessageText.text = "A m�quina est� em perfeito estado!";
                repairManager.ResetCanvas();
            }
        }
    }

    void UpdateMachineInfo()
    {
        machineNameText.text = currentMachine.machineType.ToString();
        durabilitySlider.maxValue = currentMachine.maxDurability;
        durabilitySlider.value = currentMachine.currentDurability;

        if (currentMachine.needsRepair)
        {
            statusMessageText.text = "";
        }
        else
        {
            statusMessageText.text = "A m�quina est� em perfeito estado!";
        }
    }

    void SortearConserto()
    {
        Debug.Log("Sorteando um conserto para a m�quina.");
        repairManager.RaffleRepair();
        currentMachine.ActivateRepair();
    }

    public void OpenHud()
    {
        if (hudCanvasGroup != null && !isHudOpen)
        {
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
            hudCanvasGroup.alpha = 0;
            hudCanvasGroup.interactable = false;
            hudCanvasGroup.blocksRaycasts = false;
            isHudOpen = false;
        }

        currentMachine = null;
    }

    public bool IsHudConfiguredFor(Machines machine)
    {
        return currentMachine == machine && isHudOpen;
    }
}