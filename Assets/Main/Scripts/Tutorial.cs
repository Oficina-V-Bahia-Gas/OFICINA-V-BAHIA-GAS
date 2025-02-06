using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private enum StepType { Dialogo, InteragirMaquina, AguardarReparo, FecharUI, AguardarCamera }

    [Header("Pendências")]
    public CharacterInfo characterInfo;
    public Dialogue dialogue;
    public SubtitleManager subtitleManager;

    [Header("Passos tutorial")]
    [SerializeField] private StepType[] steps;
    private int step;

    [Header("Textos")]
    [TextArea(2, 6)]
    public string[] dialogueTexts;
    private int textIndex;

    [Space]
    [TextArea(1, 3)]
    public string[] extraTexts;

    [Header("Dublagem (opcional)")]
    public List<AudioClip> voiceClips;

    [Header("Máquinas")]
    public Machines[] machines;
    private int machineIndex;

    private bool error = false;
    private bool waitFix = false;
    private bool waitUI = false;
    private bool waitCamera = false;

    private Accessibility accessibility;

    void Start()
    {
        dialogue.tutorial = this;
        characterInfo.SetTutorial(true, this);
        subtitleManager = FindObjectOfType<SubtitleManager>();
        accessibility = FindObjectOfType<Accessibility>();

        Step(true);

        foreach (var _machine in machines)
        {
            _machine.currentDurability = 0;
        }
    }

    private void Step(bool _restart = false)
    {
        if (_restart)
        {
            textIndex = 0;
            step = 0;
            machineIndex = 0;
        }
        else
        {
            step += 1;
        }

        if (step >= steps.Length)
        {
            characterInfo.SetTutorial();
            dialogue.tutorial = null;
            this.enabled = false;
            return;
        }

        switch (steps[step])
        {
            case StepType.Dialogo:
                PlayDialogueWithDubbing(dialogueTexts[textIndex], textIndex);
                textIndex += 1;
                break;
            case StepType.InteragirMaquina:
                characterInfo.SetAllowedMachine(machines[machineIndex]);
                machineIndex += 1;
                break;
            case StepType.AguardarReparo:
                characterInfo.checkFixTutorial = true;
                waitFix = true;
                break;
            case StepType.FecharUI:
                waitUI = true;
                break;
            case StepType.AguardarCamera:
                waitCamera = true;
                break;
            default:
                break;
        }
    }

    public void DialogueReturn()
    {
        if (error)
        {
            dialogue.Close();
            error = false;
            return;
        }

        if (step + 1 >= steps.Length && !waitFix)
        {
            dialogue.Close();
        }
        else if (steps[step + 1] != StepType.Dialogo && !waitFix)
        {
            dialogue.Close();
        }

        if (!waitFix)
            Step();
    }

    public void MachineReturn()
    {
        if (!waitFix)
            Step();
    }

    public void FixReturn()
    {
        Step();
        waitFix = false;
    }

    public void UIReturn()
    {
        if (!waitFix && waitUI)
        {
            Step();
            waitUI = false;
        }
    }

    public void CameraReturn()
    {
        if (!waitFix && waitCamera)
        {
            Step();
            waitCamera = false;
        }
    }

    public void MachineInteractError(Machines _allowedMachine = null)
    {
        error = true;
        if (_allowedMachine == null)
        {
            PlayDialogueWithDubbing(extraTexts[0], -1);
        }
        else
        {
            switch (_allowedMachine.machineType)
            {
                case Machines.MachineType.BombaHidraulica:
                    PlayDialogueWithDubbing(extraTexts[1], -1);
                    break;
                case Machines.MachineType.Transmissor:
                    PlayDialogueWithDubbing(extraTexts[2], -1);
                    break;
                case Machines.MachineType.Valvula:
                    PlayDialogueWithDubbing(extraTexts[3], -1);
                    break;
                case Machines.MachineType.Outro:
                    PlayDialogueWithDubbing(extraTexts[4], -1);
                    break;
                default:
                    break;
            }
        }
    }

    void PlayDialogueWithDubbing(string text, int index)
    {
        dialogue.SimpleLine(text);

        if (accessibility != null && accessibility.IsDubEnabled() && subtitleManager != null)
        {
            subtitleManager.StopSubtitles();

            if (index >= 0 && index < voiceClips.Count && voiceClips[index] != null)
            {
                subtitleManager.StartSubtitles();
                subtitleManager.GetComponent<AudioSource>().PlayOneShot(voiceClips[index]);
            }
        }
    }
}