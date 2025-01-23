using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : MonoBehaviour
{
    public Transform bulletSpawnPoint; // Where the bullets spawn
    public GameObject bulletPrefab; // The bullet prefab
    public float bulletSpeed = 20f; // Speed of the bullets
    public float fireRate = 0.1f; // Time between shots
    public int magazineSize = 30; // Maximum number of bullets per magazine
    public float reloadTime = 2f; // Time taken to reload
    public AudioClip shootSound; // Sound effect for shooting
    public AudioClip reloadSound; // Sound effect for reloading
    public ParticleSystem muzzleFlash; // Muzzle flash effect

    private int currentAmmo; // Current bullets in the magazine
    private float nextFireTime = 0f; // Time when the rifle can fire again
    private bool isReloading = false; // Flag to check if the rifle is reloading
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

        // Spawn the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
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

    // For debugging: Display current ammo in the console
    void OnGUI()
    {
        GUILayout.Label("Ammo: " + currentAmmo + " / " + magazineSize);
    }
}
