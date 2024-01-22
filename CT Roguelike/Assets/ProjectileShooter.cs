using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;  // Reference to the projectile prefab
    public Transform shootPoint;         // Reference to the point where the projectile will be spawned
    public float shootForce = 10f;       // Force applied to the projectile

    void Update()
    {
        // Check for input to shoot
        if (Input.GetButtonDown("Fire1"))  // You can customize the input button
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        // Instantiate the projectile at the shoot point
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        // Get the rigidbody component of the projectile
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // Check if the projectile has a rigidbody
        if (projectileRb != null)
        {
            // Apply force to the projectile in the forward direction
            projectileRb.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("Projectile is missing Rigidbody component.");
        }
    }
}