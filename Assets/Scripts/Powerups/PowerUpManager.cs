using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerUpManager : MonoBehaviour, IPowerUpManagerEvents
{
    public GameObject[] powerupRotations = new GameObject[numberOfPowerups];
    public GameObject[] spawnLocations = new GameObject[numberOfPowerupSpawnLocations];
    private string[] powerupNames = new string[] { "FreezeBullets", "DynamiteBullets", "LaserBullets", "HealthPowerUp", "ShieldPowerUp", "ReloadPowerUp", "SpeedPowerUp"};
    private static int numberOfPowerups;
    private static int numberOfPowerupSpawnLocations = 7;
    private float time = 0.0f;

    public void Start()
    {
        numberOfPowerups = powerupNames.Length;
        
        EventSystemListeners.main.AddListener(this.gameObject);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SendSpawnPowerUpMessage();
        }
    }

    private void SendSpawnPowerUpMessage()
    {
        // Send message to any listeners
        if (EventSystemListeners.main.listeners != null)
        {
            foreach (GameObject go in EventSystemListeners.main.listeners)  // 1
            {
                ExecuteEvents.Execute<IPowerUpManagerEvents>                   // 2
                    (go, null,                                               // 3
                     (x, y) => x.OnSpawnPowerup()            // 4
                    );
            }
        }
    }

    private int count = 0;
    private void SpawnRandomPowerUp()
    {
        //int powerUpRandomNumber = Random.Range(0, numberOfPowerups);
        //int locationRandomNumber = Random.Range(0, numberOfPowerupSpawnLocations);
        //Debug.Log(powerupNames[powerUpRandomNumber]);
        //Debug.Log(locationRandomNumber);

        int powerUpRandomNumber = count;
        int locationRandomNumber = count;
        count++;
        if (count == 7)
            count = 0;

        PhotonNetwork.Instantiate(powerupNames[powerUpRandomNumber], new Vector3(spawnLocations[locationRandomNumber].transform.position.x, spawnLocations[locationRandomNumber].transform.position.y + 1, spawnLocations[locationRandomNumber].transform.position.z), powerupRotations[powerUpRandomNumber].transform.rotation);
    }

    void IPowerUpManagerEvents.OnSpawnPowerup()
    {
        SpawnRandomPowerUp();
    }
}
