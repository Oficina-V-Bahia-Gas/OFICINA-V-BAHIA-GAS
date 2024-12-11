using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraTransitioner : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] [Tooltip("Alvo acima ou � direita")] private GameObject targetUR;
    [SerializeField] [Tooltip("Alvo abaixo ou � esquerda")] private GameObject targetDL;
    [SerializeField] private bool horizontal;
    // Start is called before the first frame update
    void Start()
    {
        Check();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerExit(Collider _other)
    {
        if (!Check()) return;

        if (_other.gameObject.layer == 3)
        {
            if (cameraManager.GetCamera().Follow.transform.position == targetUR.transform.position)
            {
                if (horizontal && math.abs(_other.transform.position.x) > math.abs(transform.position.x) ||
                    !horizontal && math.abs(_other.transform.position.y) < math.abs(transform.position.y)) 
                {
                    cameraManager.SetTarget(targetDL);
                }
                else
                {
                    Debug.Log("Mudan�a de c�mera prevenida por dire��o errada!");
                }
            }
            else
            {
                if (horizontal && math.abs(_other.transform.position.x) < math.abs(transform.position.x) ||
                    !horizontal && math.abs(_other.transform.position.y) > math.abs(transform.position.y))
                {
                    cameraManager.SetTarget(targetUR);
                }
                else
                {
                    Debug.Log("Mudan�a de c�mera prevenida por dire��o errada!");
                }
            }
        }
    }

    private bool Check()
    {
        bool _check = true;
        if (cameraManager == null)
        {
            Debug.LogWarning("camera Manager do '" + this + "' n�o est� configurada!");
            _check = false;
        }

        if (targetUR == null)
        {
            Debug.LogWarning("targetUR do '" + this + "' n�o est� configurada!");
            _check = false;
        }

        if (targetDL == null)
        {
            Debug.LogWarning("targetDL do '" + this + "' n�o est� configurada!");
            _check = false;
        }

        return _check;
    }
}
