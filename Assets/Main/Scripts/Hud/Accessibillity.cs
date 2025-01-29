using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Accessibility : MonoBehaviour
{
    [SerializeField] private Toggle accessibilityToggle;
    [SerializeField] private Slider textSlider;

    [Header("Configurações de Acessibilidade")]
    [SerializeField] private TextMeshProUGUI textSizeValue;
    [SerializeField] private TextMeshProUGUI dialogueText; // Referência ao texto do diálogo

    public static bool isOutlineEnabled = false;

    private void Start()
    {
        // Carregar configurações salvas
        isOutlineEnabled = PlayerPrefs.GetInt("Outline", 0) == 1;
        accessibilityToggle.isOn = isOutlineEnabled;

        float savedTextSize = PlayerPrefs.GetFloat("textSize", 1f);
        textSlider.value = savedTextSize;
        UpdateDialogueTextSize(savedTextSize);

        // Adiciona os eventos dos botões
        accessibilityToggle.onValueChanged.AddListener(SetOutlineState);
        textSlider.onValueChanged.AddListener(UpdateDialogueTextSize);
    }

    public void SetOutlineState(bool state)
    {
        isOutlineEnabled = state;
        PlayerPrefs.SetInt("Outline", state ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void UpdateDialogueTextSize(float value)
    {
        // Salva o novo tamanho do texto
        PlayerPrefs.SetFloat("textSize", value);
        PlayerPrefs.Save();

        // Atualiza a interface do menu
        textSizeValue.text = value.ToString("F2");

        // Aplica o novo tamanho SOMENTE ao diálogo
        if (dialogueText != null)
        {
            dialogueText.fontSize = 40 * value;
        }
    }
}
