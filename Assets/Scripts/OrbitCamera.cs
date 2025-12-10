using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;          // The player
    public float distance = 5f;       // Distance from player
    public float mouseSensitivity = 100f;
    public float pitchMin = -30f;
    public float pitchMax = 60f;

    private float yaw = 0f;
    private float pitch = 20f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (target == null)
            Debug.LogError("OrbitCamera: No target assigned!");
    }

    void LateUpdate()
    {
        if (Mouse.current == null || target == null) return;

        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

        // Adjust yaw/pitch
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Compute camera position
        Vector3 offset = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = target.position + rotation * offset;

        // Look at the player's head
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
