using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadPowerUp : PowerUp
{
    public float reloadBoost = 0.5f;


    protected override void PowerUpPayload()          // Checklist item 1
    {
        base.PowerUpPayload();
        playerBrain.SetReloadBoostOn(reloadBoost);
    }

    protected override void PowerUpHasExpired()       // Checklist item 2
    {
        playerBrain.SetReloadBoostOff();
        base.PowerUpHasExpired();
    }
}
