using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GasFlow : MonoBehaviour
{
    //
    // Para usar esse código, apenas adicione ele em todas as máquinas que o gás passe.
    //

    [Header("Conexões")]
    [SerializeField] [Tooltip("Máquina é a fonte primária de seu gás")] private bool origin = false;
    [SerializeField] [Tooltip("Máquinas e válvulas que trazem o gás para essa máquina")] private List<GasFlow> inputs = new List<GasFlow>(); // Deve ser adicionado manualmente.
    private List<GasFlow> outputs = new List<GasFlow>(); // Adiciona automaticamente no Start().

    [Header("Atributos")]
    public float currentFlow = 1f;
    [SerializeField] private float fixValue = 1f;



    void Start()
    {
        if(inputs.Any() || origin)
        {
            foreach (GasFlow _input in inputs)
            {
                _input.AddOutput(this);
            }
        }
        else
        {
            origin = true;
        }
    }


    // Chame esse método sempre que a máquina quebrar ao ponto de diminuir o fluxo de gás. O valor deve ser de 0 à 1 (como porcentagem)
    public void ChangeFixValue(float _newFixValue = 0f)
    {
        fixValue = Mathf.Clamp(_newFixValue, 0f, 1f);
        UpdateGasFlow();
    }

    public void UpdateGasFlow()
    {
        if (origin)
        {
            currentFlow = Mathf.Clamp(1f * fixValue, 0f, 1f);
        }
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

        // Opcionalmente adicionar uma linha de código chamando o script responsável pelo desgaste da máquina para que ele não desgaste enquanto não tenha fluxo passando.
        // Também pode ser utilizado para consertar mais rápido quando o fluxo estiver 0.
    }

    private void CallOutputs()
    {
        foreach (var _output in outputs)
        {
            _output.UpdateGasFlow();
        }
    }

    public void AddOutput(GasFlow _output)
    {
        outputs.Add(_output);
    }
}
