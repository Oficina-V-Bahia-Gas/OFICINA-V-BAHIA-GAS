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
    float holdDuration = 2.5f;

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
            EventTrigger _trigger = repairButton.GetComponent<EventTrigger>();
            if (_trigger == null)
            {
                _trigger = repairButton.gameObject.AddComponent<EventTrigger>();
            }
            else
            {
                _trigger.triggers.Clear();
            }

            AddEventTrigger(_trigger, EventTriggerType.PointerDown, (data) => StartHolding());
            AddEventTrigger(_trigger, EventTriggerType.PointerUp, (data) => StopHolding());
        }

        UpdateUI(0);

        CharacterInfo _characterInfo = FindObjectOfType<CharacterInfo>();
        if (_characterInfo != null)
        {
            currentMachine = _characterInfo.GetLastInteractedMachine();
            if (currentMachine != null)
            {
                Transform _targetTransform = GetFirstChild(currentMachine);
                if (_targetTransform != null && repairCameraManager != null)
                {
                    repairCameraManager.SetTargetTransform(_targetTransform);
                }
            }
        }
    }

    Transform GetFirstChild(Machines _machine)
    {
        if (_machine != null && _machine.transform.childCount > 0)
        {
            return _machine.transform.GetChild(0);
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

        AudioManager.instance.Play("Press");
        AudioManager.instance.Play("Electric Hum");
        isHolding = true;
        holdProgress = 0f;
    }

    public void StopHolding()
    {
        if (isHolding)
        {
            AudioManager.instance.Play("Release");
            AudioManager.instance.Stop("Electric Hum");
        }
        isHolding = false;
    }

    public override void FinishRepair()
    {
        base.FinishRepair();
        holdProgress = 0f;
        isHolding = false;
        AudioManager.instance.Play("Release");
        AudioManager.instance.Stop("Electric Hum");

        UpdateUI(1f);

        if (repairCameraManager != null)
            repairCameraManager.ClearTarget();
    }

    private void UpdateUI(float _progress)
    {
        if (feedbackText != null)
        {
            if (_progress == 0)
                feedbackText.text = "Reinicialização necessária!";
            else if (_progress < 1)
                feedbackText.text = "Reinicializando...";
            else
                feedbackText.text = "Reinicialização concluída!";
        }

        if (indicatorLights != null)
        {
            int _lightState = Mathf.Clamp(Mathf.FloorToInt(_progress * 4), 0, 4);

            for (int i = 0; i < indicatorLights.Length; i++)
            {
                indicatorLights[i].color = (i < _lightState) ? Color.yellow : Color.red;
            }

            if (_progress >= 1)
            {
                foreach (var _light in indicatorLights)
                    _light.color = Color.green;
            }
        }
    }

    void AddEventTrigger(EventTrigger _trigger, EventTriggerType _eventType, System.Action<BaseEventData> _callback)
    {
        EventTrigger.Entry _entry = new EventTrigger.Entry { eventID = _eventType };
        _entry.callback.AddListener((data) => _callback(data));
        _trigger.triggers.Add(_entry);
    }
}