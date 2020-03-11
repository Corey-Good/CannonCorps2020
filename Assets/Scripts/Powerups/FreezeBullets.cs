using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullets : PowerUp
{
    public float freezeBullets = 10.0f;
    protected override void PowerUpPayload()
    {
        base.PowerUpPayload();
        playerBrain.CollectFreezeBullets(freezeBullets);
    }
}
