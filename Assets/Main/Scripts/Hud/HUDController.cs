using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using DG.Tweening;

public class HUDController : MonoBehaviour
{
    [Header("Configurações de Áudio")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] AudioMixer musicMixer;
    [SerializeField] AudioMixer effectMixer;

    [Header("Opções do Menu")]
    [SerializeField] Button continueButton;
    [SerializeField] CanvasGroup menuCanvasGroup;
    [SerializeField] RectTransform menuPanel;

    bool isGamePaused = false;

    private void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("effectVolume", 0.5f);

        if (continueButton != null)
            continueButton.interactable = PlayerPrefs.GetInt("TutorialComplete", 0) == 1;
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("musicVolume", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetEffectVolume(float volume)
    {
        effectMixer.SetFloat("effectVolume", volume);
        PlayerPrefs.SetFloat("effectVolume", volume);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MapaDefinitivo");
    }

    public void ContinueGame()
    {
        Debug.Log("Pular tutorial (implementação futura)");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if (isGamePaused) return;

        isGamePaused = true;
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;

        menuCanvasGroup.alpha = 0;
        menuPanel.anchoredPosition = new Vector2(0, 600f);

        menuPanel.DOAnchorPos(new Vector2(0, 0f), 0.5f, false)
            .SetEase(Ease.OutQuint)
            .OnComplete(() =>
            {
                Time.timeScale = 0;
                menuCanvasGroup.interactable = true;
                menuCanvasGroup.blocksRaycasts = true;
            });

        menuCanvasGroup.DOFade(1, 0.5f);
        Debug.Log("Jogo pausado.");
    }

    public void ResumeGame()
    {
        if (!isGamePaused) return;

        isGamePaused = false;
        Time.timeScale = 1;

        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;

        menuCanvasGroup.DOFade(0, 0.5f);
        menuPanel.DOAnchorPos(new Vector2(0, 600f), 0.5f, false)
            .SetEase(Ease.InQuint)
            .OnComplete(() =>
            {
                menuCanvasGroup.interactable = true;
                menuCanvasGroup.blocksRaycasts = true;
            });

        Debug.Log("Jogo retomado.");
    }
}