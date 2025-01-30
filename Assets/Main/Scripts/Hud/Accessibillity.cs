using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Accessibility : MonoBehaviour
{
    [Header("Outline (Destaque de Objetos)")]
    [SerializeField] Image outlineButton;
    [SerializeField] Sprite outlineDefault;
    [SerializeField] Sprite outlineSelected;

    [Header("Tamanho do Texto")]
    [SerializeField] Slider textSizeSlider;
    [SerializeField] TextMeshProUGUI exampleText;
    [SerializeField] TextMeshProUGUI dialogueText;

    int outlineEnabled;
    float minTextSize = 1.00f;
    float maxTextSize = 1.50f;
    float minFontSize = 20f;
    float maxFontSize = 27f;

    void Start()
    {
        LoadAccessibilitySettings();
        textSizeSlider.onValueChanged.AddListener(UpdateTextSize);
    }

    void LoadAccessibilitySettings()
    {
        textSizeSlider.minValue = minTextSize;
        textSizeSlider.maxValue = maxTextSize;

        outlineEnabled = PlayerPrefs.GetInt("Outline", 0);
        UpdateOutlineVisual();

        float textSize = PlayerPrefs.GetFloat("TextSize", minTextSize);
        textSizeSlider.value = textSize;
        UpdateTextSize(textSize);
    }

    public void ToggleOutline()
    {
        outlineEnabled = outlineEnabled == 0 ? 1 : 0;
        PlayerPrefs.SetInt("Outline", outlineEnabled);
        PlayerPrefs.Save();
        UpdateOutlineVisual();
    }

    void UpdateOutlineVisual()
    {
        outlineButton.sprite = outlineEnabled == 1 ? outlineSelected : outlineDefault;
    }

    public void UpdateTextSize(float value)
    {
        PlayerPrefs.SetFloat("TextSize", value);
        PlayerPrefs.Save();

        float newFontSize = Mathf.Lerp(minFontSize, maxFontSize, Mathf.InverseLerp(minTextSize, maxTextSize, value));

        if (exampleText != null)
        {
            exampleText.fontSize = newFontSize;
        }

        if (SceneManager.GetActiveScene().buildIndex == 2 && dialogueText != null)
        {
            dialogueText.fontSize = newFontSize;
        }
    }
}