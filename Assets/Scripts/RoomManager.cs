using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public delegate void EnterGame(string gameMode);
    public static event EnterGame OnEnterGame;
    public Text playerCount;
    public Text MinPlayerNote;
    public GameObject LobbyView;
    public GameObject PlayerNames;
    public GameObject playerListingPrefab;
    private Text roomName;
    private int roomCountFFA = 0;
    private int roomCountSM = 0;
    private int roomCountTB = 0;
    private int roomCountTT = 0;
    private List<GameObject> playerListings = new List<GameObject>();
    private GameMode currentGameMode;
    enum  GameMode
    {
        FreeForAll, 
        SharksAndMinnows, 
        TeamBattle,
        Tutorial,
        Lobby
    }

    private void Awake()
    {
        currentGameMode = GameMode.Lobby;
    }

    public override void OnJoinedRoom()
    {
        //connectionStatus.text = "A new player has joined";
        UpdatePlayerList();
        if(currentGameMode == GameMode.FreeForAll)
        {
            LoadFreeForAll();
        }
        if (currentGameMode == GameMode.Tutorial)
        {
            LoadTutorial();
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        //connectionStatus.text = "Failed to create a room";
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //connectionStatus.text = "A Player has the left the room";
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //connectionStatus.text = "A new player has joined";
        UpdatePlayerList();
    }

    public void FreeForAllButtonOnClick()
    {
        if (OnEnterGame != null)
        {
            OnEnterGame("FFA");
        }
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 20 };
        PhotonNetwork.JoinOrCreateRoom("FreeForAll " + roomCountFFA, roomOps, null);
        currentGameMode = GameMode.FreeForAll;

    }

    public void SharksMinnowsButtonOnClick()
    {
        if (OnEnterGame != null)
        {
            OnEnterGame("SM");
        }
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 15 };
        PhotonNetwork.JoinOrCreateRoom("SharksAndMinnows " + roomCountSM, roomOps, null);
        currentGameMode = GameMode.SharksAndMinnows;
        OpenLobbyView();
    }

    public void TeamBattleButtonOnClick()
    {
        if (OnEnterGame != null)
        {
            OnEnterGame("TB");
        }
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.JoinOrCreateRoom("TeamBattle " + roomCountTB, roomOps, null);
        currentGameMode = GameMode.TeamBattle;
        OpenLobbyView();
    }

    public void TutorialButtonOnClick()
    {
        if (OnEnterGame != null)
        {
            OnEnterGame("TT");
        }
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 1 };
        PhotonNetwork.JoinOrCreateRoom("Tutorial " + roomCountTT, roomOps, null);
        currentGameMode = GameMode.Tutorial;

    }

    public void LoadFreeForAll()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void LoadTutorial()
    {
        PhotonNetwork.LoadLevel(3);
    }

    public void OpenLobbyView()
    {
        //GameModeView.SetActive(false);
        LobbyView.SetActive(true);
        MinPlayerNote.text = "*Note, a game needs at least 8 players to begin.";
    }

    void UpdatePlayerList()
    {
        int count = 0;
        playerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        playerCount.text += "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();

        // Remove the currently listed players
        if (playerListings != null)
        {
            foreach (GameObject listing in playerListings)
            {
                Destroy(listing);
                Debug.Log("Destroying a player listing");
            }
            playerListings.Clear();
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

        if(PhotonNetwork.CurrentRoom.PlayerCount > 2)
        {
            LoadGame();
        }
    }

    void LoadGame()
    {
        if(currentGameMode == GameMode.SharksAndMinnows)
        {
            // Close the room, increase room counter for other rooms

            // Load the game 

            //connectionStatus.text = "Loading Sharks and Minnows!";
        }
        else if (currentGameMode == GameMode.TeamBattle)
        {
            // Close the room, increase room counter for other rooms

            // Load the game 

            //connectionStatus.text = "Loading Team Battle!";
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        LobbyView.SetActive(false);
        //GameModeView.SetActive(true);

        // Remove the currently listed players
        if (playerListings != null)
        {
            foreach (GameObject listing in playerListings)
            {
                Destroy(listing);
                Debug.Log("Destroying a player listing");
            }
            playerListings.Clear();
        }
    }
}
