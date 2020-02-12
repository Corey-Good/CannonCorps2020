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

    // Start is called before the first frame update
    void Awake()
    {
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
    }

    void Start()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        PhotonNetwork.Instantiate(tank.tankModel, spawnlocations[0].transform.position, spawnlocations[0].transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (tank.healthCurrent < 0)
        {
            Debug.Log(" ");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            tank.damageTaken(10f);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            player.ScoreCurrent += 10;
        }
    }

    private void OnDisable()
    {
        
    }
}
