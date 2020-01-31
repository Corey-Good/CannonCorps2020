using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/************************************************************************/
/* Author: Eddie Habal */
/* Date Created: 1/29/2020 */
/* Last Modified Date: */
/* Modified By: */
/************************************************************************/

public class Player : MonoBehaviour
{

    private int playerID;
    public int PlayerID
    {
        get { return playerID; }
        set { playerID = value; }
    }
    private string playerName;
    private int killsCurrent;
    private int killsAlltime;
    private int killsInARow;
    private int scoreCurrent;
    private int scoreAlltime;
    private int deathsCurrent;
    private int deathsAlltime;
    private int deathsInARow;

    private static Player playerInstance;
    private void Awake()
    {
        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GivePoints(int points)
    {
        scoreCurrent += points;
    }

}
