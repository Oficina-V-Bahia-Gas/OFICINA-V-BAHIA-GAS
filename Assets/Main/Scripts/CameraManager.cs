using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    // void VerifyPlayerVisibility()
    // {

    //     // if (Vector2.Distance(_camera.Follow.transform.position, _player.transform.position) > _distanceToCamera)
    //     // {
    //     //     GameObject finalTarget = null;
    //     //     for (int i = 0; i < _targetList.Count; i++)
    //     //     {
    //     //         int value = i;
    //     //         int value2 = value + 1;
    //     //         if (finalTarget == null)
    //     //         {
    //     //             if (Vector2.Distance(_targetList[value].transform.position, _player.transform.position) < Vector2.Distance(_targetList[value2].transform.position, _player.transform.position))
    //     //             {
    //     //                 finalTarget = _targetList[value];
    //     //             }
    //     //             else
    //     //             {
    //     //                 finalTarget = _targetList[value2];
    //     //             }
    //     //         }
    //     //         else if (Vector2.Distance(finalTarget.transform.position, _player.transform.position) > Vector2.Distance(_targetList[value].transform.position, _player.transform.position))
    //     //         {
    //     //             finalTarget = _targetList[value];
    //     //         }
    //     //         Debug.Log(i);
    //     //     }
    //     //     SetTarget(finalTarget);
    //     //     Debug.Log(finalTarget.name);
    //     //     finalTarget = null;
    //     // }
    // }
    public void SetTarget(GameObject _target)
    {
        if (_target != null)
            _camera.Follow = _target.transform;
    }
    public CinemachineVirtualCamera GetCamera()
    {
        return _camera;
    }
}
