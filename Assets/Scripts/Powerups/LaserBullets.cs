using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullets : PowerUp
{
    protected override void PowerUpPayload()
    {
        base.PowerUpPayload();
        playerBrain.CollectLaserBullets();
    }
}
