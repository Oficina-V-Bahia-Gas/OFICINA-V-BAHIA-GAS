using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Machines : MonoBehaviour
{
    public enum MachineType { BombaHidraulica, Transmissor, Outro }
    public MachineType machineType;

    //[HideInInspector] public enum RepairType { Type1, Type2, Type3, Type4 }

    [Header("Repairs")]
    public List<RepairManager.RepairType> coreRepairs = new List<RepairManager.RepairType>();
    [Tooltip("Quanto come�a a poder reparar a m�quina")] [Range(0f, 1f)] public float coreRepairsStart = 0.65f;
    [Tooltip("Quanto consertos precisa inicialmente")] public int coreRepairsAmount = 1;
    [Space]
    public List<RepairManager.RepairType> randomRepairs = new List<RepairManager.RepairType>();
    [Tooltip("Quando come�a a ter consertos aleat�rios")][Range(0f, 1f)] public float randomRepairsStart = 0.3f;
    [Tooltip("Quantos consertos extras s�o adicionados")] public int randomRepairsAmount = 0;
    [Tooltip("Passagem de g�s quando parcialmente quebrado")] public float randomGasFlow = 0.5f;
    [Space]
    public List<RepairManager.RepairType> fullRepairs = new List<RepairManager.RepairType>();
    [Tooltip("Conserto priorit�rio quando est� totalmente quebrado")][Range(0f, 1f)] public float fullRepairsStart = 0f;
    [Tooltip("Quantos consertos extras s�o adicionados")] public int fullRepairsAmount = 0;
    [Tooltip("Passagem de g�s quando totalmente quebrado")] public float fullGasFlow = 0f;

    [HideInInspector] public List<RepairManager.RepairType> currentRepairs = new List<RepairManager.RepairType>();
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
            SetRepairs();

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
            GetComponent<GasFlow>().ChangeFixValue(1f);
            activatedCanvas = false;
        }

        // Ajuste de GasFlow
        if (needsRepair)
        {
            float _gasvalue = (CheckDurability(fullRepairsAmount)) ? fullGasFlow : randomGasFlow;
            GetComponent<GasFlow>().ChangeFixValue(_gasvalue);
        }
        else 
        {
            GetComponent<GasFlow>().ChangeFixValue(1f);
        }
    }

    public void Repair()
    {
        if (needsRepair && !onCooldown)
        {
            currentRepairs.RemoveAt(0);
            if (currentRepairs.Count == 0)
            {
                needsRepair = false;
                DeactivateRepair();
                SetDurability();
                StartCoroutine(RepairCooldown());
            }
            else
            {
                CharacterInfo.instance.hudInteraction.repairManager.RaffleRepair();
            }
        }
    }

    IEnumerator RepairCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(repairCooldown);
        repairCooldown = Random.Range(minCooldown, maxCooldown);
        onCooldown = false;
    }

    [System.Obsolete("M�todo repetido. Use CheckDurability() ao inv�s disso.", false)]
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

        //GasFlow _gasFlow = GetComponent<GasFlow>();
        //_gasFlow.ChangeFixValue(0f);
    }

    public void DeactivateRepair()
    {
        repairActive = false;

        //GasFlow _gasFlow = GetComponent<GasFlow>();
        //_gasFlow.ChangeFixValue(1f);
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
            if (!coreRoll && coreRepairs.Count > 0)
            {
                for (int _i = 0; _i < coreRepairsAmount; _i++)
                {

                    _index = Random.Range(0, coreRepairs.Count);
                    currentRepairs.Add(coreRepairs[_index]);
                }

                coreRoll = true;
            }

            // Random
            if (CheckDurability(randomRepairsStart) && !randomRoll && randomRepairs.Count > 0)
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
                    currentRepairs.Remove(currentRepairs[currentRepairs.Count - 1]);
                    _index = Random.Range(0, randomRepairs.Count);
                    currentRepairs.Add(randomRepairs[_index]);
                }

                randomRoll = true;
            }

            // Full
            if (CheckDurability(fullRepairsStart) && !fullRoll && fullRepairs.Count > 0)
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
                    currentRepairs.Remove(currentRepairs[currentRepairs.Count - 1]);
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
        //Debug.Log(currentRepairs);
    }

    // Checar se durabilidade abaixo do alvo.
    public bool CheckDurability(float _durability = -1f)
    {
        if (_durability == -1f)
        {
            _durability = coreRepairsStart; // padr�o
        }

        Mathf.Clamp(_durability, 0f, 1f);

        return (currentDurability / maxDurability < _durability);
    }
}