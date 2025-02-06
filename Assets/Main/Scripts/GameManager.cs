using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("Temporizador")]
    [SerializeField] TMP_Text timerText;
    [SerializeField] float levelTimer = 120;
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration = 1.5f;

    private float remainingTime;

    [Header("Pontuação")]
    [SerializeField] Slider scoreBar;
    [SerializeField] float scoreGoal = 300;
    [SerializeField] float firstStarThreshold = 100;
    [SerializeField] float secondStarThreshold = 200;
    [SerializeField] float thirdStarThreshold = 300;
    [SerializeField, Tooltip("Ganho máximo por segundo.")] float scoreGain = 3;
    [SerializeField] List<GasFlow> finalOutputs = new List<GasFlow>();

    private float currentScore = 0;

    void Start()
    {
        ResetManager();
    }

    void Update()
    {
        TimerDecrease();
        TimerVisualization();

        if (finalOutputs.Count > 0)
        {
            float _totalFlow = 0f;
            foreach (GasFlow _output in finalOutputs)
            {
                _totalFlow += _output.currentFlow;
            }
            _totalFlow = _totalFlow / finalOutputs.Count;

            ScoreGain(_totalFlow * scoreGain * Time.deltaTime);
        }

        if (remainingTime <= 0)
        {
            StartCoroutine(FadeToResultScene());
        }
    }

    void TimerDecrease()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
    }

    void TimerVisualization()
    {
        float _minutes = Mathf.FloorToInt(remainingTime / 60);
        float _seconds = remainingTime % 60;

        timerText.text = $"{_minutes:00} : {_seconds:00}";
    }

    public void ScoreGain(float _gain)
    {
        if (remainingTime > 0)
        {
            currentScore += _gain;
        }

        ScoreVisualization();
    }

    void ScoreVisualization()
    {
        scoreBar.value = currentScore;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    public void ResetManager()
    {
        remainingTime = levelTimer;
        currentScore = 0f;

        scoreBar.maxValue = scoreGoal;
        scoreBar.value = currentScore;

        ScoreVisualization();
    }

    IEnumerator FadeToResultScene()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, fadeDuration).OnComplete(() =>
        {
            PlayerPrefs.SetFloat("FinalScore", currentScore);
            SceneManager.LoadScene(currentScore >= firstStarThreshold ? "VictoryScene" : "DefeatScene");
        });

        yield return null;
    }

    public void ForceSetScore(float score)
    {
        currentScore = score;
        ScoreVisualization();
    }

    public void ForceSetTime(float time)
    {
        remainingTime = time;
    }

}