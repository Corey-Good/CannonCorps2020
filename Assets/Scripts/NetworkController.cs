using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkController : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public Text connectionStatus;

    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();

            //connectionStatus.text = "Connecting . . .";
        }
    }

    public override void OnConnectedToMaster()
    {
        //connectionStatus.text = "Now connected!!";
        PhotonNetwork.NickName = "John Smith";
    }

}


        
        

