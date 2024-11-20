using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GasFlow : MonoBehaviour
{
    //
    // Para usar esse c�digo, apenas adicione ele em todas as m�quinas que o g�s passe.
    //

    [Header("Conex�es")]
    [SerializeField] [Tooltip("M�quina � a fonte prim�ria de seu g�s")] private bool origin = false;
    [SerializeField] [Tooltip("M�quinas e v�lvulas que trazem o g�s para essa m�quina")] private List<GasFlow> inputs = new List<GasFlow>(); // Deve ser adicionado manualmente.
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


    // Chame esse m�todo sempre que a m�quina quebrar ao ponto de diminuir o fluxo de g�s. O valor deve ser de 0 � 1 (como porcentagem)
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

        // Opcionalmente adicionar uma linha de c�digo chamando o script respons�vel pelo desgaste da m�quina para que ele n�o desgaste enquanto n�o tenha fluxo passando.
        // Tamb�m pode ser utilizado para consertar mais r�pido quando o fluxo estiver 0.
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
