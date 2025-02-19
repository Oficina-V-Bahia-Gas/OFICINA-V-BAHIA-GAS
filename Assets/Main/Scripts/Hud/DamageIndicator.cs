using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    [Header("Dependencias")]
    [SerializeField] GameObject working;
    [SerializeField] GameObject warning;
    [SerializeField] GameObject error;

    [Header("Posicao")]
    [SerializeField] public Transform lookObject;
    [SerializeField] public Vector3 offset;

    [Header("Animacao")]
    [SerializeField] float shakeDuration = 0.3f;
    [SerializeField] float shakeStrenght = 20f;
    [SerializeField] float shakeMovement = 0.2f;
    [SerializeField] float shakeInterval = 1f;

    private Camera cam;
    private RectTransform canvas;
    private RectTransform thisRect;

    Quaternion originalRotation;
    Vector3 originalPosition;

    bool started = false;
    bool hold = false;
    bool running = false;
    bool reset = false;

    Coroutine coroutine;

    void Start()
    {
        if (working == null || warning == null || error == null)
        {
            Debug.LogError("GameObjects não definidos corretamente. (" + this + ")");
            Destroy(gameObject);
        }

        cam = Camera.main;
        thisRect = gameObject.GetComponent<RectTransform>();
        canvas = gameObject.GetComponentInParent<RectTransform>();

        if(canvas == null || cam == null || thisRect == null)
        {
            Debug.LogError("Não foi possível configurar. (" + this + ")");
            Destroy(gameObject);
        }


        if(lookObject != null)
            started = true;

        ChangeVisible();
    }

    public DamageIndicator Setup(Transform _object, Vector3 _offset = new Vector3(), bool _replaceOffset = false)
    {
        if (!started)
        {
            started = true;
            lookObject = _object;
            if (_replaceOffset)
            {
                offset = _offset;
            }
            else
            {
                offset += _offset;
            }
        }

        ChangeVisible();
        return this;
    }

    void Update()
    {
        if(started && !hold)
            Move();
    }

    public void Move()
    {
        Vector2 _pos = cam.WorldToScreenPoint(lookObject.position + offset);

        _pos.x *= canvas.rect.width / (float)cam.pixelWidth;
        _pos.y *= canvas.rect.height / (float)cam.pixelHeight;

        if(thisRect.anchoredPosition != _pos - canvas.sizeDelta /2f)
            thisRect.anchoredPosition = _pos - canvas.sizeDelta / 2f;
    }

    public void ChangeVisible(int _i = 0)
    {
        if (started && !hold)
        {
            switch (_i)
            {
                case 0:
                    working.SetActive(true);
                    warning.SetActive(false);
                    error.SetActive(false);
                    break;
                case 1:
                    working.SetActive(false);
                    warning.SetActive(true);
                    error.SetActive(false);
                    break;
                case 2:
                    working.SetActive(false);
                    warning.SetActive(false);
                    error.SetActive(true);
                    break;
                default:
                    working.SetActive(false);
                    warning.SetActive(false);
                    error.SetActive(false);
                    break;
            }
        }
    }

    public void Animation(int _i = 0)
    {
        if (started && !hold)
        {
            switch (_i)
            {
                case 0:
                    ResetPosition();
                    break;
                case 1:
                    ResetPosition();
                    StartCoroutine(Shake());
                    break;
                case 2:
                    ResetPosition();
                    coroutine = StartCoroutine(ConstantShake());
                    break;
                case 3:
                    ResetPosition();
                    coroutine = StartCoroutine(ConstantShake(true));
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator Shake(bool unstoppable = false)
    {
        running = true;

        Quaternion _originalRotation = working.transform.rotation;
        Vector3 _originalPosition = working.transform.position;

        float _elapsed = 0.0f;

        while ((_elapsed < shakeDuration || unstoppable) && !reset)
        {

            _elapsed += Time.deltaTime;
            float _factor = 1f;
            if (!unstoppable)
            {
                _factor = Mathf.Clamp((shakeDuration - _elapsed) / shakeDuration, 0, 1);
            }

            float z = (Random.value * shakeStrenght - (shakeStrenght / 2)) * _factor;
            float w = (Random.value * shakeStrenght - (shakeStrenght / 2)) * _factor;
            float h = (Random.value * shakeStrenght - (shakeStrenght / 2)) * _factor;

            Vector3 _rotation = new Vector3(originalRotation.x, originalRotation.y, originalRotation.z + z);
            Vector3 _position = new Vector3(originalPosition.x + (z * shakeMovement), originalPosition.y + (w * shakeMovement), originalPosition.z + (h * shakeMovement));

            working.transform.eulerAngles = _rotation;
            warning.transform.eulerAngles = _rotation;
            error.transform.eulerAngles = _rotation;
                
            yield return null;
        }

        working.transform.SetPositionAndRotation(_originalPosition, _originalRotation);
        warning.transform.SetPositionAndRotation(_originalPosition, _originalRotation);
        error.transform.SetPositionAndRotation(_originalPosition, _originalRotation);

        if (reset)
            reset = false;

        running = false;
    }

    void ResetPosition()
    {
        if (running && !reset)
        {
            reset = true;
        }

        if(coroutine != null)
            StopCoroutine(coroutine);
    }

    private IEnumerator ConstantShake(bool fast = false)
    {
        while (true)
        {
            StartCoroutine(Shake());
            if (fast)
            {
                yield return new WaitForSeconds(shakeInterval/3);
            }
            else
            {
                yield return new WaitForSeconds(shakeInterval);
            }
        }
    }

    public IEnumerator Wait(float _time = 5f)
    {
        hold = true;
        yield return new WaitForSeconds(_time);
        hold = false;
    }
}
