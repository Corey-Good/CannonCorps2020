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

    private GameObject tankObject;

    void Awake()
    {
        // Get access to the tank and player class
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();

        // Load the UI scene on top of the curremt scene
        SceneManager.LoadScene(1, LoadSceneMode.Additive);

        // Spawn the player at a random location
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

        // End the game when the timer has run out
        if (UIManager.matchTimer >= 300.0)
        {
            StartCoroutine(DisconnectAndLoad());
            UIManager.matchTimer = 0;
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
        SceneManager.UnloadSceneAsync(1);
        PhotonNetwork.LoadLevel(0);
    }

    // Spawn the player at a random spawnpoint in the map
    void SpawnPlayer()
    {
        tank.healthCurrent = tank.healthMax;
        int spawnPoint = Random.Range(0, spawnlocations.Length - 1);
        tankObject = PhotonNetwork.Instantiate(tank.tankModel, spawnlocations[spawnPoint].transform.position, spawnlocations[spawnPoint].transform.rotation);
    }

    // Move the player to a random location in the map
    void RespawnPlayer()
    {
        tank.healthCurrent = tank.healthMax * 0.1f;
        ChangeColor(Color.red);
        int spawnPoint = Random.Range(0, spawnlocations.Length - 1);
        tankObject.transform.position = spawnlocations[spawnPoint].transform.position;
    }

    void ChangeColor(Color tankColor)
    {
        if (tank.tankModel == "baseTank")
        {
            Renderer[] rends = tankObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                Material[] materials = rend.materials;
                materials[0].color = tankColor;
            }
        }
    }
}
