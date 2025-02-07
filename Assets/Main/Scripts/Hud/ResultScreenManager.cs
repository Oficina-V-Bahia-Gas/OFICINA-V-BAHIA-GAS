using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ResultScreenManager : MonoBehaviour
{
    [Header("Configuração da Tela")]
    [SerializeField] TMP_Text resultText;

    [Header("Configuração das Estrelas")]
    [SerializeField] GameObject[] starParents;
    [SerializeField] GameObject[] starChildren;

    [Header("Configuração dos Botões")]
    [SerializeField] Button continueButton;
    [SerializeField] Button retryButton;
    [SerializeField] Button menuButton;

    [Header("Configuração do Fade")]
    [SerializeField] Image fadeImage;
    [SerializeField] CanvasGroup fadeCanvasGroup;
    [SerializeField] float fadeDuration = 1.5f;

    float finalScore;
    const float firstStarThreshold = 100;
    const float secondStarThreshold = 200;
    const float thirdStarThreshold = 300;

    string tutorialScene = "Tutorial";
    string fase1Scene = "Fase1";
    string fase2Scene = "Fase2";
    string menuScene = "Menu";

    void Start()
    {
        //AudioManager.instance.Play("Final");

        if (fadeCanvasGroup == null)
        {
            fadeCanvasGroup = fadeImage.GetComponent<CanvasGroup>();
        }

        fadeCanvasGroup.alpha = 0;
        fadeCanvasGroup.gameObject.SetActive(false);

        finalScore = PlayerPrefs.GetFloat("FinalScore", 0);
        string lastScene = PlayerPrefs.GetString("LastScene", tutorialScene);
        bool won = finalScore >= firstStarThreshold;

        if (lastScene == fase2Scene)
        {
            resultText.text = "Parabéns!";
            continueButton.gameObject.SetActive(false);
            retryButton.gameObject.SetActive(false);
            menuButton.gameObject.SetActive(true);
        }
        else
        {
            resultText.text = won ? "Vitória!" : "Derrota!";
            continueButton.gameObject.SetActive(won);
            retryButton.gameObject.SetActive(!won);

            if (won)
            {
                continueButton.onClick.AddListener(ContinueToNextLevel);
            }
            else
            {
                retryButton.onClick.AddListener(RetryLevel);
            }
        }

        menuButton.onClick.AddListener(() => StartCoroutine(FadeToScene(menuScene)));

        SetStars(finalScore);
    }

    void SetStars(float score)
    {
        int starsEarned = 0;

        if (score >= firstStarThreshold) starsEarned = 1;
        if (score >= secondStarThreshold) starsEarned = 2;
        if (score >= thirdStarThreshold) starsEarned = 3;

        for (int i = 0; i < starParents.Length; i++)
        {
            starChildren[i].SetActive(i < starsEarned);
            if (i < starsEarned)
            {
                starChildren[i].transform.localScale = Vector3.zero;
                starChildren[i].transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
            }
        }
    }

    void ContinueToNextLevel()
    {
        string nextScene = GetNextScene();
        StartCoroutine(FadeToScene(nextScene));
    }

    void RetryLevel()
    {
        string retryScene = GetRetryScene();
        StartCoroutine(FadeToScene(retryScene));
    }

    string GetNextScene()
    {
        string lastScene = PlayerPrefs.GetString("LastScene", tutorialScene);

        if (lastScene == tutorialScene)
            return fase1Scene;
        else if (lastScene == fase1Scene)
            return fase2Scene;
        else
            return menuScene;
    }

    string GetRetryScene()
    {
        string lastScene = PlayerPrefs.GetString("LastScene", tutorialScene);

        if (lastScene == tutorialScene)
            return tutorialScene;
        else if (lastScene == fase1Scene)
            return fase1Scene;
        else
            return fase2Scene;
    }

    IEnumerator FadeToScene(string sceneName)
    {
        fadeCanvasGroup.gameObject.SetActive(true);
        yield return fadeCanvasGroup.DOFade(1, fadeDuration).WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        PlayerPrefs.DeleteKey("FinalScore");
        SceneManager.LoadScene(sceneName);
    }
}