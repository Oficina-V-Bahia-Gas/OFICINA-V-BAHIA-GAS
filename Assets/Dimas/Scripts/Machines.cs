using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Machines : MonoBehaviour
{
    public enum MachineType { BombaHidraulica, Transmissor, Outro }
    public MachineType machineType;

    [HideInInspector] public enum RepairType { Type1, Type2, Type3, Type4 }

    [Header("Repairs")]
    public List<RepairType> coreRepairs = new List<RepairType>();
    [Tooltip("Quanto começa a poder reparar a máquina")] [Range(0f, 1f)] public float coreRepairsStart = 0.65f;
    [Tooltip("Quanto consertos precisa inicialmente")] public int coreRepairsAmount = 1;
    public List<RepairType> randomRepairs = new List<RepairType>();
    [Tooltip("Quando começa a ter consertos aleatórios")][Range(0f, 1f)] public float randomRepairsStart = 0.3f;
    [Tooltip("Quantos consertos extras são adicionados")] public int randomRepairsAmount = 0;
    public List<RepairType> fullRepairs = new List<RepairType>();
    [Tooltip("Conserto prioritário quando está totalmente quebrado")][Range(0f, 1f)] public float fullRepairsStart = 0f;
    [Tooltip("Quantos consertos extras são adicionados")] public int fullRepairsAmount = 0;

    [HideInInspector] public List<RepairType> currentRepairs = new List<RepairType>();
    private bool coreRoll = false;
    private bool randomRoll = false;
    private bool fullRoll = false;


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

    bool onUse = false;

    public bool OnUse { get { return onUse; }  set { onUse = value; } }

    bool activatedCanvas = false;

    public bool repairActive { get; private set; }

    private void Start()
    {
        SetDurability();
        repairCooldown = Random.Range(minCooldown, maxCooldown);
        needsRepair = CheckDurability();
    }

    private void Update()
    {
        if (currentDurability > 0 && !onCooldown)
        {
            currentDurability -= Time.deltaTime;
        }

        if (CheckDurability()) 
        {
            if (!needsRepair && !onCooldown)
            {
                needsRepair = true;
                ActivateRepair();
            }

            if (OnUse && !activatedCanvas)
            {
                CharacterInfo.instance.hudInteraction.repairManager.RaffleRepair();
                activatedCanvas = true;
            }
        }
        else
        {
            activatedCanvas = false;
        }
    }

    public void Repair()
    {
        if (needsRepair && !onCooldown)
        {
            needsRepair = false;
            DeactivateRepair();
            SetDurability();
            StartCoroutine(RepairCooldown());
        }
    }

    IEnumerator RepairCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(repairCooldown);
        repairCooldown = Random.Range(minCooldown, maxCooldown);
        onCooldown = false;
    }

    [System.Obsolete("Método repetido. Use CheckDurability() ao invés disso.", false)]
    void CheckRepairStatus() // APAGAR
    {
        needsRepair = CheckDurability();
    }

    void SetDurability()
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

    public bool IsCanvasActivated()
    {
        return activatedCanvas;
    }

    public void SetCanvasActivated(bool state)
    {
        activatedCanvas = state;
    }

    public void SetRepairs()
    {
        if (CheckDurability())
        {
            int _index;

            // Core
            if (!coreRoll)
            {
                for (int _i = 0; _i < coreRepairsAmount; _i++)
                {
                    _index = Random.Range(0, coreRepairs.Count);
                    currentRepairs.Add(coreRepairs[_index]);
                }

                coreRoll = true;
            }

            // Random
            if (CheckDurability(randomRepairsStart) && !randomRoll)
            {
                if(randomRepairsAmount > 0)
                {
                    for (int _i = 0; _i < randomRepairsAmount; _i++)
                    {
                        _index = Random.Range(0, randomRepairs.Count);
                        currentRepairs.Add(randomRepairs[_index]);
                    }
                }
                else 
                {
                    currentRepairs.Remove(currentRepairs[currentRepairs.Count]);
                    _index = Random.Range(0, randomRepairs.Count);
                    currentRepairs.Add(randomRepairs[_index]);
                }

                randomRoll = true;
            }

            // Full
            if (CheckDurability(fullRepairsStart) && !fullRoll)
            {
                if (fullRepairsAmount > 0)
                {
                    for (int _i = 0; _i < fullRepairsAmount; _i++)
                    {
                        _index = Random.Range(0, fullRepairs.Count);
                        currentRepairs.Add(fullRepairs[_index]);
                    }
                }
                else
                {
                    currentRepairs.Remove(currentRepairs[currentRepairs.Count]);
                    _index = Random.Range(0, fullRepairs.Count);
                    currentRepairs.Add(fullRepairs[_index]);
                }

                fullRoll = true;
            }
        }
        else
        {
            currentRepairs.Clear();

            coreRoll = false;
            randomRoll = false;
            fullRoll = false;
        }
        Debug.Log(currentRepairs);
    }

    // Checar se durabilidade abaixo do alvo.
    public bool CheckDurability(float _durability = -1f)
    {
        if (_durability == -1f)
        {
            _durability = coreRepairsStart; // padrão
        }

        Mathf.Clamp(_durability, 0f, 1f);

        return (currentDurability / maxDurability < _durability);
    }
}