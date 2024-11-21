using UnityEngine;
using System.Collections;

public class Machines : MonoBehaviour
{
    public enum MachineType { HydraulicPump, Transmitter, Other }
    public MachineType machineType;

    [Header("Durability")]
    public float currentDurability;
    public float maxDurability;

    [Header("Durability Ranges")]
    public float minDurability = 5f;
    public float maxDurabilityRange = 10f;

    public bool needsRepair { get; private set; }
    bool onCooldown;

    [Header("Repair Cooldown Range")]
    public float minCooldown = 5f;
    public float maxCooldown = 7f;
    float repairCooldown;

    public bool repairActive { get; private set; }

    private void Start()
    {
        SetRandomDurability();
        repairCooldown = Random.Range(minCooldown, maxCooldown);
        CheckRepairStatus();
    }

    private void Update()
    {
        if (currentDurability > 0)
        {
            currentDurability -= Time.deltaTime;
        }
        else if (!needsRepair && !onCooldown)
        {
            needsRepair = true;
            ActivateRepair();
        }
    }

    public void Repair()
    {
        if (needsRepair && !onCooldown)
        {
            needsRepair = false;
            DeactivateRepair();
            onCooldown = true;
            SetRandomDurability();
            StartCoroutine(RepairCooldown());
        }
    }

    IEnumerator RepairCooldown()
    {
        yield return new WaitForSeconds(repairCooldown);
        repairCooldown = Random.Range(minCooldown, maxCooldown);
        onCooldown = false;
    }

    void CheckRepairStatus()
    {
        needsRepair = currentDurability <= 0;
    }

    void SetRandomDurability()
    {
        maxDurability = Random.Range(minDurability, maxDurabilityRange);
        currentDurability = maxDurability;
    }

    public void ActivateRepair()
    {
        repairActive = true;
        GasFlow _gasFlow = GetComponent<GasFlow>();
        _gasFlow.ChangeFixValue(0f);
    }

    public void DeactivateRepair()
    {
        repairActive = false;
        GasFlow _gasFlow = GetComponent<GasFlow>();
        _gasFlow.ChangeFixValue(1f);
    }
}