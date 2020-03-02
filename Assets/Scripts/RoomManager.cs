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

public class RoomManager : MonoBehaviourPunCallbacks
{
    #region Classes
    private Player playerInstance;
    private Tank tank;
    #endregion

    #region Lobby Info
    public Text playerCount;
    public Text MinPlayerNote;
    public GameObject LobbyView;
    public GameObject PlayerNames;
    public GameObject playerListingPrefab;
    private List<GameObject> playerListings = new List<GameObject>();
    #endregion

    #region Room Counts
    private int roomCountFFA = 0;
    private int roomCountSM = 0;
    private int roomCountTB = 0;
    private int roomCountTT = 0;
    #endregion

    #region Variables
    int previousTeam;
    public RectTransform panel;
    public Button startGameButton;
    #endregion

    private void Awake()
    {
        playerInstance = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["gameStart"])
            {
                Debug.Log("Loading the Game");
                LoadGame();
            }

        }
    }

    public override void OnJoinedRoom()
    {
        ExitGames.Client.Photon.Hashtable playerScore = new ExitGames.Client.Photon.Hashtable() { { "Score", playerInstance.ScoreCurrent } };
        PhotonNetwork.SetPlayerCustomProperties(playerScore);

        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "gameStart", false } });

        UpdatePlayerList();
        TryToStartGame();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerList();
        TryToStartGame();
    }

    public void FreeForAllButtonOnClick()
    {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 25 };
        PhotonNetwork.JoinOrCreateRoom("FreeForAll " + roomCountFFA, roomOps, null);
        playerInstance.gameState = Player.GameState.FFA;
        playerInstance.teamCode = 3;
    }

    public void SharksMinnowsButtonOnClick()
    {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 15 };
        PhotonNetwork.JoinOrCreateRoom("SharksAndMinnows " + roomCountSM, roomOps, null);
        playerInstance.gameState = Player.GameState.SM;
        playerInstance.teamCode = 3;
        OpenLobbyView();
    }

    public void TeamBattleButtonOnClick()
    {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.JoinOrCreateRoom("TeamBattle " + roomCountTB, roomOps, null);
        playerInstance.gameState = Player.GameState.TB;
        OpenLobbyView();
    }

    public void TutorialButtonOnClick()
    {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 1  };
        PhotonNetwork.JoinOrCreateRoom("Tutorial " + roomCountTT, roomOps, null);
        playerInstance.gameState = Player.GameState.TT;
        //OpenLobbyView();
    }
    
    public void OpenLobbyView()
    {
        LobbyView.SetActive(true);
        MinPlayerNote.text = "*Note, a game needs at least 8 players to begin.";
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

    public void LoadGame()
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "gameStart", false } });
        if (playerInstance.gameState == Player.GameState.FFA)
        {
            StartCoroutine(TransitionScene(2));
        }
        if (playerInstance.gameState == Player.GameState.SM)
        {
            // Set the Shark to the Master Client
            if(PhotonNetwork.IsMasterClient)
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

            // Close the current room and increment the counter
            PhotonNetwork.CurrentRoom.IsOpen = false;
            roomCountSM++;

            // Load the scene
            StartCoroutine(TransitionScene(3));
        }
        else if (playerInstance.gameState == Player.GameState.TB)
        {
            AssignTeamCodes();
            PhotonNetwork.CurrentRoom.IsOpen = false;
            roomCountTB++;
            StartCoroutine(TransitionScene(4));
        }

        if(playerInstance.gameState == Player.GameState.TT)
        {
            StartCoroutine(TransitionScene(5));
        }
    }

    public void LeaveRoom()
    {
        playerInstance.gameState = Player.GameState.Lobby;
        PhotonNetwork.LeaveRoom();
        LobbyView.SetActive(false);
        EmptyPlayerList();
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

    private void TryToStartGame()
    {
        switch (playerInstance.gameState)
        {
            case Player.GameState.FFA:
                LoadGame();
                break;

            case Player.GameState.SM:
                if (PhotonNetwork.CurrentRoom.PlayerCount >=  1/*PhotonNetwork.CurrentRoom.MaxPlayers - 5*/)
                {
                    startGameButton.interactable = true;
                }
                break;

            case Player.GameState.TB:
                if (PhotonNetwork.CurrentRoom.PlayerCount >= 1/*PhotonNetwork.CurrentRoom.MaxPlayers - 2*/)
                {
                    startGameButton.interactable = true;
                }
                break;
            case Player.GameState.TT:
                LoadGame();
                break;
        }
    }

    private void AssignTeamCodes()
    {
        Photon.Realtime.Player[] networkPlayers = PhotonNetwork.PlayerList;
        int count = 1;
        int totalPlayers = networkPlayers.Length;

        foreach (Photon.Realtime.Player player in networkPlayers)
        {
            if (count <= (int)totalPlayers / 2)
            {
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { {"team", 0 } } );
            }
            else
            {
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "team", 1 } });
            }

            count++;
        }
    }

    public void OnClickStart()
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "gameStart", true } });
    }

    private IEnumerator TransitionScene(int scene)
    {
        LeanTween.alpha(panel, 1, 1);
        yield return new WaitForSeconds(1f);
        PhotonNetwork.LoadLevel(scene);
    }
      
}