using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float acceleration = 12f;
    public float airControl = 0.45f;

    [Header("Jump And Gravity")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    private CharacterController characterController;
    private Vector3 verticalVelocity;
    private Vector3 currentHorizontalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovePlayer();
        ApplyGravity();
        Jump();
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        inputDirection = Vector3.ClampMagnitude(inputDirection, 1f);

        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        Vector3 targetVelocity = inputDirection * targetSpeed;

        float controlMultiplier = characterController.isGrounded ? 1f : airControl;

        currentHorizontalVelocity = Vector3.Lerp(
            currentHorizontalVelocity,
            targetVelocity,
            acceleration * controlMultiplier * Time.deltaTime
        );

        characterController.Move(currentHorizontalVelocity * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }

        verticalVelocity.y += gravity * Time.deltaTime;

        characterController.Move(verticalVelocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}