using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteBullets : PowerUp
{
    public float dynamiteBullets = 5.0f;
    protected override void PowerUpPayload()
    {
        base.PowerUpPayload();
        playerBrain.CollectDynamiteBullets(dynamiteBullets);
    }
}