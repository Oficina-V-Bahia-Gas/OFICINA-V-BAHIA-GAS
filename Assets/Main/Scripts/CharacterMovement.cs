using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private CharacterInfo _characterInfo;
    [SerializeField] private Rigidbody _rb;
    private Vector2 _movementDirection;

    [SerializeField] PlayerInteractionController interactionController;

    void Update()
    {
        if (interactionController == null || !interactionController.IsInteracting)
        {
            OnJoyMovement();
            RotateCharacter();
        }
        else
        {
            _movementDirection = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        CharMovement();
    }

    void OnJoyMovement()
    {
        _movementDirection = new Vector2(_joystick.Horizontal, _joystick.Vertical) * _characterInfo.GetWalkSpeed();
    }

    void RotateCharacter()
    {
        if (_movementDirection.magnitude > 0.1f)
        {
            Vector3 _movementDirection3D = new Vector3(_movementDirection.x, 0f, _movementDirection.y);
            Quaternion _targetRotation = Quaternion.LookRotation(_movementDirection3D, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * 10f);
        }
    }

    void CharMovement()
    {
        if (_movementDirection.magnitude > 0)
        {
            _rb.velocity = new Vector3(_movementDirection.x * Time.deltaTime, _rb.velocity.y, _movementDirection.y * Time.deltaTime);
        }
        else
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
    }
}