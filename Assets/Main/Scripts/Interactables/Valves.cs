using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valves : InteractableClass
{
    [SerializeField]
    protected string valveName = "Default Valve Name";
    [SerializeField]
    [Tooltip("Valor deca�do por segundo")]
    protected float decayRate = 0.5f;
    [SerializeField]
    [Tooltip("Durabilidade da v�lvula")]
    protected float durability = 100f;

    public override void Interact()
    {
        base.Interact();

        Debug.Log(valveName);
    }

    protected void Update()
    {
        
    }
}
