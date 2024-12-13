using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Temporizador")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float levelTimer = 120;

    private float remainingTime;


    [Header("Pontuação")]
    [SerializeField] private Slider scoreBar;
    [SerializeField] private float scoreGoal = 500;
    [SerializeField, Tooltip("Ganho máxmimo por segundo.")] private float scoreGain = 3;
    [SerializeField] private List<GasFlow> finalOutputs = new List<GasFlow>();


    private float currentScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        ResetManager();
    }

    // Update is called once per frame
    void Update()
    {
        TimerDecrease();
        TimerVisualization();

        if(finalOutputs.Count > 0)
        {
            float _totalFlow = 0f;
            foreach (GasFlow _output in finalOutputs)
            {
                _totalFlow += _output.currentFlow;
            }
            _totalFlow = _totalFlow / finalOutputs.Count;

            ScoreGain(_totalFlow * scoreGain * Time.deltaTime);
        }

        if(currentScore >= scoreGoal)
        {
            Debug.Log("Sucesso!!");
        }

        if (remainingTime <= 0)
        {
            if(currentScore >= scoreGoal)
            {
                Debug.Log("Tempo acabado. Missão concluida!!!!");
            }
            else
            {
                Debug.Log("Tempo acabado. Missão fracassada...");
            }
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
        float _minutes;
        float _seconds;
        if (remainingTime < 0)
            remainingTime = 0;
        _minutes = Mathf.FloorToInt(remainingTime / 60);
        _seconds = remainingTime % 60;

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
}
