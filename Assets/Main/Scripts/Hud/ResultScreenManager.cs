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

    private float finalScore;
    private const float firstStarThreshold = 100;
    private const float secondStarThreshold = 200;
    private const float thirdStarThreshold = 300;

    void Start()
    {
        finalScore = PlayerPrefs.GetFloat("FinalScore", 0);

        bool won = finalScore >= firstStarThreshold;
        resultText.text = won ? "Vitória!" : "Derrota!";

        SetStars(finalScore);

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

        menuButton.onClick.AddListener(ReturnToMenu);
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
        PlayerPrefs.DeleteKey("FinalScore");
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    void RetryLevel()
    {
        PlayerPrefs.DeleteKey("FinalScore");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void ReturnToMenu()
    {
        PlayerPrefs.DeleteKey("FinalScore");
        SceneManager.LoadScene("Menu");
    }
}