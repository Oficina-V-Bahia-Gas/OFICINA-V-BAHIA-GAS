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
    [SerializeField] private float levelTimer = 100;

    private float remainingTime;


    [Header("Pontuação")]
    [SerializeField] private Slider scoreBar;
    [SerializeField] private float scoreGoal = 100;
    [SerializeField, Tooltip("Ganho máxmimo por segundo.")] private float scoreGain = 10;


    private float currentScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        remainingTime = levelTimer;
        scoreBar.maxValue = scoreGoal;
    }

    // Update is called once per frame
    void Update()
    {
        TimerDecrease();
        TimerVisualization();
        ScoreGain(Time.deltaTime * scoreGain); // unfinished
        ScoreVisualization();
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

    void ScoreGain(float _gain = -13.7f)
    {
        if (remainingTime > 0)
        {
            // Default Value
            if(_gain == -13.7)
            {

            }
        }
    }

    void ScoreVisualization()
    {
        scoreBar.value = currentScore;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }
}
