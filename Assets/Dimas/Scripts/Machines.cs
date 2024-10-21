using UnityEngine;

public class Machines : MonoBehaviour
{
    public enum MachineAction
    {
        Repair,
        Upgrade,
        Refuel,
        Clean
    }

    public interface IMachine
    {
        void PerformAction(MachineAction action);
    }
}