using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valves : Machines
{
    public bool isOpen;

    public void AlternarEstado()
    {
        isOpen = !isOpen;
        Debug.Log(isOpen ? "V�lvula aberta!" : "V�lvula fechada!");
    }
}