using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SpeedPowerUp : PowerUp, IPowerUpEvents
{
    public float rotateMultiplier = 16f;
    public float speedMultiplier = 3.0f;
    private float speedTimer;

    protected override void PowerUpPayload()          // Checklist item 1
    {
        base.PowerUpPayload();
        playerBrain.SetSpeedBoostOn(speedMultiplier, rotateMultiplier);
        base.StartListening(this.gameObject);
    }

    protected override void PowerUpHasExpired()       // Checklist item 2
    {
        playerBrain.SetSpeedBoostOff();
        base.PowerUpHasExpired();
    }


    void IPowerUpEvents.OnReloadBoostExpired()
    {
    }

    void IPowerUpEvents.ToggleReloadBoost()
    {
    }

    void IPowerUpEvents.OnSpeedBoostExpired()
    {
        // You only want to react once collected
        if (powerUpState != PowerUpState.IsCollected)
        {
            return;
        }
        speedTimer = 0.0f;
        // Expire when timer runs out
        PowerUpHasExpired();
    }

    void IPowerUpEvents.ToggleSpeedBoost()
    {
        if (playerBrain.speedBoostTimerRunning && playerBrain.speedBoostTimer > 0.0f)
        {
            speedTimer = playerBrain.speedBoostTimer;
            playerBrain.SetSpeedBoostOff();
        }
        else if ((!playerBrain.speedBoostTimerRunning) && playerBrain.speedBoostTimer > 0.0f)
        {
            playerBrain.SetSpeedBoostOn(speedMultiplier, rotateMultiplier, speedTimer);
            speedTimer = 0.0f;
        }
    }
}

