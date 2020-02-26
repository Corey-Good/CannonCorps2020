using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Highscores : MonoBehaviour, IGivePoints
{
    KeyCode Damage;
    public Tank tank;

    private void Awake()
    {
        Damage = KeyCode.Keypad1;
    }

    void Update()
    {
        // Move play forwards and backwards, 
        if (Input.GetKey(Damage))
        {
            tank.damageTaken(10);
        }
    }
    public void GivePoints(Player player, int points)
    {
        player.ScoreCurrent += points;
    }
}
