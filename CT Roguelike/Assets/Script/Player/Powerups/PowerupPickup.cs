using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    public Powerup powerupPrefab; // Assign a powerup prefab in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PowerupManager powerupManager = other.GetComponent<PowerupManager>();
            if (powerupManager != null)
            {
                // Instantiate the powerup and apply it to the player
                Powerup newPowerup = Instantiate(powerupPrefab);
                powerupManager.AddPowerup(newPowerup);
            }

            // Destroy the pickup after use
            Destroy(gameObject);
        }
    }
}
