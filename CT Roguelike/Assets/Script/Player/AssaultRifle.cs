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

    [Header("Damage Settings")]
    public float bulletDamage = 20f; // Damage dealt by each bullet

    private int currentAmmo; // Current bullets in the magazine
    private float nextFireTime = 0f; // Time when the rifle can fire again
    private bool isReloading = false; // Flag to check if the rifle is reloading
    private float currentSpread; // Current spread angle
    private AudioSource audioSource;

    void Start()
    {
        currentAmmo = magazineSize;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        currentSpread = spreadAngle;
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Fire();

            if (isSpreadIncreasing)
            {
                currentSpread = Mathf.Min(currentSpread + spreadIncreaseRate, maxSpreadAngle);
            }
        }
        else
        {
            currentSpread = Mathf.Max(currentSpread - spreadResetRate * Time.deltaTime, spreadAngle);
        }

        if (Input.GetKeyDown(KeyCode.R) || currentAmmo == 0)
        {
            StartCoroutine(Reload());
        }
    }

    void Fire()
    {
        nextFireTime = Time.time + fireRate;

        Vector3 spreadDirection = ApplySpread(bulletSpawnPoint.forward);

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(spreadDirection));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = spreadDirection * bulletSpeed; // Apply velocity to the bullet
        }

        ARBullet bulletScript = bullet.GetComponent<ARBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDamage(bulletDamage);

            // Ignore collision with the player or gun
            Collider bulletCollider = bullet.GetComponent<Collider>();
            Collider gunCollider = GetComponent<Collider>();
            if (bulletCollider != null && gunCollider != null)
            {
                Physics.IgnoreCollision(bulletCollider, gunCollider);
            }
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        currentAmmo--;
        Destroy(bullet, 2f);
    }

    Vector3 ApplySpread(Vector3 baseDirection)
    {
        float xSpread = Random.Range(-currentSpread, currentSpread);
        float ySpread = Random.Range(-currentSpread, currentSpread);
        Quaternion spreadRotation = Quaternion.Euler(xSpread, ySpread, 0);
        return spreadRotation * baseDirection;
    }

    IEnumerator Reload()
    {
        if (currentAmmo == magazineSize || isReloading) yield break;

        isReloading = true;

        if (reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;
        isReloading = false;
    }

    void OnGUI()
    {
        GUILayout.Label($"Ammo: {currentAmmo} / {magazineSize}");
        GUILayout.Label($"Current Spread: {currentSpread:F2}°");
    }
}