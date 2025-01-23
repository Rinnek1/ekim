using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player's transform
    public Vector3 offset = new Vector3(0, 5, -10); // Offset position relative to the player
    public float followSpeed = 5f; // Speed at which the camera follows the player
    public float mouseSensitivity = 100f; // Sensitivity for mouse input
    public float minYAngle = -20f; // Minimum vertical angle of the camera
    public float maxYAngle = 80f; // Maximum vertical angle of the camera
    public float verticalLookOffset = 1.5f; // Adjust the look target to make the player appear closer to the bottom

    private float currentYaw = 0f; // Horizontal rotation angle
    private float currentPitch = 0f; // Vertical rotation angle

    void Start()
    {
        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogError("Target is not assigned! Please assign the player's transform.");
            return;
        }

        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust yaw (horizontal rotation) and pitch (vertical rotation)
        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minYAngle, maxYAngle);

        // Calculate rotation from yaw and pitch
        Quaternion cameraRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

        // Calculate the desired position of the camera
        Vector3 desiredPosition = target.position + cameraRotation * offset;

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Adjust the look target to make the player appear closer to the bottom
        Vector3 lookTarget = target.position + Vector3.up * verticalLookOffset;

        // Rotate the camera to look at the adjusted target
        transform.LookAt(lookTarget);
    }
}
