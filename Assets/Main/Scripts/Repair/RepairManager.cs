using System.Collections.Generic;
using UnityEngine;

public class RepairManager : MonoBehaviour
{
    public enum RepairType { Type1, Type2, Type3, Type4 }
    public List<RepairType> repairTypes = new List<RepairType>();
    RepairType currentRepair;
    Repairs currentRepairScript;

    public RepairHold repairHold;
    public RepairTap repairTap;
    public RepairRotate repairRotate;
    public RepairSwipe repairSwipe;

    public CanvasGroup[] canvasGroupsRepairs;
    bool repairInProgress = false;

    public void RaffleRepair()
    {
        if (repairInProgress)
        {
            Debug.Log("Um conserto já está em andamento.");
            return;
        }

        if (repairTypes.Count == 0)
        {
            Debug.LogError("Nenhum tipo de conserto disponível para sortear.");
            return;
        }

        int index = Random.Range(0, repairTypes.Count);
        currentRepair = repairTypes[index];

        ActivateCanvas(index);

        currentRepairScript = GetRepairScript(currentRepair);
        if (currentRepairScript != null)
        {
            currentRepairScript.StartRepair();
            repairInProgress = true;
            Debug.Log($"Conserto sorteado e iniciado: {currentRepair}");
        }
        else
        {
            Debug.LogError("Script do conserto não foi encontrado.");
        }
    }

    public bool IsRepairInProgress()
    {
        return repairInProgress;
    }

    public void NotifyRepairComplete()
    {
        if (!repairInProgress)
        {
            Debug.LogWarning("Nenhum conserto em andamento para finalizar.");
            return;
        }

        Debug.Log($"Conserto finalizado: {currentRepair}");
        ResetCanvas();
        repairInProgress = false;

        if (HudInteraction.instance.currentMachine != null)
        {
            HudInteraction.instance.currentMachine.Repair();
        }
    }

    Repairs GetRepairScript(RepairType type)
    {
        switch (type)
        {
            case RepairType.Type1: return repairHold;
            case RepairType.Type2: return repairTap;
            case RepairType.Type3: return repairRotate;
            case RepairType.Type4: return repairSwipe;
            default: return null;
        }
    }

    void ActivateCanvas(int index)
    {
        for (int i = 0; i < canvasGroupsRepairs.Length; i++)
        {
            if (i == index)
            {
                canvasGroupsRepairs[i].alpha = 1;
                canvasGroupsRepairs[i].interactable = true;
                canvasGroupsRepairs[i].blocksRaycasts = true;
                Debug.Log($"Canvas ativado para o conserto: {index}");
            }
            else
            {
                canvasGroupsRepairs[i].alpha = 0;
                canvasGroupsRepairs[i].interactable = false;
                canvasGroupsRepairs[i].blocksRaycasts = false;
            }
        }
    }

    public void ResetCanvas()
    {
        foreach (var _canvasGroup in canvasGroupsRepairs)
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}