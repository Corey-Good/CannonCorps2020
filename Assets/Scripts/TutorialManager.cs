/************************************************************************/
/* Author:  */
/* Date Created: */
/* Last Modified Date: */
/* Modified By: */
/************************************************************************/

using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    #region Classes

    private Tank tank;
    private Player player;

    #endregion Classes

    #region Spawn Locations

    public GameObject[] spawnlocations = new GameObject[1];

    #endregion Spawn Locations

    private PhotonView tankPhotonView;
    public RectTransform panel;

    private void Awake()
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

    private void Start()
    {
        LeanTween.alpha(panel, 0, 1);
    }

    // Spawn the player at a random spawnpoint in the map
    private void SpawnPlayer()
    {
        tank.healthCurrent = tank.healthMax;
        int spawnPoint = Random.Range(0, spawnlocations.Length - 1);
        GameObject tankObject = PhotonNetwork.Instantiate(tank.tankModel, spawnlocations[spawnPoint].transform.position, spawnlocations[spawnPoint].transform.rotation);
        tankPhotonView = tankObject.GetComponent<PhotonView>();
    }
}