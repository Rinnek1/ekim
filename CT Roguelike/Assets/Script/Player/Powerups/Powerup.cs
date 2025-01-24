using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    protected PowerupManager powerupManager;
    public float duration = 5f; // Default duration for powerups

    // Initialize the powerup
    public void Initialize(PowerupManager manager)
    {
        powerupManager = manager;
        StartCoroutine(ApplyEffect());
    }

    // Apply the powerup effect
    protected abstract void OnActivate();

    // Revert the powerup effect
    protected abstract void OnDeactivate();

    // Coroutine to handle powerup duration
    private IEnumerator ApplyEffect()
    {
        OnActivate();
        yield return new WaitForSeconds(duration);
        OnDeactivate();
        powerupManager.RemovePowerup(this);
    }
}
