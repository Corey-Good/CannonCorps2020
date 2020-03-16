using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullets : PowerUp
{
    public float laserBullets = 15.0f;
    protected override void PowerUpPayload()
    {
        base.PowerUpPayload();
        playerBrain.CollectLaserBullets(laserBullets);
    }
}
