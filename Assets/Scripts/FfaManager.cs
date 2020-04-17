using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FfaManager : MonoBehaviour
{
    #region Classes
    private Tank tank;
    private Player player;
    #endregion

    #region Spawn Locations
    public GameObject[] spawnlocations = new GameObject[5];
    #endregion

    #region Variables
    private PhotonView tankPhotonView;
    public RectTransform panel;
    bool firstCall = true;
    GameObject tankObject;
    #endregion

    void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        // Get access to the tank and player class
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();

        player.leaveGame = false;
        //Spawn the player at a random location
        SpawnPlayer();

        // Load the UI scene on top of the curremt scene
        SceneManager.LoadScene(1, LoadSceneMode.Additive);

        tankPhotonView.RPC("ChangeColor_RPC", RpcTarget.AllBuffered, tank.tankModel, tank.tankColor.r, tank.tankColor.g, tank.tankColor.b);

        tank.healthCurrent = tank.healthMax;

    }
    void Start()
    {
        LeanTween.alpha(panel, 0, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(player.leaveGame);
        // Leave the game when the player dies
        if (tank.healthCurrent < 0.1f && firstCall)
        {
            // Triggers the leave function in UIManager
            player.leaveGame = true;
            firstCall = false;
        }

        if(MapNet.FixTankPosition)
        {
            MapNet.FixTankPosition = false;
            player.fellThroughMap = true;
            tank.healthCurrent = 0f;
            
        }
    }

    // Spawn the player at a random spawnpoint in the map
    void SpawnPlayer()
    {
        tank.healthCurrent = tank.healthMax;
        int spawnPoint = Random.Range(0, spawnlocations.Length - 1);
        tankObject = PhotonNetwork.Instantiate(tank.tankModel, 
            spawnlocations[spawnPoint].transform.position, 
            spawnlocations[spawnPoint].transform.rotation);
        tankPhotonView = tankObject.GetComponent<PhotonView>();
    }
}
