using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Photon.Pun;


public class UIManager : MonoBehaviourPunCallbacks
{
    private Tank tank;
    private Player player;

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerScoreText;
    public Slider healthBar;
    public Slider reloadBar;

    public GameObject playerTable;
    public GameObject playerListing;
    private List<GameObject> scoreListings = new List<GameObject>();

    void Awake()
    {
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        playerScoreText.text = "0";
        tank.reloadProgress = 1.0f;
        UpdateTable();
    }

    private void Start()
    {
        playerName.text = player.PlayerName;
        playerScoreText.text = player.ScoreCurrent.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthBar.value = tank.healthCurrent / tank.healthMax;
        reloadBar.value = tank.reloadProgress;
        playerScoreText.text = player.ScoreCurrent.ToString();
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

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdateTable();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdateTable();
    }

}
