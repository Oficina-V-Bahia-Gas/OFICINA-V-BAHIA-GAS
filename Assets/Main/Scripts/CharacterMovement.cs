using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private CharacterInfo characterInfo;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float rotationSpeed = 10f;
    private Vector2 movementDirection;

    [SerializeField] private PlayerInteractionController interactionController;

    private Animator animator;

    private enum AnimationState { Idle, Walk }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

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

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void UpdateMovementInput()
    {
        movementDirection = new Vector2(joystick.Horizontal, joystick.Vertical);

        if (movementDirection.magnitude > 1f)
        {
            movementDirection.Normalize();
        }
    }

    private void MoveCharacter()
    {
        if (movementDirection.magnitude > 0.01f)
        {
            Vector3 moveVector = new Vector3(
                movementDirection.x * characterInfo.GetWalkSpeed() * Time.fixedDeltaTime * 100,
                rb.velocity.y,
                movementDirection.y * characterInfo.GetWalkSpeed() * Time.fixedDeltaTime * 100);

            //rb.MovePosition(rb.position + moveVector * Time.fixedDeltaTime);
            rb.velocity = moveVector;

            //Debug.Log(rb.position);
        }
        else
        {
            new Vector3(0f, rb.velocity.y, 0f);
        }
    }

    private void RotateCharacter()
    {
        if (movementDirection.magnitude > 0.01f)
        {
            Vector3 targetDirection = new Vector3(movementDirection.x, 0f, movementDirection.y);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void UpdateAnimation()
    {
        if (movementDirection.magnitude > 0.01f)
        {
            SetAnimationState(AnimationState.Walk);
        }
        else
        {
            SetAnimationState(AnimationState.Idle);
        }
    }

    private void SetAnimationState(AnimationState state)
    {
        animator.SetInteger("State", (int)state);
    }
}
