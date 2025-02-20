using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using DG.Tweening;

public class HUDController : MonoBehaviour
{
    [Header("Opções do Menu")]
    [SerializeField] Button continueButton;
    [SerializeField] CanvasGroup menuCanvasGroup;
    [SerializeField] RectTransform menuPanel;

    bool isGamePaused = false;

    private void Start() => LoadSettings();

    void LoadSettings()
    {
        if (continueButton != null)
            continueButton.interactable = PlayerPrefs.GetInt("TutorialComplete", 0) == 1;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void ContinueGame()
    {
        if (continueButton != null)
            continueButton.interactable = true;

        SceneManager.LoadScene("Fase1");
    }

    public void QuitGame() => Application.Quit();

    public void BackToMenu() => SceneManager.LoadScene("Menu");

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
                //Inserir aqui: Parar o timer
                menuCanvasGroup.interactable = true;
                menuCanvasGroup.blocksRaycasts = true;
            });

        menuCanvasGroup.DOFade(1, 0.5f);
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
    }
}