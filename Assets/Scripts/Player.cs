using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 7f;

    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Keyboard.current == null)
        return;

        float horizontal = 0f;
        float vertical = 0f;

        // WASD
        if (Keyboard.current.aKey.isPressed) horizontal = -1f;
        if (Keyboard.current.dKey.isPressed) horizontal = 1f;
        if (Keyboard.current.wKey.isPressed) vertical = 1f;
        if (Keyboard.current.sKey.isPressed) vertical = -1f;

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            isGrounded = true;
        }
    }
}
