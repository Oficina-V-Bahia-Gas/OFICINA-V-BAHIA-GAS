using UnityEngine;
using UnityEngine.UI;

public class RepairSwipe : Repairs
{
    public RepairsCameraManager repairCameraManager;
    public float distance = 50f;
    float swipeProgress = 0f;
    public float swipesRequired = 20f;

    [SerializeField] Image dirtOverlay;
    [SerializeField] ParticleSystem foamParticles;

    public override void StartRepair(RepairManager _repairManager = null)
    {
        base.StartRepair(_repairManager);
        swipeProgress = 0f;

        if (dirtOverlay != null)
        {
            dirtOverlay.color = new Color(dirtOverlay.color.r, dirtOverlay.color.g, dirtOverlay.color.b, 1f);
        }

        if (foamParticles != null)
        {
            foamParticles.Stop();
        }

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

    private void Update()
    {
        if (!repairInProgress) return;

        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            float _distance = Vector2.Distance(Vector2.zero, _touch.deltaPosition);

            if (_distance >= distance)
            {
                swipeProgress++;
                Debug.Log($"Swipe registrado: {swipeProgress}/{swipesRequired}");

                UpdateDirtTransparency();

                if (swipeProgress >= swipesRequired)
                {
                    FinishRepair();
                    AudioManager.instance.Stop("Brush");
                }
                else
                {
                    if(!AudioManager.instance.IsPlaying("Brush"))
                    AudioManager.instance.Play("Brush");
                }
            }
        }
        else
        {
            AudioManager.instance.Stop("Brush");
        }
    }

    void UpdateDirtTransparency()
    {
        if (dirtOverlay != null)
        {
            float progress = swipeProgress / swipesRequired;
            float newAlpha = Mathf.Lerp(1f, 0f, progress);
            dirtOverlay.color = new Color(dirtOverlay.color.r, dirtOverlay.color.g, dirtOverlay.color.b, newAlpha);
        }

        if (foamParticles != null)
        {
            if (!foamParticles.isPlaying)
            {
                foamParticles.Play();
            }
        }
    }

    public override void FinishRepair()
    {
        base.FinishRepair();
        swipeProgress = 0f;

        if (dirtOverlay != null)
        {
            dirtOverlay.color = new Color(dirtOverlay.color.r, dirtOverlay.color.g, dirtOverlay.color.b, 0f);
        }

        if (foamParticles != null)
        {
            foamParticles.Stop();
        }

        if (repairCameraManager != null)
        {
            repairCameraManager.ClearTarget();
        }
    }
}