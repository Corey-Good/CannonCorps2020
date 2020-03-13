using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadPowerUp : PowerUp, IPowerUpEvents
{
    public float reloadBoost = 0.5f;
    public float reloadTime = 10.0f;


    protected override void PowerUpPayload()          // Checklist item 1
    {
        base.PowerUpPayload();
        playerBrain.SetReloadBoostOn(reloadBoost, reloadTime);

    }

    void IPowerUpEvents.OnReloadBoostExpired()
    {
        // You only want to react once collected
        if (powerUpState != PowerUpState.IsCollected)
        {
            return;
        }

        // You expire when player hurt
        PowerUpHasExpired();
    }

    protected override void PowerUpHasExpired()       // Checklist item 2
    {
        playerBrain.SetReloadBoostOff();
        base.PowerUpHasExpired();
    }

    void IPowerUpEvents.OnReloadBoostOn()
    {
        reloadTime = playerBrain.reloadBoostTimer;
        playerBrain.SetReloadBoostOff();
    }
}
