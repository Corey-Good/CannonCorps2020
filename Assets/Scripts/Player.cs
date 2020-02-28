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
    public bool inGame = false;
    public PhotonView photonView;

    private static Player playerInstance;
    public enum GameState
    {
        FFA, 
        SM, 
        TB,
        TT,
        Lobby
    }
    public GameState gameState;

    private void UpdateHighscoreTable(string returningGameMode)
    {
        inGame = true;
        switch(returningGameMode)
        {
            case "FFA":
                gameState = GameState.FFA;
                break;
            case "SM":
                gameState = GameState.SM;
                break;
            case "TB":
                gameState = GameState.TB;
                break;
        }
    }

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

    public void ResetPlayerStats()
    {
        KillsAlltime  += KillsCurrent;
        KillsCurrent   = 0;  
        ScoreAlltime  += ScoreCurrent;
        ScoreCurrent   = 0;
        DeathsAlltime += DeathsCurrent;
        DeathsCurrent  = 0;        
        DeathsInARow   = 0;
        KillsInARow    = 0;

        Debug.Log("Stats have been reset");
    }



}
