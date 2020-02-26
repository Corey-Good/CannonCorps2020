using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SpeedPowerUp : PowerUp, IPlayerEvents
{
    public float rotateMultiplier = 16f;
    public float speedMultiplier = 2.0f;

    protected override void PowerUpPayload()          // Checklist item 1
    {
        base.PowerUpPayload();
        playerBrain.SetSpeedBoostOn(speedMultiplier, rotateMultiplier);
    }

    protected override void PowerUpHasExpired()       // Checklist item 2
    {
        playerBrain.SetSpeedBoostOff();
        base.PowerUpHasExpired();
    }

    void IPlayerEvents.OnPlayerHurt(int newHealth)
    {
        // You only want to react once collected
        if (powerUpState != PowerUpState.IsCollected)
        {
            return;
        }

        // You expire when player hurt
        PowerUpHasExpired();
    }

    void IPlayerEvents.OnPlayerReachedExit(GameObject exit)
    {
        
    }
}

