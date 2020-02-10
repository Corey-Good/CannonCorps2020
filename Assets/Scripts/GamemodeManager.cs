using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamemodeManager : MonoBehaviour
{
    private Tank tank;
    private Player player;

    // Start is called before the first frame update
    void Awake()
    {
        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
    }

    void Start()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        if (tank.healthCurrent < 0)
        {
            StartCoroutine(DisconnectAndLoad());
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

    private IEnumerator DisconnectAndLoad()
    {
        SceneManager.UnloadSceneAsync(2);
        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        Cursor.lockState = CursorLockMode.None;        
        PhotonNetwork.LoadLevel(0);
    }
}
