using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;

    void Update()
    {
        if (Keyboard.current == null)
        return;

        float horizontal = 0f;
        float vertical = 0f;
        float fly = 0f;

        // WASD
        if (Keyboard.current.aKey.isPressed) horizontal = -1f;
        if (Keyboard.current.dKey.isPressed) horizontal = 1f;
        if (Keyboard.current.wKey.isPressed) vertical = 1f;
        if (Keyboard.current.sKey.isPressed) vertical = -1f;

        Vector3 direction = new Vector3(horizontal, fly, vertical);

        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.Self);
    }
}
