/************************************************************************/
/* Author:  */
/* Date Created: */
/* Last Modified Date: */
/* Modified By: */
/************************************************************************/

using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Text playerCount;
    public Text MinPlayerNote;
    public GameObject LobbyView;
    public GameObject PlayerNames;
    public GameObject playerListingPrefab;
    private int roomCountFFA = 0;
    private int roomCountSM = 0;
    private int roomCountTB = 0;
    private List<GameObject> playerListings = new List<GameObject>();
    private Player playerInstance;

    private void Awake()
    {
        playerInstance = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        playerInstance.gameState = Player.GameState.Lobby;
    }

    public override void OnJoinedRoom()
    {
        ExitGames.Client.Photon.Hashtable playerScore = new ExitGames.Client.Photon.Hashtable() { { "Score", playerInstance.ScoreCurrent } };
        PhotonNetwork.SetPlayerCustomProperties(playerScore);
        UpdatePlayerList();

        switch (playerInstance.gameState)
        {
            case Player.GameState.FFA:
                LoadFreeForAll();
                break;

            case Player.GameState.SM:
            case Player.GameState.TB:
                if (PhotonNetwork.CurrentRoom.PlayerCount > PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    LoadGame();
                }
                break;
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerList();
    }

    public void FreeForAllButtonOnClick()
    {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 20 };
        PhotonNetwork.JoinOrCreateRoom("FreeForAll " + roomCountFFA, roomOps, null);
        playerInstance.gameState = Player.GameState.FFA;
    }

    public void SharksMinnowsButtonOnClick()
    {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 15 };
        PhotonNetwork.JoinOrCreateRoom("SharksAndMinnows " + roomCountSM, roomOps, null);
        playerInstance.gameState = Player.GameState.SM;
        OpenLobbyView();
    }

    public void TeamBattleButtonOnClick()
    {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.JoinOrCreateRoom("TeamBattle " + roomCountTB, roomOps, null);
        playerInstance.gameState = Player.GameState.TB;
        OpenLobbyView();
    }

    public void LoadFreeForAll()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void OpenLobbyView()
    {
        //GameModeView.SetActive(false);
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

    private void LoadFreeForAll()
    {
        PhotonNetwork.LoadLevel(1);
        roomCountFFA++;
    }

    private void LoadGame()
    {
        if (playerInstance.gameState == Player.GameState.SM)
        {
            // Close the room, increase room counter for other rooms

            // Load the game

            //connectionStatus.text = "Loading Sharks and Minnows!";
            roomCountSM++;
        }
        else if (playerInstance.gameState == Player.GameState.TB)
        {
            // Close the room, increase room counter for other rooms

            // Load the game

            //connectionStatus.text = "Loading Team Battle!";
            roomCountTB++;
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
}