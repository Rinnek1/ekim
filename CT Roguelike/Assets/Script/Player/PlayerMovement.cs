using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float rotationSpeed = 720f; // Rotation speed in degrees per second

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to the capsule
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody not found! Please attach a Rigidbody to the capsule.");
        }
    }

    void FixedUpdate()
    {
        // Get input from WASD/Arrow keys
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // Move the capsule
            Vector3 targetPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);

            // Rotate the capsule to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}
