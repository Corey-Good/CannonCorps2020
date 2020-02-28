/************************************************************************/
/* Author:             CannonCorps                                      */
/* Date Created:       1/27/2020                                        */
/* Last Modified Date: 2/26/2020                                        */
/* Modified By:        M. Agamalian                                     */
/************************************************************************/

using Photon.Pun;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenerateName : MonoBehaviour
{
    public  TextMeshProUGUI playerName;
    private Player          player;

    #region list of names
    string[]
        nameAdj = new string[]
        {
            "Great ", "Sneaky ", "Tenacious ", "Notorious ", "Slippery ", "Bodacious ", "THE ", "The Only ", "Ambitious ", "Courageous ", "Gregarious ", "Practical ", "Witty ", ""
        },
        nameTitle = new string[]
        {
            "Sir ", "Knight ", "Honourable ", "Admiral ", "General ", "Private ", "Specialist ", "Corporal ", "Sergeant ", "Major ", "Captain ", ""
        },
        nameBody = new string[]
        {
            "Lee", "Grant", "Sherman", "Abrams", "Bradley", "Patton", "Chaffee", "Jackson", "Pershing"
        };
    List<string> allNames = new List<string>();
    #endregion

    // Assign a player name to the player
    private void Awake()
    {
        GenerateAllNames();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();

        if(player.PlayerName != null)
            playerName.text = player.PlayerName;
    }

    // Assign a random name to the player
    public void randomName()
    {
        var    random_number = new System.Random();
        string random_name   = allNames[random_number.Next(0, allNames.Count)];

        // prevent name from being reused
        allNames.Remove(random_name);

        // assign the name to all aspects of the player
        playerName.text        = random_name;
        player.PlayerName      = playerName.text;
        PhotonNetwork.NickName = playerName.text;
    }


    // Assemble all possible combinations of the names into a list
    public void GenerateAllNames()
    {
        for (int i = 0; i <= nameAdj.Length - 1; i++)
        {
            for (int j = 0; j <= nameTitle.Length - 1; j++)
            {
                for (int k = 0; k <= nameBody.Length - 1; k++)
                {
                    allNames.Add(nameAdj[i] + nameTitle[j] + nameBody[k]);
                }
            }
        }
    }
}