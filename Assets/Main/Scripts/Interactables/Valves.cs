using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valves : InteractableClass
{
    [SerializeField]
    protected string valveName = "Default Valve Name";
    [SerializeField]
    [Tooltip("Valor decaído por segundo")]
    protected float decayRate = 0.5f;
    [SerializeField]
    [Tooltip("Durabilidade da válvula")]
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
