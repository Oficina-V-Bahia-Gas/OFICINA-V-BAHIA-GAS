using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    Machines maquinaAtual;

    public void DefinirMaquinaInteragivel(Machines maquina) => maquinaAtual = maquina;

    public void LimparMaquinaInteragivel() => maquinaAtual = null;
}