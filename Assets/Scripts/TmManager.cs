using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TmManager : MonoBehaviour
{
    #region Classes
    private Tank tank;
    private Player player;
    #endregion

    #region Spawn Locations
    public GameObject[] redSpawnlocations = new GameObject[5];
    public GameObject[] blueSpawnlocations = new GameObject[5];
    #endregion

    #region Team Scores
    private ExitGames.Client.Photon.Hashtable teamScores = new ExitGames.Client.Photon.Hashtable();
    private int RedScore = 0;
    private int BlueScore = 0;
    #endregion

    #region Variables
    private GameObject tankObject;
    private PhotonView tankPhotonView;
    public RectTransform panel;
    bool firstCall = true;
    #endregion

    void Awake()
    {
        // Get access to the tank and player class
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();

        // Load the UI scene on top of the curremt scene
        SceneManager.LoadScene(1, LoadSceneMode.Additive);

        // Initialize the scores for both teams
        teamScores.Add("RedScore", RedScore);
        teamScores.Add("BlueScore", BlueScore);
        PhotonNetwork.CurrentRoom.SetCustomProperties(teamScores);

        // Spawn the player at a random location 
        player.teamCode = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
        SpawnPlayer();
        if (player.teamCode == 0)
        {
            tankPhotonView.RPC("ChangeColor_RPC", RpcTarget.AllBuffered, 0, tank.tankModel);

        }
        else if (player.teamCode == 1)
        {
            tankPhotonView.RPC("ChangeColor_RPC", RpcTarget.AllBuffered, 1, tank.tankModel);
        }
    }

    void Start()
    {
        LeanTween.alpha(panel, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        // Respawn the player when they die
        if (tank.healthCurrent < 0.1f)
        {
            RespawnPlayer();
        }

        // End the game when one of the team reaches 100 points
        if (((int)PhotonNetwork.CurrentRoom.CustomProperties["RedScore"] >= 100 ||
             (int)PhotonNetwork.CurrentRoom.CustomProperties["BlueScore"] >= 100) &&
                  firstCall)
        {
            player.leaveGame = true;
            firstCall = false;
        }

        if(player.gotPoints)
        {
            player.gotPoints = false;
            UpdateTeamScores(player.teamCode, 10);
        }

        if (MapNet.FixTankPosition)
        {
            tank.healthCurrent = 0f;
            MapNet.FixTankPosition = false;
        }

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    player.ScoreCurrent += 10;
        //}
    }

    // Grab a random spawn point code


    // Spawn the player at a random spawnpoint in the map
    void SpawnPlayer()
    {
        tank.healthCurrent = tank.healthMax;

        int redCount = 0;
        int blueCount = 0;
        foreach (Photon.Realtime.Player networkPlayer in PhotonNetwork.PlayerList)
        {
            if (networkPlayer == PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.LocalPlayer.ActorNumber))
            {
                if(player.teamCode == 0)
                {
                    tankObject = PhotonNetwork.Instantiate(tank.tankModel, redSpawnlocations[redCount].transform.position, redSpawnlocations[redCount].transform.rotation);
                    redCount++;
                }
                else if(player.teamCode == 1)
                {
                    tankObject = PhotonNetwork.Instantiate(tank.tankModel, blueSpawnlocations[blueCount].transform.position, blueSpawnlocations[blueCount].transform.rotation);
                    blueCount++;
                }                
            }            
        }
        tankPhotonView = tankObject.GetComponent<PhotonView>();
    }

    // Move the player to a random location in the map
    void RespawnPlayer()
    {
        int spawnPoint;
        tank.healthCurrent = tank.healthMax;
        if (player.teamCode == 0)
        {
            spawnPoint = Random.Range(0, redSpawnlocations.Length - 1);
            tankObject.transform.position = redSpawnlocations[spawnPoint].transform.position;
            tankObject.transform.rotation = redSpawnlocations[spawnPoint].transform.rotation;

        }
        else if (player.teamCode == 1)
        {
            spawnPoint = Random.Range(0, blueSpawnlocations.Length - 1);
            tankObject.transform.position = blueSpawnlocations[spawnPoint].transform.position;
            tankObject.transform.rotation = blueSpawnlocations[spawnPoint].transform.rotation;
        }
        
    }

    public void UpdateTeamScores(int teamCode, int pointsEarned)
    {
        // Get the current score for both teams
        RedScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["RedScore"];
        BlueScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["BlueScore"];

        // Increment the score based on the team
        if (teamCode == 0)
        {
            RedScore += pointsEarned;
        }
        else if (teamCode == 1)
        {
            BlueScore += pointsEarned;
        }

        // Create a new Hashtable and add the new scores
        ExitGames.Client.Photon.Hashtable newScores = new ExitGames.Client.Photon.Hashtable();
        newScores.Add("RedScore", RedScore);
        newScores.Add("BlueScore", BlueScore);

        // Set the new custom properties for the room
        PhotonNetwork.CurrentRoom.SetCustomProperties(newScores);
    }
}
