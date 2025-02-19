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
    public GameManager gameManager;

    [Header("Passos tutorial")]
    [SerializeField] private StepType[] steps;
    int step;

    [Header("Textos")]
    [TextArea(2, 6)]
    public string[] dialogueTexts;
    int textIndex;

    [Space]
    [TextArea(1, 3)]
    public string[] extraTexts;

    [Header("Dublagem (Diálogos Principais)")]
    public List<AudioClip> voiceClips;

    [Header("Dublagem (Textos Extras)")]
    public List<AudioClip> extraVoiceClips = new List<AudioClip>();

    [Header("Máquinas")]
    public Machines[] machines;
    int machineIndex;

    bool error = false;
    bool waitFix = false;
    bool waitUI = false;
    bool waitCamera = false;

    Accessibility accessibility;

    const float dubDelay = 0.2f;

    void Start()
    {
        dialogue.tutorial = this;
        characterInfo.SetTutorial(true, this);
        accessibility = FindObjectOfType<Accessibility>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        Step(true);
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
            step += 1;

        if (step >= steps.Length)
        {
            PlayerPrefs.SetInt("TutorialComplete", 1);
            PlayerPrefs.Save();

            characterInfo.SetTutorial();
            dialogue.tutorial = null;
            this.enabled = false;
            gameManager.ForceEnd();
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
            dialogue.Close();

        else if (steps[step + 1] != StepType.Dialogo && !waitFix)
            dialogue.Close();

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

    void PlayDialogueWithDubbing(string _text, int _index)
    {
        dialogue.SimpleLine(_text);
        StartCoroutine(PlayDubbingWithDelay(_index));
    }

    void PlayExtraDialogueWithDubbing(int _extraIndex)
    {
        if (_extraIndex < 0 || _extraIndex >= extraTexts.Length) return;

        dialogue.SimpleLine(extraTexts[_extraIndex]);
        StartCoroutine(PlayExtraDubbingWithDelay(_extraIndex));
    }

    IEnumerator PlayDubbingWithDelay(int _index)
    {
        StopAudio();

        yield return new WaitForSeconds(dubDelay);

        if (accessibility != null && accessibility.IsDubEnabled() && _index >= 0 && _index < voiceClips.Count && voiceClips[_index] != null)
            audioSource.PlayOneShot(voiceClips[_index]);
    }

    IEnumerator PlayExtraDubbingWithDelay(int _extraIndex)
    {
        StopAudio();

        yield return new WaitForSeconds(dubDelay);

        if (accessibility != null && accessibility.IsDubEnabled() && _extraIndex < extraVoiceClips.Count && 
            extraVoiceClips[_extraIndex] != null)
        {
            audioSource.PlayOneShot(extraVoiceClips[_extraIndex]);
        }
    }

    void StopAudio()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}