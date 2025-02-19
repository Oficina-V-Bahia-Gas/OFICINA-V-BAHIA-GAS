using UnityEngine;
using UnityEngine.UI;

public class RepairTap : Repairs
{
    public RepairsCameraManager repairCameraManager;

    [SerializeField] Image[] looseScrews;
    [SerializeField] Image[] placedScrews;
    int tapCount = 0;
    const int totalTapsRequired = 8;

    public override void StartRepair(RepairManager _repairManager = null)
    {
        base.StartRepair(_repairManager);
        tapCount = 0;

        foreach (var _screw in looseScrews) _screw.gameObject.SetActive(true);
        foreach (var _hole in placedScrews) _hole.gameObject.SetActive(false);

        CharacterInfo _characterInfo = FindObjectOfType<CharacterInfo>();
        if (_characterInfo != null)
        {
            currentMachine = _characterInfo.GetLastInteractedMachine();
            if (currentMachine != null)
            {
                Transform _targetTransform = GetFirstChild(currentMachine);

                if (_targetTransform != null && repairCameraManager != null)
                    repairCameraManager.SetTargetTransform(_targetTransform);
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

    public void OnTap()
    {
        if (!repairInProgress) return;

        if (tapCount < totalTapsRequired)
        {
            int _screwIndex = tapCount / 2;

            if (tapCount % 2 == 0)
                looseScrews[_screwIndex].gameObject.SetActive(false);
            else
                placedScrews[_screwIndex].gameObject.SetActive(true);

            tapCount++;
            Debug.Log($"Tap registrado: {tapCount}/{totalTapsRequired}");
            AudioManager.instance.Play("Hammer");

            if (tapCount >= totalTapsRequired) FinishRepair();
        }
    }

    public override void FinishRepair()
    {
        base.FinishRepair();
        tapCount = 0;

        if (repairCameraManager != null)
            repairCameraManager.ClearTarget();
    }
}