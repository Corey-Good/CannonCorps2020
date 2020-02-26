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

    void Awake()
    {
        // Get access to the tank and player class
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();

        // Load the UI scene on top of the curremt scene
        SceneManager.LoadScene(1, LoadSceneMode.Additive);

        //Spawn the player at a random location
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        // Leave the game when the player dies
        if (tank.healthCurrent < 0.1f)
        {
            StartCoroutine(DisconnectAndLoad());
        }
    }

    // Spawn the player at a random spawnpoint in the map
    void SpawnPlayer()
    {
        tank.healthCurrent = tank.healthMax;
        int spawnPoint = Random.Range(0, spawnlocations.Length - 1);
        PhotonNetwork.Instantiate(tank.tankModel, spawnlocations[spawnPoint].transform.position, spawnlocations[spawnPoint].transform.rotation);
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
        SceneManager.UnloadSceneAsync(1);
        PhotonNetwork.LoadLevel(0);
    }
}
