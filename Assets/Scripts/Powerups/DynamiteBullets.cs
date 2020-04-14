using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteBullets : PowerUp
{
    protected override void PowerUpPayload()
    {
        base.PowerUpPayload();
        playerBrain.CollectDynamiteBullets();
    }
}