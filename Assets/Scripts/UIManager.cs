using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Photon.Pun;


public class UIManager : MonoBehaviourPunCallbacks
{
    #region Classes
    private Tank tank;
    private Player player;
    #endregion

    #region Player Info
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerScoreText;
    public Slider healthBar;
    public Slider reloadBar;
    #endregion

    #region Table of Players
    public GameObject playerTable;
    public GameObject playerListing;
    private List<GameObject> scoreListings = new List<GameObject>();
    #endregion

    #region Game Timer
    public TextMeshProUGUI gameTimer;
    private int minute;
    private int second;
    public static float matchTimer = 300f;
    #endregion

    void Awake()
    {
        // Get the instance of the Tank and Player class
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();

        // Set default values
        playerScoreText.text = "0";
        tank.reloadProgress = 1.0f;
        playerName.text = player.PlayerName;
        playerScoreText.text = player.ScoreCurrent.ToString();

        // Turn on game timer when appropriate
        if (player.gameState == Player.GameState.SM)
        {
            gameTimer.gameObject.SetActive(true);
        }

        // Update the table of players
        UpdateTable();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        // Constantly update the various UI rendered
        healthBar.value = tank.healthCurrent / tank.healthMax;
        reloadBar.value = tank.reloadProgress;
        playerScoreText.text = player.ScoreCurrent.ToString();
        if (player.gameState == Player.GameState.SM)
        {
            UpdateTimer();
        }

        // Display the list of players 
        if (Input.GetKeyUp(KeyCode.P))
        {
            playerTable.SetActive(!playerTable.activeSelf);
        }        
    }

    private void UpdateTable()
    {

        if (scoreListings != null)
        {
            foreach (GameObject listing in scoreListings)
            {
                Destroy(listing);
            }
            scoreListings.Clear();
        }

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            // Create and add a player listing
            GameObject tempListing = Instantiate(playerListing);
            tempListing.transform.SetParent(playerTable.transform, false);

            // Add the player listing to the list
            scoreListings.Add(tempListing);

            // Set the players name
            Text tempText = tempListing.GetComponentInChildren<Text>();
            tempText.text = player.NickName;
        }
    }

    public void UpdateTimer()
    {
        matchTimer -= Time.deltaTime;
        second = (int)(matchTimer % 60.0f);
        minute = (int)(matchTimer / 60.0f);
        gameTimer.text = "";
        gameTimer.text = minute.ToString() + ":" + second.ToString("00");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdateTable();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdateTable();
    }

}
