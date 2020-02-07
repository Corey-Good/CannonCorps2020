using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/************************************************************************/
/* Author: Eddie Habal */
/* Date Created: 1/29/2020 */
/* Last Modified Date: */
/* Modified By: */
/************************************************************************/

public class Player : MonoBehaviour
{
    public delegate void PlayerReturnsToMenu(Player player);
    public static event PlayerReturnsToMenu OnPlayerReturnsToMenu;
    public string PlayerID { get; set; }
    public string PlayerName { get; set; }
    public int KillsCurrent { get; set; }
    public int KillsAlltime { get; set; }
    public int KillsInARow { get; set; }
    public int ScoreCurrent { get; set; }
    public int ScoreAlltime { get; set; }
    public int DeathsCurrent { get; set; }
    public int DeathsAlltime { get; set; }
    public int DeathsInARow { get; set; }

    private static Player playerInstance;

    public enum GameState
    {
        FFA, 
        SM, 
        TB
    }
    public GameState gameState;
    private void Awake()
    {
        this.PlayerID = CreatePlayerID(); // assign random, new player ID
        // Could add some kind of logic to see if player wants to reuse player ID
        // "Enter ID" and if ID matches then use that player

        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private string CreatePlayerID()
    {
        string newPlayerID = Random.Range(1, 1000).ToString();

        while (PlayerPrefs.HasKey(newPlayerID)) //If playerID is already taken in the highscore table, then create a new one
        {
            newPlayerID = Random.Range(1, 1000).ToString();
        }

        return newPlayerID;
    }

    private void OnDisable() // Using OnDisable for testing purposes, should be called during player state change
    {
        Debug.Log("GameState.FFA.ToString()" + GameState.FFA.ToString());
        Debug.Log("gameState.ToString()" + gameState.ToString());
        this.PlayerName = PhotonNetwork.NickName;
        Debug.Log("this is the nickname:" + PlayerName);
        this.ScoreCurrent = 90;
        Debug.Log(PlayerPrefs.GetString("highscoretable"));
        Debug.Log(PlayerPrefs.HasKey("highscoretable"));
        if (OnPlayerReturnsToMenu != null)
        {
            OnPlayerReturnsToMenu(this);
        }

        if(this.ScoreCurrent > this.ScoreAlltime)
        {
            this.ScoreAlltime = this.ScoreCurrent;
        }
        this.ScoreCurrent = 0;
        //Debug.Log("Player ID" + this.PlayerID);
        //Debug.Log("Player Highscore" + PlayerPrefs.GetInt(this.PlayerID));
        //Debug.Log("Current Score" + this.ScoreCurrent.ToString());

        //Debug.Log("Current Score" + this.ScoreCurrent.ToString());

    }

}
