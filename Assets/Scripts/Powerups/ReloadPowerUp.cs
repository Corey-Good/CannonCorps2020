using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadPowerUp : PowerUp, IPowerUpEvents
{
    private float reloadBoost = 0.5f;
    private float reloadTime;


    protected override void PowerUpPayload()          // Checklist item 1
    {
        base.PowerUpPayload();
        playerBrain.SetReloadBoostOn(reloadBoost);
        playerBrain.SetReloadBoostOff();
        StartListening(this.gameObject);
    }

    void IPowerUpEvents.OnReloadBoostExpired()
    {
        // You only want to react once collected
        if (powerUpState != PowerUpState.IsCollected)
        {
            return;
        }
        reloadTime = 0.0f;
        // Expire when timer runs out
        PowerUpHasExpired();
    }

    protected override void PowerUpHasExpired()       // Checklist item 2
    {
        playerBrain.SetReloadBoostOff();
        base.PowerUpHasExpired();
    }

    void IPowerUpEvents.ToggleReloadBoost()
    {
        if(playerBrain.reloadBoostTimerRunning && playerBrain.reloadBoostTimer > 0.0f)
        {
            reloadTime = playerBrain.reloadBoostTimer;
            playerBrain.SetReloadBoostOff();
        }
        else if((!playerBrain.reloadBoostTimerRunning) && playerBrain.reloadBoostTimer > 0.0f)
        {
            playerBrain.SetReloadBoostOn(reloadBoost, reloadTime);
            reloadTime = 0.0f;
        }

    }

    void IPowerUpEvents.OnSpeedBoostExpired()
    {
    }

    void IPowerUpEvents.ToggleSpeedBoost()
    {
    }
}
