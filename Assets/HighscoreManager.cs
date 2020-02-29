using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreManager : MonoBehaviour
{
    dreamloLeaderBoard dreamlo;
    public GameObject ffaScrollView;
    public GameObject smScrollView;
    public GameObject tbScrollView;
    public GameObject playerScoreListing;
    private List<GameObject> playerListings = new List<GameObject>();
    private Player player;

    List<dreamloLeaderBoard.Score> scores = new List<dreamloLeaderBoard.Score>();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        this.dreamlo = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
    }

    private void Update()
    {
        if(player.returning && player.PlayerName != null)
        {            
            dreamlo.AddScore(player.PlayerName, player.ScoreCurrent, 0, player.gameState.ToString());
            player.returning = false;
            player.gameState = Player.GameState.Lobby;
            player.ResetPlayerStats();
        }
    }

    public void DisplayScores(List<dreamloLeaderBoard.Score> scores)
    {
        Debug.Log(scores.Count);
        int count = 1;
        foreach(GameObject listing in playerListings)
        {
            Destroy(listing);
        }
        playerListings.Clear();

        foreach (dreamloLeaderBoard.Score score in scores)
        {
            int playerScore = score.score;
            string playerName = score.playerName.Replace("+", " ");

            // Create and add a player listing
            GameObject tempListing = Instantiate(playerScoreListing);
            switch(score.shortText)
            {
                case "FFA":
                    tempListing.transform.SetParent(ffaScrollView.transform, false);
                    break;
                case "SM":
                    tempListing.transform.SetParent(smScrollView.transform, false);
                    break;
                case "TB":
                    tempListing.transform.SetParent(tbScrollView.transform, false);
                    break;
            }

            // Set the players name and score
            Text[] tempText = tempListing.GetComponentsInChildren<Text>();
            tempText[0].text = count.ToString() + ".  " + playerName;
            tempText[1].text = playerScore.ToString();
            playerListings.Add(tempListing);
            count++;
        }
    }
}
