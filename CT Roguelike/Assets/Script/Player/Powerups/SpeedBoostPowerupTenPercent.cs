using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPowerupTenPercent : Powerup
{
    private float originalSpeed;

    protected override void OnActivate()
    {
        // Example: Increase player's speed by 10%
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            originalSpeed = playerMovement.moveSpeed;
            playerMovement.moveSpeed += originalSpeed * 0.1f; // 10% increase
        }
    }

    protected override void OnDeactivate()
    {
        // No deactivation logic as the powerup should remain active
    }
}
