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

    int outlineEnabled, dubEnabled;

    const float minTextSize = 1.00f;
    const float maxTextSize = 1.50f;

    const float minExampleFontSize = 20f;
    const float maxExampleFontSize = 27f;

    const float minDialogueFontSize = 30f;
    const float maxDialogueFontSize = 43f;

    void Start()
    {
        LoadAccessibilitySettings();

        if (textSizeSlider != null)
            textSizeSlider.onValueChanged.AddListener(UpdateTextSize);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        ApplyTextSizeToDialogues();
        ApplyDubbingSetting();
    }

    void LoadAccessibilitySettings()
    {
        if (textSizeSlider != null)
        {
            textSizeSlider.minValue = minTextSize;
            textSizeSlider.maxValue = maxTextSize;
        }

        outlineEnabled = PlayerPrefs.GetInt("Outline", 0);
        UpdateOutlineVisual();

        dubEnabled = PlayerPrefs.GetInt("Dublagem", 1);
        UpdateDubVisual();

        float _textSize = PlayerPrefs.GetFloat("TextSize", minTextSize);
        if (textSizeSlider != null)
            textSizeSlider.value = _textSize;

        UpdateTextSize(_textSize);
    }

    public void ToggleOutline()
    {
        outlineEnabled = outlineEnabled == 0 ? 1 : 0;
        PlayerPrefs.SetInt("Outline", outlineEnabled);
        PlayerPrefs.Save();
        UpdateOutlineVisual();
    }

    public void ToggleDubbing()
    {
        dubEnabled = dubEnabled == 0 ? 1 : 0;
        PlayerPrefs.SetInt("Dublagem", dubEnabled);
        PlayerPrefs.Save();
        UpdateDubVisual();
        ApplyDubbingSetting();
    }

    void UpdateOutlineVisual()
    {
        if (outlineButton != null)
            outlineButton.sprite = outlineEnabled == 1 ? outlineSelected : outlineDefault;

        Outline[] _outlines = FindObjectsOfType<Outline>();
        foreach (Outline _outline in _outlines)
            _outline.enabled = (outlineEnabled == 1);
    }

    void UpdateDubVisual()
    {
        if (dubButton != null)
            dubButton.sprite = dubEnabled == 1 ? dubSelected : dubDefault;
    }

    public void UpdateTextSize(float _value)
    {
        PlayerPrefs.SetFloat("TextSize", _value);
        PlayerPrefs.Save();
        ApplyTextSizeToDialogues();
    }

    void ApplyTextSizeToDialogues()
    {
        float _textSize = PlayerPrefs.GetFloat("TextSize", minTextSize);

        if (exampleText != null)
        {
            exampleText.fontSize = Mathf.Lerp(minExampleFontSize, maxExampleFontSize,
                Mathf.InverseLerp(minTextSize, maxTextSize, _textSize));
        }

        Dialogue[] _dialogues = FindObjectsOfType<Dialogue>(true);
        foreach (Dialogue _dialogue in _dialogues)
        {
            if (_dialogue.text != null)
            {
                _dialogue.text.fontSize = Mathf.Lerp(minDialogueFontSize, maxDialogueFontSize,
                    Mathf.InverseLerp(minTextSize, maxTextSize, _textSize));
            }
        }
    }

    void ApplyDubbingSetting()
    {
        SubtitleManager _subtitleManager = FindObjectOfType<SubtitleManager>();
        if (_subtitleManager != null)
        {
            if (IsDubEnabled())
            {
                _subtitleManager.StartSubtitles();
            }
            else
            {
                _subtitleManager.StopSubtitles();
            }
        }
    }

    public bool IsOutlineEnabled() => outlineEnabled == 1;
    public bool IsDubEnabled() => dubEnabled == 1;
}