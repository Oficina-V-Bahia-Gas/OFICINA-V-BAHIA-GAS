using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private Object _Scene;
    [SerializeField] private Button _button;

    // Start is called before the first frame update
    void Start()
    {
        _button.onClick.AddListener(OnButtonClick);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnButtonClick()
    {
        SceneManager.LoadSceneAsync(_Scene.name);
    }
}
