using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    // List of currently active powerups
    private List<Powerup> activePowerups = new List<Powerup>();

    // Apply a new powerup to the player
    public void AddPowerup(Powerup powerup)
    {
        // Create an instance of the powerup and attach it to the player
        Powerup newPowerup = gameObject.AddComponent(powerup.GetType()) as Powerup;
        if (newPowerup != null)
        {
            newPowerup.Initialize(this);
            activePowerups.Add(newPowerup);
        }
    }

    // Remove a powerup from the active list
    public void RemovePowerup(Powerup powerup)
    {
        if (activePowerups.Contains(powerup))
        {
            activePowerups.Remove(powerup);
            Destroy(powerup);
        }
    }
}