using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [Header("Dependencias")]
    [SerializeField] GameObject damageIndicatorPrefab;
    [SerializeField] Machines[] machines;
    [SerializeField] bool tutorial = false;
    
    void Start()
    {
        foreach (var _machine in machines)
        {
            DamageIndicator _damageIndicator = Instantiate(damageIndicatorPrefab).GetComponent<DamageIndicator>();
            _damageIndicator.transform.SetParent(transform);
            RectTransform _rect = _damageIndicator.gameObject.GetComponent<RectTransform>();
            _rect.localScale = Vector3.one;
            _rect.localPosition = new Vector3(_rect.localPosition.x, _rect.localPosition.y, 500);
            _machine.damageIndicator = _damageIndicator;
            _damageIndicator.Setup(_machine.transform, _machine.indicatorOffset, _machine.offsetReplace);

            if (tutorial)
            {
                //_damageIndicator.ChangeVisible(2);
                //_damageIndicator.Animation(3);
                //StartCoroutine(_damageIndicator.Wait(1f));
            }
        }
    }
}
