using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private Object _defeatScene;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        OnTimerEnd();
    }
    void OnTimerEnd()
    {
        if (_timer.GetRemainingTime() <= 0)
        {
            SceneManager.LoadSceneAsync(_defeatScene.name);
        }
    }
}
