using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerUpManager : MonoBehaviour, IPowerUpManagerEvents
{
    private static int numberOfPowerups;
    private static int numberOfPowerupSpawnLocations;
    private string[] powerupNames = new string[] { "FreezeBullets", "DynamiteBullets", "LaserBullets", "HealthPU", "ShieldPowerUp", "ReloadPowerUp", "SpeedPowerUp"};
    public Location[] spawnLocations;
    

    private float time = 0.0f;
    private float waitRandomTimeAmount;
    private float randomFloor = 5.0f;
    private float randomCeiling = 10.0f;
    private bool allLocationsLocked = false;

    public static PowerUpManager main;

    /// <summary>
    /// Check we are singleton
    /// </summary>
    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        else
        {
            Debug.LogWarning("EventSystemListeners re-creation attempted, destroying the new one");
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        numberOfPowerups = powerupNames.Length;
        numberOfPowerupSpawnLocations = this.transform.childCount;

        spawnLocations = new Location[numberOfPowerupSpawnLocations];

        for (int i = 0; i < numberOfPowerupSpawnLocations; i++)
        {
            spawnLocations[i] = new Location();
            spawnLocations[i].spawnPad = this.transform.GetChild(i);
            spawnLocations[i].locked = false;
        }
        
        EventSystemListeners.main.AddListener(this.gameObject);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnRandomPowerUp();
        }

        if (!allLocationsLocked)
        {
            time += Time.deltaTime;

            if (time >= waitRandomTimeAmount)
            {
                SpawnRandomPowerUp();
                time = 0.0f;
                waitRandomTimeAmount = Random.Range(randomFloor, randomCeiling);
            }
        }
    }


    private void SpawnRandomPowerUp()
    {
        //int powerUpRandomNumber = Random.Range(0, numberOfPowerups);
        //int locationRandomNumber = Random.Range(0, numberOfPowerupSpawnLocations);
        //Debug.Log(powerupNames[powerUpRandomNumber]);
        //Debug.Log(locationRandomNumber);

        if (PhotonNetwork.IsMasterClient)
        {
            int locationRandomNumber = HandleSpawnLocation();
            int powerUpRandomNumber = Random.Range(0, numberOfPowerups);
            if (!allLocationsLocked)
            {
                PhotonNetwork.Instantiate(powerupNames[powerUpRandomNumber], 
                                          new Vector3(spawnLocations[locationRandomNumber].spawnPad.position.x, spawnLocations[locationRandomNumber].spawnPad.position.y + 1, spawnLocations[locationRandomNumber].spawnPad.position.z),
                                          spawnLocations[locationRandomNumber].spawnPad.rotation);
                spawnLocations[locationRandomNumber].locked = true;
            }
        }
    }

    private int HandleSpawnLocation()
    {   
        // random location and then lock location
        int randomLocation = Random.Range(0, numberOfPowerupSpawnLocations);

        // if random location is locked, then find next unlocked
        int loopCount = 0;
        while (spawnLocations[randomLocation].locked && loopCount < numberOfPowerupSpawnLocations)
        {
            randomLocation++;
            if (randomLocation >= numberOfPowerupSpawnLocations)
                randomLocation = 0;

            loopCount++;
        }

        if (loopCount >= numberOfPowerupSpawnLocations)
        {
            allLocationsLocked = true;
        }
        else
        {
            spawnLocations[randomLocation].locked = true;
        }

        return randomLocation;
    }

    void IPowerUpManagerEvents.OnSpawnPowerup()
    {
        SpawnRandomPowerUp();
    }

    void IPowerUpManagerEvents.OnPowerUpCollected(float xPosition, float zPosition)
    {
        int i = 0;
        while(xPosition != spawnLocations[i].spawnPad.position.x && zPosition != spawnLocations[i].spawnPad.position.z)
        {
            i++;
        }

        spawnLocations[i].locked = false;

        if(allLocationsLocked)
            allLocationsLocked = false;
    }

    public class Location
    {
        public Transform spawnPad;
        public bool locked;
    }
}
