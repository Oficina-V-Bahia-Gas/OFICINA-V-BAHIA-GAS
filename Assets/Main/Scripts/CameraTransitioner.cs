using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitioner : MonoBehaviour
{
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private GameObject _target1;
    [SerializeField] private GameObject _target2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if (_cameraManager.GetCamera().Follow.transform.position == _target1.transform.position)
            {
                _cameraManager.SetTarget(_target2);
            }
            else
            {
                _cameraManager.SetTarget(_target1);
            }
        }
    }
}
