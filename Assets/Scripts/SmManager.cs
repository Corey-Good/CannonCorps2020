using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmManager : MonoBehaviour
{
    #region Classes
    private Tank tank;
    private Player player;
    #endregion

    #region Spawn Locations
    public GameObject[] spawnlocations = new GameObject[5];
    #endregion

    #region Variables
    private GameObject    tankObject;
    private PhotonView    tankPhotonView;
    public  RectTransform panel;
            int           deathCount = 0;
            bool          firstCall = true;
    #endregion

    void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        // Get access to the tank and player class
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();

        // Load the UI scene on top of the curremt scene
        SceneManager.LoadScene(1, LoadSceneMode.Additive);

        // Spawn the player at a random location
        SpawnPlayer();

        // Set the room property MinnowCount to the all of the player except the shark
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "SharkCount", 1} });
    }

    void Start()
    {
        LeanTween.alpha(panel, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (((int)PhotonNetwork.CurrentRoom.CustomProperties["SharkCount"] >= PhotonNetwork.CurrentRoom.PlayerCount ||
            UIManager.matchTimer >= 300.0) && firstCall)
        {
            // Triggers the leave function in UIManager
            player.leaveGame = true;
            UIManager.matchTimer = 0;
            firstCall = false;
        }

        // Respawn the player when they die
        if (tank.healthCurrent < 0.1f)
        {
            deathCount++;
            RespawnPlayer();            
        }

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    player.ScoreCurrent += 10;
        //}

        // If a shark leaves the game for some reason, decrease the shark count
        if (player.leaveGame && deathCount > 1)
        {
            int sharkCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["SharkCount"];
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "SharkCount", sharkCount - 1 } });
        }
    }

    // Spawn the player at a random spawnpoint in the map
    void SpawnPlayer()
    {
        if (tank.tankModel == "baseTank")
        {
            tank.healthCurrent = tank.healthMax * 0.2f;
        }
        else
        {
            tank.healthCurrent = tank.healthMax;
        }

        int count = 0;
        foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if(player == PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.LocalPlayer.ActorNumber))
            {
                tankObject = PhotonNetwork.Instantiate(tank.tankModel, spawnlocations[count].transform.position, spawnlocations[count].transform.rotation);
            }

            count++;
        }        
        
        tankPhotonView = tankObject.GetComponent<PhotonView>();
    }

    // Move the player to a random location in the map
    void RespawnPlayer()
    {
        if (tank.tankModel == "baseTank")
        {
            tank.healthCurrent = tank.healthMax * 0.2f;
            tankPhotonView.RPC("ChangeColor_RPC", RpcTarget.AllBuffered);
        }
        else
        {
            tank.healthCurrent = tank.healthMax;
        }        
        int spawnPoint = Random.Range(0, spawnlocations.Length - 1);
        tankObject.transform.position = spawnlocations[spawnPoint].transform.position;        

        if(deathCount == 1 && !PhotonNetwork.IsMasterClient)
        {
            int sharkCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["SharkCount"];
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "SharkCount", sharkCount + 1 } });
        }
    }
}
