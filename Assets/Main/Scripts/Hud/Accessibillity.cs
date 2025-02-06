using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Accessibility : MonoBehaviour
{
    [Header("Outline (Destaque de Objetos)")]
    [SerializeField] Image outlineButton;
    [SerializeField] Sprite outlineDefault, outlineSelected;

    [Header("Tamanho do Texto")]
    [SerializeField] Slider textSizeSlider;
    [SerializeField] TextMeshProUGUI exampleText;

    [Header("Dublagem (Voz nos Diálogos)")]
    [SerializeField] Image dubButton;
    [SerializeField] Sprite dubDefault, dubSelected;

    static bool settingsLoaded = false;
    int outlineEnabled, dubEnabled;

    const float minTextSize = 1.00f;
    const float maxTextSize = 1.50f;

    const float minExampleFontSize = 20f;
    const float maxExampleFontSize = 27f;

    const float minDialogueFontSize = 30f;
    const float maxDialogueFontSize = 45f;

    void Start()
    {
        if (!settingsLoaded)
        {
            LoadAccessibilitySettings();
            settingsLoaded = true;
        }

        if (textSizeSlider != null)
        {
            textSizeSlider.onValueChanged.AddListener(UpdateTextSize);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyTextSizeToDialogues();
        ApplyDubbingSetting();
    }

    void LoadAccessibilitySettings()
    {
        textSizeSlider.minValue = minTextSize;
        textSizeSlider.maxValue = maxTextSize;

        outlineEnabled = PlayerPrefs.GetInt("Outline", 0);
        UpdateOutlineVisual();

        dubEnabled = PlayerPrefs.GetInt("Dublagem", 1);
        UpdateDubVisual();

        float textSize = PlayerPrefs.GetFloat("TextSize", minTextSize);
        if (textSizeSlider != null)
        {
            textSizeSlider.value = textSize;
        }

        UpdateTextSize(textSize);
    }

    public void ToggleOutline()
    {
        outlineEnabled = outlineEnabled == 0 ? 1 : 0;
        PlayerPrefs.SetInt("Outline", outlineEnabled);
        UpdateOutlineVisual();
    }

    public void ToggleDubbing()
    {
        dubEnabled = dubEnabled == 0 ? 1 : 0;
        PlayerPrefs.SetInt("Dublagem", dubEnabled);
        PlayerPrefs.Save();
        UpdateDubVisual();
    }

    void UpdateOutlineVisual()
    {
        if (outlineButton != null)
        {
            outlineButton.sprite = outlineEnabled == 1 ? outlineSelected : outlineDefault;
        }
    }

    void UpdateDubVisual()
    {
        if (dubButton != null)
        {
            dubButton.sprite = dubEnabled == 1 ? dubSelected : dubDefault;
        }
    }

    public void UpdateTextSize(float value)
    {
        PlayerPrefs.SetFloat("TextSize", value);
        PlayerPrefs.Save();
        ApplyTextSizeToDialogues();
    }

    void ApplyTextSizeToDialogues()
    {
        float textSize = PlayerPrefs.GetFloat("TextSize", minTextSize);

        if (exampleText != null)
        {
            exampleText.fontSize = Mathf.Lerp(minExampleFontSize, maxExampleFontSize, Mathf.InverseLerp(minTextSize, maxTextSize, textSize));
        }

        Dialogue[] dialogues = FindObjectsOfType<Dialogue>(true);
        foreach (Dialogue dialogue in dialogues)
        {
            if (dialogue.text != null)
            {
                dialogue.text.fontSize = Mathf.Lerp(minDialogueFontSize, maxDialogueFontSize, Mathf.InverseLerp(minTextSize, maxTextSize, textSize));
            }
        }
    }

    void ApplyDubbingSetting()
    {
        SubtitleManager subtitleManager = FindObjectOfType<SubtitleManager>();
        if (subtitleManager != null)
        {
            if (IsDubEnabled())
            {
                subtitleManager.StartSubtitles();
            }
            else
            {
                subtitleManager.StopSubtitles();
            }
        }
    }

    public bool IsOutlineEnabled() => outlineEnabled == 1;
    public bool IsDubEnabled() => dubEnabled == 1;
}