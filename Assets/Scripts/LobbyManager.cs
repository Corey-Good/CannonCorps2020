/************************************************************************/
/* Author:  */
/* Date Created: */
/* Last Modified Date: */
/* Modified By: */
/************************************************************************/

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region Classes
    private Player player;
    private Tank tank;
    #endregion

    #region Lobby Info
    public TextMeshProUGUI playerCount;
    public TextMeshProUGUI lobbyStatus;
    public GameObject LobbyView;
    public GameObject PlayerNames;
    public GameObject playerListingPrefab;
    public RectTransform transitionPanel;
    private List<GameObject> playerListings = new List<GameObject>();
    private float lobbyTimer = 30f;
    private bool beginCountDown = false;
    #endregion

    public int minPlayers = 2;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
    }

    private void FixedUpdate()
    {
        if(beginCountDown)
        {
            lobbyTimer -= Time.deltaTime;
            lobbyStatus.text = "Starting game in " + lobbyTimer.ToString("00");
        }

        if(lobbyTimer <= 0)
        {
            SetUpGame();
            beginCountDown = false;
            lobbyStatus.text = "";
        }

        if(PhotonNetwork.InRoom && (bool)PhotonNetwork.CurrentRoom.CustomProperties["StartGame"])
        {
            Debug.Log("Countdown should start soon");
            BeginCountDown();
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Ready", false } });
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "StartGame", false } });
        }

        UpdatePlayerList();
        UpdateLobbyStatus();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerList();
        UpdateLobbyStatus();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdatePlayerList();
        UpdateLobbyStatus();
    }

    public void LeaveRoom()
    {
        player.gameState = Player.GameState.Lobby;
        PhotonNetwork.LeaveRoom();
        LobbyView.SetActive(false);
        EmptyPlayerList();
    }

    private void UpdatePlayerList()
    {
        int count = 0;
        playerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        playerCount.text += "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

        // Remove the currently listed players
        if (playerListings != null)
        {
            EmptyPlayerList();
        }

        // List the current players in the room
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            count += 1;
            // Create and add a player listing
            GameObject tempListing = Instantiate(playerListingPrefab);
            tempListing.transform.SetParent(PlayerNames.transform, false);

            // Add the player listing to the list
            playerListings.Add(tempListing);

            // Set the players name
            Text tempText = tempListing.GetComponentInChildren<Text>();
            tempText.text = count.ToString() + " " + player.NickName;
        }
    }

    private void EmptyPlayerList()
    {
        // Remove the currently listed players
        if (playerListings != null)
        {
            foreach (GameObject listing in playerListings)
            {
                Destroy(listing);
            }
            playerListings.Clear();
        }
    }

    private void UpdateLobbyStatus()
    {
        playerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }

    private void SetUpGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;

        if (player.gameState == Player.GameState.SM)
        {
            // Set the Shark to the Master Client
            if (PhotonNetwork.IsMasterClient)
            {
                tank.tankModel = "futureTank";
                tank.healthCurrent = 100f;
            }
            // Set everyone else in the room to the base tank with minmal health
            else
            {
                tank.tankModel = "baseTank";
                tank.healthCurrent = 10f;
            }

            StartCoroutine(LoadGame(3));
        }
        else if (player.gameState == Player.GameState.TB)
        {
            Photon.Realtime.Player[] networkPlayers = PhotonNetwork.PlayerList;
            int count = 1;
            int totalPlayers = networkPlayers.Length;

            foreach (Photon.Realtime.Player player in networkPlayers)
            {
                if (count <= (int)totalPlayers / 2)
                {
                    player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "team", 0 } });
                }
                else
                {
                    player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "team", 1 } });
                }

                count++;
            }

            StartCoroutine(LoadGame(4));
        }
    }

    private IEnumerator LoadGame(int scene)
    {
        LeanTween.alpha(transitionPanel, 1, 1);
        yield return new WaitForSeconds(1f);
        PhotonNetwork.LoadLevel(scene);
    }

    public void ReadyUp(Button btn)
    {
        Debug.Log("CLicked ready up");
        PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Ready", true } });

        int readyPlayers = 0;
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        foreach (Photon.Realtime.Player player in players)
        {
            if ((bool)player.CustomProperties["Ready"])
            {
                readyPlayers++;
            }
        }
        Debug.Log("Ready players is: " + readyPlayers);
        if (readyPlayers >= minPlayers)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "StartGame", true } });
            Debug.Log("Setting this value to true");

        }

        btn.GetComponentInChildren<TextMeshProUGUI>().text = "Ready";
    }

    public void BeginCountDown()
    {
        beginCountDown = true;
        lobbyTimer = 5f;
    }
}
