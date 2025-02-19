using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Temporizador")]
    [SerializeField] TMP_Text timerText;
    [SerializeField] float levelTimer = 180;
    [SerializeField] Image fadeImage;
    [SerializeField] CanvasGroup fadeCanvasGroup;
    [SerializeField] float fadeDuration = 1.5f;

    [SerializeField] string music;
    [SerializeField] bool tutorial = false;

    float remainingTime;

    [Header("Pontuação")]
    [SerializeField] Slider scoreBar;
    [SerializeField] float scoreGoal = 300;
    public float firstStarThreshold = 100;
    public float secondStarThreshold = 200;
    public float thirdStarThreshold = 300;
    [SerializeField, Tooltip("Ganho máximo por segundo.")] float scoreGain = 3;
    [SerializeField] List<GasFlow> finalOutputs = new List<GasFlow>();

    float currentScore = 0;
    bool gameEnded = false;

    void Start()
    {
        AudioManager.instance.Stop("Squeak");
        AudioManager.instance.Stop("Brush");
        AudioManager.instance.Stop("Electric Hum");
        AudioManager.instance.Stop("Menu");
        AudioManager.instance.Stop("Tutorial");
        AudioManager.instance.Stop("Fase 1");
        AudioManager.instance.Stop("Fase 2");
        AudioManager.instance.Play(music);

        if (fadeCanvasGroup == null && fadeImage != null)
            fadeCanvasGroup = fadeImage.GetComponent<CanvasGroup>();

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0;
            fadeCanvasGroup.gameObject.SetActive(false);
        }

        ResetManager();
    }

    void Update()
    {
        if (!gameEnded)
        {
            TimerDecrease();
            TimerVisualization();
            UpdateScore();

            if (remainingTime <= 0 && !gameEnded && !tutorial)
            {
                gameEnded = true;
                StartCoroutine(FadeToResultScene());
            } 
            else if (tutorial)
                remainingTime = 0;
        }
    }

    void TimerDecrease()
    {
        if (remainingTime > 0 && !tutorial)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0) remainingTime = 0;
        }
    }

    void TimerVisualization()
    {
        float _minutes = Mathf.FloorToInt(remainingTime / 60);
        float _seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = $"{_minutes:00} : {_seconds:00}";
    }

    void UpdateScore()
    {
        if (finalOutputs.Count > 0)
        {
            float _totalFlow = 0f;
            foreach (GasFlow _output in finalOutputs)
            {
                _totalFlow += _output.currentFlow;
            }
            _totalFlow /= finalOutputs.Count;

            ScoreGain(_totalFlow * scoreGain * Time.deltaTime);
        }
    }

    public void ScoreGain(float _gain)
    {
        if (remainingTime > 0 || tutorial)
        {
            currentScore += _gain;
        }

        ScoreVisualization();
    }

    void ScoreVisualization()
    {
        scoreBar.value = currentScore;
    }

    public void ResetManager()
    {
        remainingTime = levelTimer;
        currentScore = 0f;
        gameEnded = false;

        if (scoreBar != null)
        {
            scoreBar.maxValue = scoreGoal;
            scoreBar.value = currentScore;
        }

        ScoreVisualization();
    }

    IEnumerator FadeToResultScene()
    {
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            yield return fadeCanvasGroup.DOFade(1, fadeDuration).WaitForCompletion();
        }

        yield return new WaitForSeconds(0.5f);

        PlayerPrefs.SetFloat("FinalScore", currentScore);
        SceneManager.LoadScene("Vitoria&Derrota");
    }

    public void ForceSetScore(float _score)
    {
        currentScore = _score;
        ScoreVisualization();
    }

    public void ForceSetTime(float _time)
    {
        remainingTime = Mathf.Max(0, _time);
    }

    public void ForceEnd()
    {
        StartCoroutine(FadeToResultScene());
    }
}