using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private CharacterInfo _characterInfo;
    [SerializeField] private Rigidbody _rb;
    private Vector2 _movementDirection;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OnJoyMovement();
    }
    private void FixedUpdate()
    {
        CharMovement();
    }
    void OnJoyMovement()
    {

        _movementDirection = new Vector2(_joystick.Horizontal, _joystick.Vertical) * _characterInfo.GetWalkSpeed();
    }
    void CharMovement()
    {
        if (_movementDirection.magnitude > 0)
        {
            _rb.velocity = new Vector3(_movementDirection.x * Time.deltaTime, _rb.velocity.y, _movementDirection.y * Time.deltaTime);
        }
    }
}
