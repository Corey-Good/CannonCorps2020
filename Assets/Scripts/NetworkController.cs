/************************************************************************/
/* Author:             CannonCorps                                      */
/* Date Created:       1/27/2020                                        */
/* Last Modified Date: 2/26/2020                                        */
/* Modified By:        M. Agamalian                                     */
/************************************************************************/

using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* This controller is used mostly for debug puposes and will remain 
   commented out most of the time                                       */
public class NetworkController : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public Text connectionStatus;

    // Attempt to connect to the main server useing preset settings
    private void Update()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();

            //connectionStatus.text = "Connecting . . .";
        }
    }

    // Alert that connection has been succesful
    public override void OnConnectedToMaster()
    {
        //connectionStatus.text = "Now connected!!";
    }


}


        
        

