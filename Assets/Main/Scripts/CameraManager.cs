using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] Tutorial tutorial;

    public void SetTarget(GameObject _target)
    {
        if (_target != null)
        {
            cam.Follow = _target.transform;

            if (tutorial != null)
                tutorial.CameraReturn();

            AudioManager.instance.Play("Whoosh");
        }
    }
    public CinemachineVirtualCamera GetCamera()
    {
        return cam;
    }
}