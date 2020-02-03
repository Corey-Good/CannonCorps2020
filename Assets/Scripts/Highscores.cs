using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscore : MonoBehaviour
{
    private string firstPlaceScore = "firstPlaceScore";
    private string firstPlaceName = "firstPlaceName";
    private string secondPlaceScore;
    private string secondPlaceName;
    private string thirdPlaceScore;
    private string thirdPlaceName;

    // These are used to 
    private void OnEnable()
    {
        Player.OnPlayerReturnsToMenu += UpdatePermanentTable;
    }

    private void UpdatePermanentTable(Player player)
    {
        if (PlayerPrefs.GetInt(player.PlayerID) < player.ScoreCurrent) //Only update if score in highscore table is lower than current score
        {
            PlayerPrefs.SetInt(player.PlayerID, player.ScoreCurrent);
        }

    }

    private void OnDisable()
    {
        Player.OnPlayerReturnsToMenu -= UpdatePermanentTable;
    }
}
