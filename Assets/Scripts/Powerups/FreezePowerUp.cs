using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePowerUp : PowerUp
{
    public float rotateMultiplier = 0f;
    public float speedMultiplier = 0f;
    public float Damage = -5.0f;

    protected override void PowerUpPayload()          // Checklist item 1
    {
        base.PowerUpPayload();
        playerBrain.SetSpeedBoostOff();
        playerBrain.SetSpeedBoostOn(speedMultiplier, rotateMultiplier);
        playerBrain.SetHealthBoost(Damage);
        StartCoroutine(FreezeTimer());
    }

    IEnumerator FreezeTimer()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);
        PowerUpHasExpired();
    }

    protected override void PowerUpHasExpired()       // Checklist item 2
    {
        playerBrain.SetSpeedBoostOff();
        base.PowerUpHasExpired();
    }
}
