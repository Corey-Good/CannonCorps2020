using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePoints : MonoBehaviour
{
    Player playerInfo;

    // Start is called before the first frame update
    void Start()
    {
        GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            playerInfo.GivePoints(10);            
        }

    }

    void GetPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("PlayerClass");

        playerInfo = player.GetComponent<Player>();
    }
}
