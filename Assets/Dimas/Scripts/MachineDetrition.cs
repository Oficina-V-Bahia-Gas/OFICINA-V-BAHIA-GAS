using UnityEngine;
using System.Collections;
using static Machines;

public class MachineDetrition : MonoBehaviour, IMachine
{
    public enum MachineState { Wear, Repair, Cooldown }
    public MachineState currentState = MachineState.Wear;

    MeshRenderer machineRenderer;
    [SerializeField] float wearDuration;
    [SerializeField] float repairTime = 10f;
    [SerializeField] float repairCooldown = 30f;
    [SerializeField] PlayerInteractionController interactionController;
    float currentTime = 0f;
    Color initialColor = Color.green;
    Color purpleColor = new Color(0.5f, 0f, 0.5f);
    Color yellowColor = Color.yellow;
    Color redColor = Color.red;

    void Start()
    {
        machineRenderer = GetComponent<MeshRenderer>();

        if (machineRenderer != null)
        {
            machineRenderer.material.color = initialColor;
        }
    }

    void Update()
    {
        if (currentState == MachineState.Wear)
        {
            ApplyWear();
        }
    }

    void ApplyWear()
    {
        if (currentTime < wearDuration)
        {
            currentTime += Time.deltaTime;

            if (currentTime <= wearDuration / 3f)
            {
                machineRenderer.material.color = Color.Lerp(initialColor, yellowColor, currentTime / (wearDuration / 3f));
            }
            else if (currentTime <= 2f * (wearDuration / 3f))
            {
                float _t = (currentTime - (wearDuration / 3f)) / (wearDuration / 3f);
                machineRenderer.material.color = Color.Lerp(yellowColor, purpleColor, _t);
            }
            else
            {
                float _t = (currentTime - 2f * (wearDuration / 3f)) / (wearDuration / 3f);
                machineRenderer.material.color = Color.Lerp(purpleColor, redColor, _t);
            }
        }
        else
        {
            machineRenderer.material.color = redColor;
        }
    }

    void ApplyRepair()
    {
        StartCoroutine(RepairCoroutine());
    }

    IEnumerator RepairCoroutine()
    {
        float _initialRepairTime = currentTime;
        float _repairElapsedTime = 0f;
        Color _startColor = machineRenderer.material.color;

        while (_repairElapsedTime < repairTime)
        {
            _repairElapsedTime += Time.deltaTime;
            float _t = _repairElapsedTime / repairTime;

            machineRenderer.material.color = Color.Lerp(_startColor, initialColor, _t);

            currentTime = Mathf.Lerp(_initialRepairTime, 0f, _t);

            yield return null;
        }

        machineRenderer.material.color = initialColor;
        currentTime = 0f;
        currentState = MachineState.Cooldown;
        interactionController?.StopInteraction();

        Debug.Log("Repair complete. Machine has returned to its initial state.");

        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(repairCooldown);
        currentState = MachineState.Wear;
        currentTime = 0f;
    }

    public void PerformAction(MachineAction _action)
    {
        if (_action == MachineAction.Repair && currentState == MachineState.Wear && currentTime > 0f)
        {
            currentState = MachineState.Repair;
            interactionController?.StartInteraction();
            ApplyRepair();
        }
        else if (currentState == MachineState.Wear && currentTime <= 0f)
        {
            Debug.LogWarning("Cannot repair: The machine has not started wearing down yet.");
        }
    }
}