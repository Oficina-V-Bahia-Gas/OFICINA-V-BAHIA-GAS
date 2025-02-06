using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private enum StepType { Dialogo, InteragirMaquina , AguardarReparo, FecharUI, AguardarCamera}
    [Header("Pendências")]
    public CharacterInfo characterInfo;
    public Dialogue dialogue;
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
    [Header("Máquinas")]
    public Machines[] machines;
    private int machineIndex;

    private bool error = false;
    private bool waitFix = false;
    private bool waitUI = false;
    private bool waitCamera = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogue.tutorial = this;
        characterInfo.SetTutorial(true, this);
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
        Debug.LogError(step);
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
                dialogue.SimpleLine(dialogueTexts[textIndex]);
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

                break;
            default:
                break;
        }
    }

    public void DialogueReturn()
    {
        if (error)
        {
            Debug.LogError("t_error");
            dialogue.Close();
            error = false;
            return;
        }
        if(step + 1 >= steps.Length && !waitFix)
        {
            Debug.LogError("t_close");
            dialogue.Close();
        }
        else if (steps[step +1] != StepType.Dialogo && !waitFix)
        {
            Debug.LogError("t2_CLOSE");
            dialogue.Close();
        }
        if(!waitFix)
            Step();
    }

    public void MachineReturn()
    {
        if(!waitFix)
            Step();
    }

    public void FixReturn()
    {
        Step();
        waitFix = false;
    }

    public void UIReturn()
    {
        if(!waitFix && waitUI)
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
        if(_allowedMachine == null)
        {
            dialogue.SimpleLine(extraTexts[0]);
        }
        else
        {
            switch (_allowedMachine.machineType)
            {
                case Machines.MachineType.BombaHidraulica:
                    dialogue.SimpleLine(extraTexts[1]);
                    break;
                case Machines.MachineType.Transmissor:
                    dialogue.SimpleLine(extraTexts[2]);
                    break;
                case Machines.MachineType.Valvula:
                    dialogue.SimpleLine(extraTexts[3]);
                    break;
                case Machines.MachineType.Outro:
                    dialogue.SimpleLine(extraTexts[4]);
                    break;
                default:
                    break;
            }
        }
    }
}
