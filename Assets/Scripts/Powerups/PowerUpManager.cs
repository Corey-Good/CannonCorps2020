﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerUpManager : MonoBehaviour, IPowerUpManagerEvents
{
    private static int numberOfPowerups;
    private static int numberOfPowerupSpawnLocations = 14;
    public GameObject[] spawnLocations = new GameObject[numberOfPowerupSpawnLocations];
    private string[] powerupNames = new string[] { "FreezeBullets", "DynamiteBullets", "LaserBullets", "HealthPowerUp", "ShieldPowerUp", "ReloadPowerUp", "SpeedPowerUp"};
    public bool[] lockedLocations = new bool[numberOfPowerupSpawnLocations];
    

    private float time = 0.0f;
    private float waitRandomTimeAmount;
    private float randomFloor = 5.0f;
    private float randomCeiling = 10.0f;
    private bool allLocationsLocked = false;
    public int powerUpsOut = 0;

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

        for(int i = 0; i < numberOfPowerupSpawnLocations; i++)
        {
            lockedLocations[i] = false;
        }
        
        EventSystemListeners.main.AddListener(this.gameObject);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnRandomPowerUp();
        }

        // if PowerUpsOut counter == 0, then unlock all locations
        if (powerUpsOut == 0)
        {
            for (int i = 0; i < numberOfPowerupSpawnLocations; i++)
            {
                lockedLocations[i] = false;
            }

            allLocationsLocked = false;
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

            if (locationRandomNumber < numberOfPowerupSpawnLocations)
            {
                PhotonNetwork.Instantiate(powerupNames[powerUpRandomNumber], 
                                          new Vector3(spawnLocations[locationRandomNumber].transform.position.x, spawnLocations[locationRandomNumber].transform.position.y + 1, spawnLocations[locationRandomNumber].transform.position.z),
                                          spawnLocations[locationRandomNumber].transform.rotation);
                // Increment PowerUpsOut counter
                powerUpsOut++;
            }
        }
    }

    private int HandleSpawnLocation()
    {   
        // random location and then lock location
        int randomLocation = Random.Range(0, numberOfPowerupSpawnLocations);

        // if random location is locked, then find next unlocked
        int loopCount = 0;
        while (CheckIfLocked(randomLocation) && loopCount < numberOfPowerupSpawnLocations)
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
            lockedLocations[randomLocation] = true;
        }


        loopCount = 0;

        // if all are locked then return numberOfPowerupSpawnLocations + 1
        if (allLocationsLocked)
            randomLocation = numberOfPowerupSpawnLocations;

        return randomLocation;
    }

    private bool CheckIfLocked(int locationToCheck)
    {
        return lockedLocations[locationToCheck];
    }

    void IPowerUpManagerEvents.OnSpawnPowerup()
    {
        SpawnRandomPowerUp();
    }

    void IPowerUpManagerEvents.OnPowerUpCollected()
    {
        // Decrement PowerUpsOut counter
        powerUpsOut--;
    }


}
