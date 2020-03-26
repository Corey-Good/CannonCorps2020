using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPowerUp : PowerUp
{
    private float damage = 5f;
    protected override void PowerUpPayload()          // Checklist item 1
    {
        base.PowerUpPayload();
        playerBrain.SetHealthBoost(damage);
    }

    private void OnEnable()
    {
        StartCoroutine(LaserTimer());
    }

    IEnumerator LaserTimer()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);
        PowerUpHasExpired();
    }

    protected override void PowerUpHasExpired()       // Checklist item 2
    {
        base.PowerUpHasExpired();
    }
}
