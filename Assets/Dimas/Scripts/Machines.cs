using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Machines : MonoBehaviour
{
    public enum MachineType { BombaHidraulica, Transmissor, Valvula, Outro }
    public MachineType machineType;

    //[HideInInspector] public enum RepairType { Type1, Type2, Type3, Type4 }

    [Header("Repairs")]
    public List<RepairManager.RepairType> coreRepairs = new List<RepairManager.RepairType>();
    [Tooltip("Quanto começa a poder reparar a máquina")] [Range(0f, 1f)] public float coreRepairsStart = 0.65f;
    [Tooltip("Quanto consertos precisa inicialmente")] public int coreRepairsAmount = 1;
    [Tooltip("Ganho de pontuação de reparo inicialmente")] public int coreRepairsScore = 25;
    [Tooltip("Passagem de gás quando parcialmente quebrado")] public float coreGasFlow = 0.9f;
    [Space]
    public List<RepairManager.RepairType> randomRepairs = new List<RepairManager.RepairType>();
    [Tooltip("Quando começa a ter consertos aleatórios")][Range(0f, 1f)] public float randomRepairsStart = 0.3f;
    [Tooltip("Quantos consertos extras são adicionados")] public int randomRepairsAmount = 0;
    [Tooltip("Ganho de pontuação de reparo quando parcialmente quebrado")] public int randomRepairsScore = 27;
    [Tooltip("Passagem de gás quando parcialmente quebrado")] public float randomGasFlow = 0.5f;
    [Space]
    public List<RepairManager.RepairType> fullRepairs = new List<RepairManager.RepairType>();
    [Tooltip("Conserto prioritário quando está totalmente quebrado")][Range(0f, 1f)] public float fullRepairsStart = 0f;
    [Tooltip("Quantos consertos extras são adicionados")] public int fullRepairsAmount = 0;
    [Tooltip("Ganho de pontuação de reparo quando totalmente quebrado")] public int fullRepairsScore = 32;
    [Tooltip("Passagem de gás quando totalmente quebrado")] public float fullGasFlow = 0f;

    [HideInInspector] public List<RepairManager.RepairType> currentRepairs = new List<RepairManager.RepairType>();
    private bool coreRoll = false;
    private bool randomRoll = false;
    private bool fullRoll = false;


    [Header("Durability")]
    public float currentDurability;
    public float maxDurability;


    [Header("Durability Ranges")]
    public float minDurability = 20f;
    public float maxDurabilityRange = 40f;

    public bool needsRepair { get; private set; }
    public bool onCooldown;


    [Header("Repair Cooldown Range")]
    public float minCooldown = 5f;
    public float maxCooldown = 20f;
    float repairCooldown;

    bool onUse = false;

    public bool OnUse { get { return onUse; }  set { onUse = value; } }

    bool activatedCanvas = false;

    public bool repairActive { get; private set; }

    private GameManager gameManager;


    private void Start()
    {
        gameObject.layer = 6;
        SetDurability();
        repairCooldown = Random.Range(minCooldown, maxCooldown);
        needsRepair = CheckDurability();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void Update()
    {
        if (currentDurability > 0 && !onCooldown)
        {
            currentDurability -= Time.deltaTime;
        }

        SetRepairs();

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
            if (GetComponent<GasFlow>() != null)
            {
                GetComponent<GasFlow>().ChangeFixValue(1f);
            }
            activatedCanvas = false;
        }

        // Ajuste de GasFlow
        if (needsRepair)
        {
            if (GetComponent<GasFlow>() != null)
            {
                float _gasvalue = coreGasFlow;

                if (CheckDurability(randomRepairsStart)) _gasvalue = randomGasFlow;
                if (CheckDurability(fullRepairsStart)) _gasvalue = fullGasFlow;

                GetComponent<GasFlow>().ChangeFixValue(_gasvalue);
            }
        }
        else 
        {
            if (GetComponent<GasFlow>() != null)
            {
                GetComponent<GasFlow>().ChangeFixValue(1f);
            }
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
                if (gameManager)
                {
                    float _gain = coreRepairsScore;
                    if (CheckDurability(randomRepairsStart)) _gain = randomRepairsScore;
                    if (CheckDurability(fullRepairsStart)) _gain = fullRepairsScore;
                    gameManager.ScoreGain(_gain);
                }
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
            _durability = coreRepairsStart; // padrão
        }

        Mathf.Clamp(_durability, 0f, 1f);

        return (currentDurability / maxDurability < _durability);
    }
}