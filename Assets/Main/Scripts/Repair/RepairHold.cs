using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RepairHold : Repairs
{
    public RepairsCameraManager repairCameraManager;

    [Header("Configurações de Tempo")]
    bool isHolding = false;
    float holdProgress = 0f;
    public float holdDuration = 5f;

    [Header("UI do Conserto")]
    [SerializeField] Button repairButton;
    [SerializeField] TextMeshProUGUI feedbackText;
    [SerializeField] Image[] indicatorLights;

    public override void StartRepair(RepairManager _repairManager = null)
    {
        base.StartRepair(_repairManager);
        holdProgress = 0f;
        isHolding = false;

        if (repairButton != null)
        {
            EventTrigger trigger = repairButton.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = repairButton.gameObject.AddComponent<EventTrigger>();
            }
            else
            {
                trigger.triggers.Clear();
            }

            AddEventTrigger(trigger, EventTriggerType.PointerDown, (data) => StartHolding());
            AddEventTrigger(trigger, EventTriggerType.PointerUp, (data) => StopHolding());
        }

        UpdateUI(0);

        CharacterInfo characterInfo = FindObjectOfType<CharacterInfo>();
        if (characterInfo != null)
        {
            currentMachine = characterInfo.GetLastInteractedMachine();
            if (currentMachine != null)
            {
                Transform targetTransform = GetFirstChild(currentMachine);
                if (targetTransform != null && repairCameraManager != null)
                {
                    repairCameraManager.SetTargetTransform(targetTransform);
                }
            }
        }
    }

    private Transform GetFirstChild(Machines machine)
    {
        if (machine != null && machine.transform.childCount > 0)
        {
            return machine.transform.GetChild(0);
        }
        return null;
    }

    private void Update()
    {
        if (repairInProgress && isHolding)
        {
            holdProgress += Time.deltaTime;
            UpdateUI(holdProgress / holdDuration);

            if (holdProgress >= holdDuration)
            {
                StopHolding();
                FinishRepair();
            }
        }
    }

    public void StartHolding()
    {
        if (!repairInProgress) return;

        isHolding = true;
        holdProgress = 0f;
    }

    public void StopHolding()
    {
        isHolding = false;
    }

    public override void FinishRepair()
    {
        base.FinishRepair();
        holdProgress = 0f;
        isHolding = false;

        UpdateUI(1f);

        if (repairCameraManager != null)
        {
            repairCameraManager.ClearTarget();
        }
    }

    private void UpdateUI(float progress)
    {
        if (feedbackText != null)
        {
            if (progress == 0)
                feedbackText.text = "Reinicialização necessária!";
            else if (progress < 1)
                feedbackText.text = "Reinicializando...";
            else
                feedbackText.text = "Reinicialização concluída!";
        }

        if (indicatorLights != null)
        {
            int lightState = Mathf.Clamp(Mathf.FloorToInt(progress * 4), 0, 4);

            for (int i = 0; i < indicatorLights.Length; i++)
            {
                indicatorLights[i].color = (i < lightState) ? Color.yellow : Color.red;
            }

            if (progress >= 1)
            {
                foreach (var light in indicatorLights)
                {
                    light.color = Color.green;
                }
            }
        }
    }

    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((data) => callback(data));
        trigger.triggers.Add(entry);
    }
}