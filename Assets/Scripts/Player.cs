using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.1f;

    private Rigidbody rb;
    private CapsuleCollider col;
    private Vector3 velocity;
    private bool jumpRequested = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Kinematic Rigidbody
        col = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Jump input
        if (Keyboard.current.spaceKey.wasPressedThisFrame && IsGrounded())
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // WASD input
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed) horizontal = -1f;
        if (Keyboard.current.dKey.isPressed) horizontal = 1f;
        if (Keyboard.current.wKey.isPressed) vertical = 1f;
        if (Keyboard.current.sKey.isPressed) vertical = -1f;

        Vector3 move = new Vector3(horizontal, 0f, vertical).normalized * moveSpeed * Time.fixedDeltaTime;

        // Jump
        if (jumpRequested)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpRequested = false;
        }

        // Apply gravity if not grounded
        if (!IsGrounded() || velocity.y > 0f)
        {
            velocity.y += gravity * Time.fixedDeltaTime;
        }
        else
        {
            velocity.y = 0f; // Reset vertical velocity when grounded
        }

        // Combine movement
        Vector3 finalMove = move + new Vector3(0, velocity.y * Time.fixedDeltaTime, 0);

        // Move the kinematic Rigidbody
        rb.MovePosition(rb.position + transform.TransformDirection(finalMove));
    }

    // Ground check using Raycast
    private bool IsGrounded()
    {
        Vector3 origin = rb.position + Vector3.up * 0.1f; // Slight offset to avoid self-collision
        float rayLength = col.bounds.extents.y + groundCheckDistance;
        return Physics.Raycast(origin, Vector3.down, rayLength);
    }
}
