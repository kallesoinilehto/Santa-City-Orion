using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.1f;

    [Header("Flight Settings")]
    public bool canFly = false;       // Toggle flying mode
    public float flySpeed = 5f;       // Up/Down speed when flying

    [Header("Camera Reference")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private CapsuleCollider col;
    private Vector3 velocity;
    private bool jumpRequested = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Use physics-driven movement (not kinematic).
        rb.isKinematic = false;
        rb.freezeRotation = true; // We'll control rotation manually in the script
        col = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Jump input (only if not flying)
        if (!canFly && Keyboard.current.spaceKey.wasPressedThisFrame && IsGrounded())
        {
            jumpRequested = true;
        }

        // Toggle flying mode
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            canFly = !canFly;
        }
    }

    void FixedUpdate()
    {
        if (Keyboard.current == null || cameraTransform == null) return;

        // --- Horizontal WASD input ---
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed) horizontal = -1f;
        if (Keyboard.current.dKey.isPressed) horizontal = 1f;
        if (Keyboard.current.wKey.isPressed) vertical = 1f;
        if (Keyboard.current.sKey.isPressed) vertical = -1f;

        Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 move = Vector3.zero;

        if (inputDir.magnitude > 0.01f)
        {
            // Camera-relative movement
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            move = (cameraForward * vertical + cameraRight * horizontal).normalized * moveSpeed;

            // Smooth rotation toward movement direction
            Quaternion targetRot = Quaternion.LookRotation(new Vector3(move.x, 0f, move.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.fixedDeltaTime);
        }

        // --- Vertical movement ---
        if (canFly)
        {
            // Flying up/down with Q/E
            float verticalInput = 0f;
            if (Keyboard.current.eKey.isPressed) verticalInput += 1f;
            if (Keyboard.current.qKey.isPressed) verticalInput -= 1f;

            velocity.y = verticalInput * flySpeed;
        }
        else
        {
            // Jump
            if (jumpRequested)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpRequested = false;
            }

            // Gravity
            if (!IsGrounded() || velocity.y > 0f)
                velocity.y += gravity * Time.fixedDeltaTime;
            else
                velocity.y = 0f;
        }

        // --- Apply movement using velocities ---
        // rb.velocity is per-second velocity, so don't multiply by deltaTime here.
        Vector3 newVelocity = new Vector3(move.x, velocity.y, move.z);
        rb.linearVelocity = newVelocity;
    }

    private bool IsGrounded()
    {
        Vector3 origin = rb.position + Vector3.up * 0.1f;
        float rayLength = col.bounds.extents.y + groundCheckDistance;
        return Physics.Raycast(origin, Vector3.down, rayLength);
    }
}
