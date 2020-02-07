using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Highscores : MonoBehaviour, IGivePoints
{
    public void GivePoints(Player player, int points)
    {
        player.ScoreCurrent += points;
    }
}
