using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;

    Transform cam;
    float camRotation = 0f;

    [SerializeField] float mouseSensitivity = 200f;
    [SerializeField] float movementSpeed = 0.2f;

    [Header("Smoothing\n")]
    // Smooth player movement
    float Smoothment = 0.3f;
    Vector2 curDir = Vector2.zero;
    Vector2 curDirVelocity = Vector2.zero;

    // Smooth mouse movement
    float mouseSmoothment = 0.03f;
    Vector2 curMouseMove = Vector2.zero;
    Vector2 curMouseMoveVelocity = Vector2.zero;

    // Gravity
    float yVel = 0f; // Keeps track of downward speed
    float gravity = -13f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = transform.GetChild(0).transform;
    }

    void Update()
    {
        movement();
        lookAtMouse();
    }

    void movement()
    {
        // Gets the current keyboard input
        Vector2 target = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Normalizes the movement through each direction (since moving diagonal can make the player go farther, which makes it unbalanced)
        target.Normalize();

        // Smoothens the transition between vectors
        curDir = Vector2.SmoothDamp(curDir, target, ref curDirVelocity, Smoothment);

        if (controller.isGrounded) yVel = 0f;

        // Increases downwards gravity as player falls
        yVel += gravity * Time.deltaTime;

        // Directional movement
        // 'Vector3.up' instead of down because 'Yvel' is a minus (player would go up if they weren't grounded)
        Vector3 move = ((transform.forward * curDir.y + transform.right * curDir.x) * movementSpeed + Vector3.up * yVel) * Time.deltaTime;

        // Character controller handles movement and collision
        controller.Move(move);
    }

    void lookAtMouse()
    {
        // Gets the current mouse input
        Vector2 target = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Smoothens the transition between vectors
        curMouseMove = Vector2.SmoothDamp(curMouseMove, target, ref curMouseMoveVelocity, mouseSmoothment);

        // y-axis of mouse to effect the value of camera pitch
        // '-=' instead of '+=' to solve the inverse problem (unless intentional)
        camRotation -= curMouseMove.y * mouseSensitivity * Time.deltaTime;
        camRotation = Mathf.Clamp(camRotation, -90, 90);

        // Set the camera's angles
        cam.localEulerAngles = Vector3.right * camRotation;

        // Rotates the player upwards by the x-axis of the mouse
        transform.Rotate(Vector3.up * curMouseMove.x * mouseSensitivity * Time.deltaTime);
    }

    void ItemPickUp()
    {

    }
}
