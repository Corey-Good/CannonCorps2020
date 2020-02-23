using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamemodeManager : MonoBehaviour
{
    private Tank tank;
    private Player player;

    public GameObject[] spawnlocations = new GameObject[5];

    private ExitGames.Client.Photon.Hashtable teamScores = new ExitGames.Client.Photon.Hashtable();
    private int RedScore = 0;
    private int BlueScore = 0;


    void Awake()
    {
        // Get access to the tank and player class
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();

        // Load the UI scene on top of the curremt scene
        SceneManager.LoadScene(2, LoadSceneMode.Additive);

        // Spawn the player at a random location 
        if (player.gameState == Player.GameState.TB)
        {
            SpawnPlayer(player.teamCode);
        }
        else
        {
            SpawnPlayer();
        }

        teamScores.Add("RedScore", RedScore);
        teamScores.Add("BlueScore", BlueScore);
        PhotonNetwork.CurrentRoom.SetCustomProperties(teamScores);
    }

    // Update is called once per frame
    void Update()
    {
        // Call the update function for the respective game mode
        switch (player.gameState)
        {
            case Player.GameState.FFA:
                FFA_Update();
                break;
            case Player.GameState.SM:
                SM_Update();
                break;
            case Player.GameState.TB:
                TB_Update();
                break;
        }


        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateTeamScores();
        }

    }

    // Update for the Free for All game mode
    void FFA_Update()
    {
        // Leave the game when the player dies
        if (tank.healthCurrent < 0.1f)
        {
            StartCoroutine(DisconnectAndLoad());
        }
    }

    // Update for the Sharks and Minnows game mode
    void SM_Update()
    {
        // Respawn the player when they die
        if (tank.healthCurrent < 0.1f)
        {
            SpawnPlayer();
        }

        // End the game when the timer has run out
        if (UIManager.matchTimer >= 300.0)
        {
            StartCoroutine(DisconnectAndLoad());
            UIManager.matchTimer = 0;
        }
    }

    // Update for the Team Battle game mode
    void TB_Update()
    {
        // Respawn the player when they die
        if (tank.healthCurrent < 0.1f)
        {
            SpawnPlayer(player.teamCode);
        }

        // End the game when the timer has run out
        if (UIManager.matchTimer >= 300.0f)
        {
            StartCoroutine(DisconnectAndLoad());
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

    // Spawn the player at a random spawnpoint in the map
    void SpawnPlayer()
    {
        tank.healthCurrent = tank.healthMax;
        int spawnPoint = Random.Range(0, spawnlocations.Length - 1);
        PhotonNetwork.Instantiate(tank.tankModel, spawnlocations[spawnPoint].transform.position, spawnlocations[spawnPoint].transform.rotation);
    }

    // Overload: Spawn a player at a random spawnpoint in the map based on their team
    void SpawnPlayer(int teamCode)
    {
        tank.healthCurrent = tank.healthMax;
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

        PhotonNetwork.Instantiate(tank.tankModel, spawnlocations[spawnPoint].transform.position, spawnlocations[spawnPoint].transform.rotation);
    }

    void UpdateTeamScores()
    {
        ExitGames.Client.Photon.Hashtable newScores = new ExitGames.Client.Photon.Hashtable();
        RedScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["RedScore"];
        BlueScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["BlueScore"];
        RedScore += 5;

        newScores.Add("RedScore", RedScore);
        newScores.Add("BlueScore", BlueScore);

        PhotonNetwork.CurrentRoom.SetCustomProperties(newScores);
    }
}
