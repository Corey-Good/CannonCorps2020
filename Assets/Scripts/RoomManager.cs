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

public class RoomManager : MonoBehaviourPunCallbacks
{
    #region Classes
    private Player player;
    #endregion

    #region Room Counts
    private int roomCountFFA = 0;
    private int roomCountSM = 0;
    private int roomCountTB = 0;
    private int roomCountTT = 0;
    #endregion

    public GameObject LobbyView;
    public RectTransform transitionPanel;
    public TextMeshProUGUI ffaButtonText;
    public TextMeshProUGUI ttButtonText;

    private void Awake()
    {
        Cursor.visible = true;
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
    }

    public void FreeForAllButtonOnClick()
    {
        player.gameState = Player.GameState.FFA;
        ffaButtonText.text = "Loading...";
        CreateRoom(roomCountFFA, 25);
    }

    public void SharksMinnowsButtonOnClick()
    {
        player.gameState = Player.GameState.SM;
        CreateRoom(roomCountSM, 15);
    }

    public void TeamBattleButtonOnClick()
    {        
        player.gameState = Player.GameState.TB;
        CreateRoom(roomCountTB, 10);
    }

    public void TutorialButtonOnClick()
    {
        player.gameState = Player.GameState.TT;
        ttButtonText.text = "Loading...";
        CreateRoom(roomCountTT, 1);
    }

    void CreateRoom(int roomNumber, int maxPlayers)
    {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)maxPlayers };
        PhotonNetwork.JoinOrCreateRoom(player.gameState.ToString() + roomNumber, roomOps, null);
    }

    public override void OnJoinedRoom()
    {
        if(player.gameState == Player.GameState.FFA)
        {
            PhotonNetwork.IsMessageQueueRunning = false;
            StartCoroutine(TransitionScene(2));
        }
        else if (player.gameState == Player.GameState.SM)
        {
            PhotonNetwork.IsMessageQueueRunning = false;
            LobbyView.SetActive(true);
        }
        else if (player.gameState == Player.GameState.TB)
        {
            PhotonNetwork.IsMessageQueueRunning = false;
            LobbyView.SetActive(true);
        }
        else if(player.gameState == Player.GameState.TT)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            StartCoroutine(TransitionScene(5));
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (player.gameState == Player.GameState.FFA)
        {
            roomCountFFA++;
            FreeForAllButtonOnClick();
        }
        else if (player.gameState == Player.GameState.SM)
        {
            roomCountSM++;
            SharksMinnowsButtonOnClick();
        }
        else if (player.gameState == Player.GameState.TB)
        {
            roomCountTB++;
            TeamBattleButtonOnClick();
        }
        else if (player.gameState == Player.GameState.TT)
        {
            roomCountTT++;
            TutorialButtonOnClick();
        }
    }

    private IEnumerator TransitionScene(int scene)
    {        
        LeanTween.alpha(transitionPanel, 1, 1);
        yield return new WaitForSeconds(1f);
        PhotonNetwork.LoadLevel(scene);
    }
}