using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
using DG.Tweening;

public class HUDController : MonoBehaviour
{
    [Header("Configurações de Áudio")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer effectMixer;

    [Header("Acessibilidade")]
    [SerializeField] private Toggle outlineToggle;
    [SerializeField] private Image outlineButton;
    [SerializeField] private TextMeshProUGUI outlineButtonText;
    [SerializeField] private Sprite outlineDefault;
    [SerializeField] private Sprite outlineSelected;

    [Header("Tamanho do Texto")]
    [SerializeField] private Slider textSizeSlider;
    [SerializeField] private TextMeshProUGUI textSizeValue;
    [SerializeField] private TextMeshProUGUI exampleText;

    [Header("Opções do Menu")]
    [SerializeField] private Button continueButton;
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private RectTransform menuPanel;

    private int outlineEnabled;

    private void Start()
    {
        LoadSettings();
    }

    // Carrega as configurações salvas pelo jogador
    private void LoadSettings()
    {
        // Carregar volume salvo
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("effectVolume", 0.5f);

        // Carregar estado do outline
        outlineEnabled = PlayerPrefs.GetInt("Outline", 0);
        UpdateOutlineVisual();

        // Carregar tamanho do texto
        float textSize = PlayerPrefs.GetFloat("TextSize", 1f);
        textSizeSlider.value = textSize;
        UpdateTextSize(textSize);

        // Configurar botão continuar
        if (continueButton != null)
        {
            continueButton.interactable = PlayerPrefs.GetInt("TutorialComplete", 0) == 1;
        }
    }

    // Configura o volume da música
    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("musicVolume", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    // Configura o volume dos efeitos sonoros
    public void SetEffectVolume(float volume)
    {
        effectMixer.SetFloat("effectVolume", volume);
        PlayerPrefs.SetFloat("effectVolume", volume);
    }

    // Alterna o estado da borda (outline)
    public void ToggleOutline()
    {
        outlineEnabled = outlineEnabled == 0 ? 1 : 0;
        PlayerPrefs.SetInt("Outline", outlineEnabled);
        UpdateOutlineVisual();
    }

    // Atualiza a aparência do botão do outline com base na ativação ou desativação
    private void UpdateOutlineVisual()
    {
        if (outlineEnabled == 1)
        {
            outlineButton.sprite = outlineSelected;
            outlineButtonText.text = "DESATIVAR  \n BORDA \n (OBJETOS)";
        }
        else
        {
            outlineButton.sprite = outlineDefault;
            outlineButtonText.text = "ATIVAR \n BORDA \n (OBJETOS)";
        }
    }

    // Atualiza o tamanho da fonte dos textos do jogo
    public void UpdateTextSize(float value)
    {
        PlayerPrefs.SetFloat("TextSize", value);
        textSizeValue.text = value.ToString("F2");
        exampleText.fontSize = 40 * value;
    }

    // Inicia o jogo carregando a cena principal
    public void StartGame()
    {
        SceneManager.LoadScene("MapaDefinitivo");
    }

    // Permite continuar o jogo (caso o tutorial tenha sido concluído)
    public void ContinueGame()
    {
        Debug.Log("Pulando tutorial (implementação futura)");
        // Implementação do pulo do tutorial será adicionada futuramente
    }

    // Encerra o jogo
    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }

    // Faz o menu descer do topo da tela
    public void OpenMenu()
    {
        menuCanvasGroup.alpha = 0;
        menuPanel.anchoredPosition = new Vector2(0, 600f); // Começa fora da tela

        menuPanel.DOAnchorPos(new Vector2(0, 0f), 0.5f, false).SetEase(Ease.OutQuint);
        menuCanvasGroup.DOFade(1, 0.5f);
    }

    // Esconde o menu subindo para fora da tela
    public void CloseMenu()
    {
        menuCanvasGroup.DOFade(0, 0.5f);
        menuPanel.DOAnchorPos(new Vector2(0, 600f), 0.5f, false).SetEase(Ease.InQuint);
    }
}
