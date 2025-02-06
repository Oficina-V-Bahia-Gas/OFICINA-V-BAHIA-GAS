using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private enum StepType { Dialogo, InteragirMaquina, AguardarReparo, FecharUI, AguardarCamera }

    [Header("Pendências")]
    public CharacterInfo characterInfo;
    public Dialogue dialogue;
    public AudioSource audioSource;

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

    [Header("Dublagem (Diálogos Principais)")]
    public List<AudioClip> voiceClips;

    [Header("Dublagem (Textos Extras)")]
    public List<AudioClip> extraVoiceClips = new List<AudioClip>();

    [Header("Máquinas")]
    public Machines[] machines;
    private int machineIndex;

    private bool error = false;
    private bool waitFix = false;
    private bool waitUI = false;
    private bool waitCamera = false;

    private Accessibility accessibility;

    private const float dubDelay = 0.2f;

    void Start()
    {
        dialogue.tutorial = this;
        characterInfo.SetTutorial(true, this);
        accessibility = FindObjectOfType<Accessibility>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

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
        StopAudio();

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
            PlayExtraDialogueWithDubbing(0);
        }
        else
        {
            switch (_allowedMachine.machineType)
            {
                case Machines.MachineType.BombaHidraulica:
                    PlayExtraDialogueWithDubbing(1);
                    break;
                case Machines.MachineType.Transmissor:
                    PlayExtraDialogueWithDubbing(2);
                    break;
                case Machines.MachineType.Valvula:
                    PlayExtraDialogueWithDubbing(3);
                    break;
                case Machines.MachineType.Outro:
                    PlayExtraDialogueWithDubbing(4);
                    break;
                default:
                    break;
            }
        }
    }

    void PlayDialogueWithDubbing(string text, int index)
    {
        dialogue.SimpleLine(text);
        StartCoroutine(PlayDubbingWithDelay(index));
    }

    void PlayExtraDialogueWithDubbing(int extraIndex)
    {
        if (extraIndex < 0 || extraIndex >= extraTexts.Length) return;

        dialogue.SimpleLine(extraTexts[extraIndex]);
        StartCoroutine(PlayExtraDubbingWithDelay(extraIndex));
    }

    IEnumerator PlayDubbingWithDelay(int index)
    {
        StopAudio();

        yield return new WaitForSeconds(dubDelay);

        if (accessibility != null && accessibility.IsDubEnabled() && index >= 0 && index < voiceClips.Count && voiceClips[index] != null)
        {
            audioSource.PlayOneShot(voiceClips[index]);
        }
    }

    IEnumerator PlayExtraDubbingWithDelay(int extraIndex)
    {
        StopAudio();

        yield return new WaitForSeconds(dubDelay);

        if (accessibility != null && accessibility.IsDubEnabled() && extraIndex < extraVoiceClips.Count && extraVoiceClips[extraIndex] != null)
        {
            audioSource.PlayOneShot(extraVoiceClips[extraIndex]);
        }
    }

    void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}