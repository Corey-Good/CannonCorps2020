using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullets : PowerUp
{
    protected override void PowerUpPayload()
    {
        base.PowerUpPayload();
        playerBrain.CollectFreezeBullets();
    }
}
