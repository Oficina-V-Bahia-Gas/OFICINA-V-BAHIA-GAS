using UnityEngine;
using System.Collections;
using static Machines;

public class MachineDetrition : MonoBehaviour, IMachine
{
    public enum MachineState { Wear, Repair, Cooldown }
    public MachineState currentState = MachineState.Wear;

    MeshRenderer machineRenderer;

    [SerializeField] ParticleSystem smokeParticles, sparkParticles;

    [SerializeField] float wearDuration;
    [SerializeField] float repairTime = 10f;
    [SerializeField] float repairCooldown = 30f;

    float currentTime = 0f;
    float tremorIntensity = 0f;

    Vector3 initialPosition;
    Color initialColor = Color.green;
    Color purpleColor = new Color(0.5f, 0f, 0.5f);
    Color yellowColor = Color.yellow;
    Color redColor = Color.red;

    [SerializeField] PlayerInteractionController interactionController;
    [SerializeField] InteractableTrigger interactableTrigger;

    void Start()
    {
        machineRenderer = GetComponent<MeshRenderer>();
        initialPosition = transform.position;

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
            ApplyTremor();

            if (currentTime >= wearDuration / 3f && currentTime < 2f * (wearDuration / 3f) && !smokeParticles.isPlaying)
            {
                smokeParticles.Play();
                Debug.Log("Smoke particles started.");
            }
            else if ((currentTime < wearDuration / 3f || currentTime >= 2f * (wearDuration / 3f)) && smokeParticles.isPlaying)
            {
                smokeParticles.Stop();
                Debug.Log("Smoke particles stopped.");
            }
        }
        else
        {
            transform.position = initialPosition;
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

            tremorIntensity = Mathf.Lerp(0f, 0.05f, currentTime / wearDuration);
        }
        else
        {
            machineRenderer.material.color = redColor;
            tremorIntensity = 0.05f;
        }
    }

    void ApplyTremor()
    {
        if (tremorIntensity > 0f)
        {
            float _offsetX = Mathf.Sin(Time.time * 20f) * tremorIntensity;
            float _offsetY = Mathf.Sin(Time.time * 25f) * tremorIntensity;
            float _offsetZ = Mathf.Sin(Time.time * 30f) * tremorIntensity;

            transform.position = initialPosition + new Vector3(_offsetX, _offsetY, _offsetZ);
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

        sparkParticles.Play();

        while (_repairElapsedTime < repairTime)
        {
            _repairElapsedTime += Time.deltaTime;
            float _t = _repairElapsedTime / repairTime;

            machineRenderer.material.color = Color.Lerp(_startColor, initialColor, _t);

            currentTime = Mathf.Lerp(_initialRepairTime, 0f, _t);

            interactableTrigger?.UpdateRepairProgress(_t);

            yield return null;
        }

        sparkParticles.Stop();

        machineRenderer.material.color = initialColor;
        currentTime = 0f;
        currentState = MachineState.Cooldown;
        interactionController?.StopInteraction();

        Debug.Log("Repair complete. Machine has returned to its initial state.");

        StartCoroutine(StartCooldown());

        interactableTrigger?.UpdateRepairProgress(1f);
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(repairCooldown);
        currentState = MachineState.Wear;
        currentTime = 0f;
        tremorIntensity = 0f;
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

    public bool CanPerformAction(MachineAction _action)
    {
        return _action == MachineAction.Repair && currentState == MachineState.Wear && currentTime > 0f;
    }
}