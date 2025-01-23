using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : MonoBehaviour
{
    [Header("General Settings")]
    public Transform bulletSpawnPoint; // Where the bullets spawn
    public GameObject bulletPrefab; // The bullet prefab
    public float bulletSpeed = 20f; // Speed of the bullets
    public float fireRate = 0.1f; // Time between shots
    public int magazineSize = 30; // Maximum number of bullets per magazine
    public float reloadTime = 2f; // Time taken to reload
    public AudioClip shootSound; // Sound effect for shooting
    public AudioClip reloadSound; // Sound effect for reloading
    public ParticleSystem muzzleFlash; // Muzzle flash effect

    [Header("Spread Settings")]
    public float spreadAngle = 2f; // Maximum angle of spread in degrees
    public bool isSpreadIncreasing = true; // Spread increases as you keep firing
    public float maxSpreadAngle = 5f; // Maximum spread angle
    public float spreadIncreaseRate = 0.5f; // Rate at which spread increases
    public float spreadResetRate = 2f; // Rate at which spread resets when not firing

    private int currentAmmo; // Current bullets in the magazine
    private float nextFireTime = 0f; // Time when the rifle can fire again
    private bool isReloading = false; // Flag to check if the rifle is reloading
    private float currentSpread; // Current spread angle
    private AudioSource audioSource;

    void Start()
    {
        // Initialize the ammo count
        currentAmmo = magazineSize;

        // Add an AudioSource component to the rifle if not already present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Initialize spread
        currentSpread = spreadAngle;
    }

    void Update()
    {
        // Fire weapon when the left mouse button is held down
        if (isReloading)
        {
            return; // Do nothing while reloading
        }

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Fire();

            // Increase spread if the mechanic is enabled
            if (isSpreadIncreasing)
            {
                currentSpread = Mathf.Min(currentSpread + spreadIncreaseRate, maxSpreadAngle);
            }
        }
        else
        {
            // Gradually reset spread when not firing
            currentSpread = Mathf.Max(currentSpread - spreadResetRate * Time.deltaTime, spreadAngle);
        }

        // Reload if the reload button ("R") is pressed or the magazine is empty
        if (Input.GetKeyDown(KeyCode.R) || currentAmmo == 0)
        {
            StartCoroutine(Reload());
        }
    }

    void Fire()
    {
        nextFireTime = Time.time + fireRate; // Set the next available fire time

        // Apply spread to the bullet's direction
        Vector3 spreadDirection = ApplySpread(bulletSpawnPoint.forward);

        // Spawn the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(spreadDirection));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = spreadDirection * bulletSpeed;
        }

        // Play muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Play shooting sound
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Decrease ammo count after each shot
        currentAmmo--;

        // Destroy the bullet after a few seconds
        Destroy(bullet, 2f);
    }

    Vector3 ApplySpread(Vector3 baseDirection)
    {
        // Randomize spread angle within the current spread range
        float xSpread = Random.Range(-currentSpread, currentSpread);
        float ySpread = Random.Range(-currentSpread, currentSpread);

        // Apply spread to the base direction
        Quaternion spreadRotation = Quaternion.Euler(xSpread, ySpread, 0);
        return spreadRotation * baseDirection;
    }

    IEnumerator Reload()
    {
        if (currentAmmo == magazineSize || isReloading)
        {
            yield break; // Skip reload if already full or reloading
        }

        isReloading = true;

        // Play reload sound
        if (reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        // Wait for reload time
        yield return new WaitForSeconds(reloadTime);

        // Refill the ammo
        currentAmmo = magazineSize;

        isReloading = false;
    }

    // For debugging: Display current ammo and spread in the console
    void OnGUI()
    {
        GUILayout.Label($"Ammo: {currentAmmo} / {magazineSize}");
        GUILayout.Label($"Current Spread: {currentSpread:F2}°");
    }
}