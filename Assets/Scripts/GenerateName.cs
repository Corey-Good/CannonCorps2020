/************************************************************************/
/* Author:  */
/* Date Created: */
/* Last Modified Date: */
/* Modified By: */
/************************************************************************/

using Photon.Pun;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenerateName : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    private Player player;

    #region list of names
    string[]
        nameAdj = new string[]
        {
            "Great ", "Sneaky ", "Tenacious ", "Notorious ", "Slippery ", "Bodacious ", "THE ", "The Only ", "Ambitious ", "Courageous ", "Gregarious ", "Practical ", "Witty ", ""
        },
        nameTitle = new string[]
        {
            "Sir ", "Knight ", "Honourable ", "Admiral ", "Professor ", "Private ", "Specialist ", "Corporal ", "Sergeant ", "Major ", "Captain ", ""
        },
        nameBody = new string[]
        {
            "Lee", "Grant", "Sherman", "Abrams", "Bradley", "Patton", "Chaffee", "Jackson", "Pershing"
        };
    List<string> allNames = new List<string>();
    #endregion list of names

    private void Awake()
    {
        GenerateAllNames();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        if(player.PlayerName != null)
            playerName.text = player.PlayerName;
    }

    public void randomName()
    {

        var random_number = new System.Random();
        string random_name = allNames[random_number.Next(0, allNames.Count)];
        allNames.Remove(random_name);

        playerName.text = random_name;
        player.PlayerName = playerName.text;
        PhotonNetwork.NickName = playerName.text;
    }

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
