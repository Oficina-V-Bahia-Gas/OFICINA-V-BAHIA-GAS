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

        foreach (var screw in looseScrews) screw.gameObject.SetActive(true);
        foreach (var hole in placedScrews) hole.gameObject.SetActive(false);

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

    Transform GetFirstChild(Machines machine)
    {
        if (machine != null && machine.transform.childCount > 0)
        {
            return machine.transform.GetChild(0);
        }
        return null;
    }

    public void OnTap()
    {
        if (!repairInProgress) return;

        if (tapCount < totalTapsRequired)
        {
            int screwIndex = tapCount / 2;

            if (tapCount % 2 == 0)
            {
                looseScrews[screwIndex].gameObject.SetActive(false);
            }
            else
            {
                placedScrews[screwIndex].gameObject.SetActive(true);
            }

            tapCount++;
            Debug.Log($"Tap registrado: {tapCount}/{totalTapsRequired}");
            AudioManager.instance.Play("Hammer");

            if (tapCount >= totalTapsRequired)
            {
                FinishRepair();
            }
        }
    }

    public override void FinishRepair()
    {
        base.FinishRepair();
        tapCount = 0;

        if (repairCameraManager != null)
        {
            repairCameraManager.ClearTarget();
        }
    }
}