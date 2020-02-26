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
    public GameObject[] spawnlocations = new GameObject[5];
    #endregion

    #region Team Scores
    private ExitGames.Client.Photon.Hashtable teamScores = new ExitGames.Client.Photon.Hashtable();
    private int RedScore = 0;
    private int BlueScore = 0;
    #endregion

    void Awake()
    {
        // Get access to the tank and player class
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();

        // Load the UI scene on top of the curremt scene
        SceneManager.LoadScene(1, LoadSceneMode.Additive);

        // Spawn the player at a random location 
        player.teamCode = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
        if (player.teamCode == 0)
        {
            // For now change the model to test setting up the team view
            tank.tankModel = "baseTank";
            tank.CreateTank("baseTank");
        }
        else if (player.teamCode == 1)
        {
            tank.tankModel = "cartoonTank";
            tank.CreateTank("cartoonTank");
        }

        // Initialize the scores for both teams
        teamScores.Add("RedScore", RedScore);
        teamScores.Add("BlueScore", BlueScore);
        PhotonNetwork.CurrentRoom.SetCustomProperties(teamScores);
        

        SpawnPlayer();
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
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["RedScore"] >= 100 ||
            (int)PhotonNetwork.CurrentRoom.CustomProperties["BlueScore"] >= 100)
        {
            StartCoroutine(DisconnectAndLoad());
        }

        // Test the Red Score Bar by hitting U
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateTeamScores(0, 5);
        }
        // Test the Blue Score Bar by hitting I
        if (Input.GetKeyDown(KeyCode.I))
        {
            UpdateTeamScores(1, 5);
        }

    }

    // Leave the game and return to the main menu
    private IEnumerator DisconnectAndLoad()
    {
        player.gameState = Player.GameState.Lobby;
        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.UnloadSceneAsync(2);
        PhotonNetwork.LoadLevel(0);
    }

    // Grab a random spawn point code
    int GetSpawnPoint(int teamCode)
    {
        int spawnPoint = 0;

        if (teamCode == 0)
        {
            // Get a spawnpoint from the first half of the array
            spawnPoint = Random.Range(0, (int)spawnlocations.Length / 2 - 1);
        }
        else if (teamCode == 1)
        {
            // Get a spawnpoint from the second half of the array
            spawnPoint = Random.Range((int)spawnlocations.Length / 2, spawnlocations.Length - 1);
        }

        return spawnPoint;
    }

    // Spawn the player at a random spawnpoint in the map
    void SpawnPlayer()
    {
        tank.healthCurrent = tank.healthMax;
        int spawnPoint = GetSpawnPoint(player.teamCode);
        PhotonNetwork.Instantiate(tank.tankModel, spawnlocations[spawnPoint].transform.position, spawnlocations[spawnPoint].transform.rotation);
    }

    // Move the player to a random location in the map
    void RespawnPlayer()
    {
        tank.healthCurrent = tank.healthMax;
        GameObject.FindGameObjectWithTag("PlayerGO").transform.position = spawnlocations[GetSpawnPoint(player.teamCode)].transform.position;
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
