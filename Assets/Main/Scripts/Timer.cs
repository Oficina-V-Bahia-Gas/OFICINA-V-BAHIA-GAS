using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _levelTimer = 100;
    [SerializeField] private TMP_Text _timerText;
    private float _remainingTime;
    // Start is called before the first frame update
    void Start()
    {
        _remainingTime = _levelTimer;
    }

    // Update is called once per frame
    void Update()
    {
        TimerDecrease();
        TimerVisualization();
    }
    void TimerDecrease()
    {
        if (_remainingTime > 0)
        {
            _remainingTime -= Time.deltaTime;
        }
    }
    void TimerVisualization()
    {
        float minutes;
        float seconds;
        if (_remainingTime < 0)
            _remainingTime = 0;
        minutes = Mathf.FloorToInt(_remainingTime / 60);
        seconds = _remainingTime % 60;

        _timerText.text = $"{minutes:00} : {seconds:00}";
    }
    public float GetRemainingTime()
    {
        return _remainingTime;
    }
}
