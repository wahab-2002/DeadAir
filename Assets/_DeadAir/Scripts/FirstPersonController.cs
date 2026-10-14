using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] Transform view;
    [SerializeField] NoteViewer notes;
    [SerializeField] float walkSpeed = 1.8f;
    [SerializeField] float mouseSensitivity = 0.24f;
    [SerializeField] float cameraHeight = 1.2f;
    float pitch;
    float verticalSpeed;

    void Awake() { view.localPosition = new Vector3(view.localPosition.x, cameraHeight, view.localPosition.z); }

    void OnEnable() { LockCursor(); }
    void OnDisable() { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }

    void Update()
    {
        if (notes.IsOpen) return;
        // Clicking the Game view restores the lock after the Editor releases it.
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) LockCursor();
        if (Cursor.lockState != CursorLockMode.Locked) return;
        if (Mouse.current != null)
        {
            Vector2 delta = Mouse.current.delta.ReadValue() * mouseSensitivity;
            transform.Rotate(0, delta.x, 0);
            pitch = Mathf.Clamp(pitch - delta.y, -80, 80);
            view.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
        Vector2 move = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) move.y += 1;
            if (Keyboard.current.sKey.isPressed) move.y -= 1;
            if (Keyboard.current.dKey.isPressed) move.x += 1;
            if (Keyboard.current.aKey.isPressed) move.x -= 1;
        }
        move = Vector2.ClampMagnitude(move, 1);
        if (controller.isGrounded && verticalSpeed < 0) verticalSpeed = -2;
        verticalSpeed += Physics.gravity.y * Time.deltaTime;
        Vector3 velocity = (transform.right * move.x + transform.forward * move.y) * walkSpeed;
        velocity.y = verticalSpeed;
        controller.Move(velocity * Time.deltaTime);
    }

    void LockCursor() { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
}
