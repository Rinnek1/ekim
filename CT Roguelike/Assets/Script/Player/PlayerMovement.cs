using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Normal movement speed
    public float sprintSpeed = 8f; // Sprint speed
    public float rotationSpeed = 720f; // Rotation speed in degrees per second
    public KeyCode sprintKey = KeyCode.LeftShift; // Key to sprint
    public KeyCode moveForwardKey = KeyCode.W; // Custom forward key
    public KeyCode moveBackwardKey = KeyCode.S; // Custom backward key
    public KeyCode moveLeftKey = KeyCode.A; // Custom left key
    public KeyCode moveRightKey = KeyCode.D; // Custom right key

    [Header("Jumping Settings")]
    public float jumpForce = 5f; // Force applied for jumping
    public int maxJumps = 2; // Maximum number of jumps allowed
    public KeyCode jumpKey = KeyCode.Space; // Key to jump
    public LayerMask groundLayer; // Layer mask to identify what counts as ground

    [Header("Stamina Settings")]
    public float maxStamina = 100f; // Maximum stamina value
    public float staminaDrainRate = 20f; // Stamina drain rate per second while sprinting
    public float staminaRegenRate = 10f; // Stamina regeneration rate per second when not sprinting

    private Rigidbody rb;
    private int jumpsRemaining; // Tracks the remaining jumps
    private bool isGrounded = false; // Tracks if the player is on the ground
    private float currentStamina; // Current stamina
    private bool isSprinting = false; // Tracks if the player is sprinting

    void Start()
    {
        // Get the Rigidbody component attached to the capsule
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody not found! Please attach a Rigidbody to the capsule.");
        }

        // Initialize jumpsRemaining and stamina
        jumpsRemaining = maxJumps;
        currentStamina = maxStamina;
    }

    void FixedUpdate()
    {
        // Check if the player is grounded
        GroundCheck();

        // Handle movement
        HandleMovement();
    }

    void Update()
    {
        // Handle sprint input
        HandleSprint();

        // Handle jumping
        if (Input.GetKeyDown(jumpKey) && jumpsRemaining > 0)
        {
            Jump();
        }

        // Debug stamina
        Debug.Log($"Stamina: {currentStamina}");
    }

    private void HandleMovement()
    {
        // Determine movement direction based on input
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(moveForwardKey)) moveZ += 1f;
        if (Input.GetKey(moveBackwardKey)) moveZ -= 1f;
        if (Input.GetKey(moveLeftKey)) moveX -= 1f;
        if (Input.GetKey(moveRightKey)) moveX += 1f;

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // Determine current speed based on sprint state
            float speed = isSprinting ? sprintSpeed : moveSpeed;

            // Move the capsule
            Vector3 targetPosition = rb.position + moveDirection * speed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);

            // Rotate the capsule to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    private void HandleSprint()
    {
        if (Input.GetKey(sprintKey) && currentStamina > 0)
        {
            isSprinting = true;
            currentStamina -= staminaDrainRate * Time.deltaTime; // Drain stamina
        }
        else
        {
            isSprinting = false;
            currentStamina += staminaRegenRate * Time.deltaTime; // Regenerate stamina
        }

        // Clamp stamina to valid range
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    void GroundCheck()
    {
        // Adjust the raycast origin slightly downward to avoid collider issues
        Vector3 rayOrigin = transform.position + Vector3.down * 0.5f;

        // Perform the raycast
        RaycastHit hit;
        bool wasGrounded = isGrounded; // Store previous grounded state
        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, out hit, 0.6f, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            // Reset jumpsRemaining when grounded
            jumpsRemaining = maxJumps;
        }

        // Optional: visualize the raycast in the Scene view
        Debug.DrawRay(rayOrigin, Vector3.down * 0.6f, Color.red);
    }

    void Jump()
    {
        if (jumpsRemaining <= 0) return; // Prevent jumping when out of jumps

        // Apply upward force for jumping
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Reset vertical velocity
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Decrease jumpsRemaining
        jumpsRemaining--;

        Debug.Log($"Jumps Remaining: {jumpsRemaining}");
    }

    private void OnDrawGizmos()
    {
        // Visualize the ground check ray in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.down * 0.5f, Vector3.down * 0.6f);
    }
}