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
    private Location[] locations = new Location[numberOfPowerupSpawnLocations];
    private static int numberOfPowerups;
    private static int numberOfPowerupSpawnLocations = 7;

    private float time = 0.0f;
    private float waitRandomTimeAmount;
    private float randomFloor = 5.0f;
    private float randomCeiling = 10.0f;
    private bool allLocationsLocked = false;
    private int locationCount = 0;
    private int powerUpsOut = 0;

    public void Start()
    {
        numberOfPowerups = powerupNames.Length;

        foreach (Location loc in locations)
        {
            loc.locked = false;
            loc.spawnNumber = locationCount;
            locationCount++;
        }
        
        EventSystemListeners.main.AddListener(this.gameObject);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SendSpawnPowerUpMessage();
        }

        if (!allLocationsLocked)
        {
            time += Time.deltaTime;

            if(time >= waitRandomTimeAmount)
            {
                SendSpawnPowerUpMessage();
                time = 0.0f;
                waitRandomTimeAmount = Random.Range(randomFloor, randomCeiling);
            }
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


    private void SpawnRandomPowerUp()
    {
        //int powerUpRandomNumber = Random.Range(0, numberOfPowerups);
        //int locationRandomNumber = Random.Range(0, numberOfPowerupSpawnLocations);
        //Debug.Log(powerupNames[powerUpRandomNumber]);
        //Debug.Log(locationRandomNumber);

        int powerUpRandomNumber = locationCount;
        int locationRandomNumber = HandleSpawnLocation();


        if(locationRandomNumber <= numberOfPowerupSpawnLocations)
        {
            PhotonNetwork.Instantiate(powerupNames[powerUpRandomNumber], 
                                      new Vector3(spawnLocations[locationRandomNumber].transform.position.x, spawnLocations[locationRandomNumber].transform.position.y + 1, spawnLocations[locationRandomNumber].transform.position.z), 
                                      powerupRotations[powerUpRandomNumber].transform.rotation);
            // Increment PowerUpsOut counter
        }

    }

    private int HandleSpawnLocation()
    {   
        // if PowerUpsOut counter == 0, then unlock all locations
        // random location and then lock location
        // if random location is locked, then find next unlocked
        // if all are locked then return numberOfPowerupSpawnLocations + 1
        return locationCount;
    }

    void IPowerUpManagerEvents.OnSpawnPowerup()
    {
        SpawnRandomPowerUp();
    }

    void IPowerUpManagerEvents.OnPowerUpCollected()
    {
        // Decrement PowerUpsOut counter
    }

    private class Location
    {
        public int spawnNumber;
        public bool locked;
    }
}
