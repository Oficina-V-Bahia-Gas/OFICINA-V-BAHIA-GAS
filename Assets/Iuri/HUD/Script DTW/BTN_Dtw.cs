using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BTN_Dtw : MonoBehaviour
{
    [Header("Configurações da Pulsação")]
    [SerializeField] private float pulseScale = 1.2f; // Escala máxima do pulso
    [SerializeField] private float pulseDuration = 0.8f; // Duração de cada pulsação

    private RectTransform _rectTransform; // Referência ao RectTransform do botão

    void Start()
    {
        // Garante que o componente RectTransform está atribuído
        _rectTransform = GetComponent<RectTransform>();

        if (_rectTransform != null)
        {
            StartPulseAnimation();
        }
        else
        {
            Debug.LogError("RectTransform não encontrado no objeto!");
        }
    }

    private void StartPulseAnimation()
    {
        // Cria a animação de pulsação em loop
        _rectTransform.DOScale(pulseScale, pulseDuration)
            .SetEase(Ease.InOutSine) // Suaviza a transição
            .SetLoops(-1, LoopType.Yoyo); // Loop infinito (Yoyo = vai e volta)
    }

    void OnDestroy()
    {
        // Interrompe a animação ao destruir o objeto
        DOTween.Kill(_rectTransform);
    }
}