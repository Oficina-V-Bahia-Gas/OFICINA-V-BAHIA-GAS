using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GasFlow : MonoBehaviour
{
    [Header("Conexões")]
    [SerializeField] [Tooltip("Máquina é a fonte primária de seu gás")] bool origin = false;
    [SerializeField] [Tooltip("Máquinas e válvulas que trazem o gás para essa máquina")] List<GasFlow> inputs = new List<GasFlow>(); // Deve ser adicionado manualmente.
    List<GasFlow> outputs = new List<GasFlow>();

    [Header("Atributos")]
    public float currentFlow = 1f;
    [SerializeField] private float fixValue = 1f;

    void Start()
    {
        if (inputs.Any() || origin)
        {
            foreach (GasFlow _input in inputs)
                _input.AddOutput(this);
        }
        else
            origin = true;
    }

    public void ChangeFixValue(float _newFixValue = 0f)
    {
        fixValue = Mathf.Clamp(_newFixValue, 0f, 1f);
        UpdateGasFlow();
    }

    public void UpdateGasFlow()
    {
        if (origin)
            currentFlow = Mathf.Clamp(1f * fixValue, 0f, 1f);
        else
        {
            int _n = 0;
            float _total = 0f;
            foreach (GasFlow _input in inputs)
            {
                _n++;
                _total += _input.currentFlow;
            }

            currentFlow = Mathf.Clamp((_total / _n) * fixValue, 0f, 1f);
        }

        CallOutputs();
    }

    void CallOutputs()
    {
        foreach (var _output in outputs)
            _output.UpdateGasFlow();
    }

    public void AddOutput(GasFlow _output) => outputs.Add(_output);
}