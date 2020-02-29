using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    #region Classes
    private Tank tank;
    private Player player;
    #endregion

    #region Spawn Locations
    public GameObject[] spawnlocations = new GameObject[1];
    #endregion
    private PhotonView tankPhotonView;
    public RectTransform panel;

    void Awake()
    {
        // Get access to the tank and player class
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();

        // Load the UI scene on top of the curremt scene
        SceneManager.LoadScene(1, LoadSceneMode.Additive);

        //Spawn the player at a random location
        SpawnPlayer();

        tankPhotonView.RPC("ChangeColor_RPC", RpcTarget.AllBuffered, tank.tankModel, tank.tankColor.r, tank.tankColor.g, tank.tankColor.b);
    }
    void Start()
    {
        LeanTween.alpha(panel, 0, 1);
    }

    // Spawn the player at a random spawnpoint in the map
    void SpawnPlayer()
    {
        tank.healthCurrent = tank.healthMax;
        int spawnPoint = Random.Range(0, spawnlocations.Length - 1);
        GameObject tankObject = PhotonNetwork.Instantiate(tank.tankModel, spawnlocations[spawnPoint].transform.position, spawnlocations[spawnPoint].transform.rotation);
        tankPhotonView = tankObject.GetComponent<PhotonView>();
    }
}
