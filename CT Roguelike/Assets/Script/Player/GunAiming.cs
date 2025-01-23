using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAiming : MonoBehaviour
{
    public Transform playerCamera; // Reference to the player's camera
    public Transform gunTransform; // Reference to the gun's transform

    public float rotationSpeed = 10f; // Speed of rotation for aiming the gun

    void Update()
    {
        AimGun();
    }

    void AimGun()
    {
        if (playerCamera != null && gunTransform != null)
        {
            // Get the camera's forward direction
            Vector3 targetDirection = playerCamera.forward;

            // Create a rotation that faces the target direction
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Smoothly rotate the gun towards the target rotation
            gunTransform.rotation = Quaternion.Slerp(gunTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
