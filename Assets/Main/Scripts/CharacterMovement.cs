using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    [SerializeField] CharacterInfo characterInfo;
    [SerializeField] Rigidbody rb;

    [SerializeField] float rotationSpeed = 10f;
    Vector2 movementDirection;

    [SerializeField] PlayerInteractionController interactionController;

    Animator animator;

    enum AnimationState { Idle, Walk }

    private void Start() => animator = GetComponent<Animator>();

    private void Update()
    {
        if (interactionController == null || !interactionController.IsInteracting)
        {
            UpdateMovementInput();
            RotateCharacter();
            UpdateAnimation();
        }
        else
        {
            movementDirection = Vector2.zero;
            UpdateAnimation();
        }
    }

    private void FixedUpdate() => MoveCharacter();

    void UpdateMovementInput()
    {
        movementDirection = new Vector2(joystick.Horizontal, joystick.Vertical);

        if (movementDirection.magnitude > 1f) movementDirection.Normalize();
    }

    void MoveCharacter()
    {
        if (movementDirection.magnitude > 0.01f)
        {
            Vector3 _moveVector = new Vector3(
                movementDirection.x * characterInfo.GetWalkSpeed() * Time.fixedDeltaTime * 100,
                rb.velocity.y,
                movementDirection.y * characterInfo.GetWalkSpeed() * Time.fixedDeltaTime * 100);

            rb.velocity = _moveVector;

            if(!AudioManager.instance.IsPlaying("Footsteps"))
                AudioManager.instance.Play("Footsteps");
        }
        else
        {
            AudioManager.instance.Stop("Footsteps");
            new Vector3(0f, rb.velocity.y, 0f);
        }
    }

    void RotateCharacter()
    {
        if (movementDirection.magnitude > 0.01f)
        {
            Vector3 _targetDirection = new Vector3(movementDirection.x, 0f, movementDirection.y);
            Quaternion _targetRotation = Quaternion.LookRotation(_targetDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void UpdateAnimation()
    {
        if (movementDirection.magnitude > 0.01f)
            SetAnimationState(AnimationState.Walk);
        else
            SetAnimationState(AnimationState.Idle);
    }

    void SetAnimationState(AnimationState state) => animator.SetInteger("State", (int)state);
}