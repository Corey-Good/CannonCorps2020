using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ITakeDamage
{
    void damageTaken(float damage);
}

interface IGivePoints
{
    void GivePoints(Player player, int points);
}

