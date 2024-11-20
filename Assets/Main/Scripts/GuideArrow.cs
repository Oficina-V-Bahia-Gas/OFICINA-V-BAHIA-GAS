using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideArrow : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z));
    }
    public void SetTarget(GameObject _actualTarget)
    {
        _target = _actualTarget;
    }
}
