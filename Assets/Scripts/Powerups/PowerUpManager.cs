using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject[] powerupRotations = new GameObject[4];
    public GameObject[] spawnLocations = new GameObject[4];
    private string[] powerupNames = new string[] { "FreezeBullets", "DynamiteBullets", "LaserBullets", "HealthPowerUp", "ShieldPowerUp", "ReloadPowerUp", "SpeedPowerUp"};
    private float time = 0.0f;

    public void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time > 8.0f)
        {
            SpawnRandomPowerUp();
            time = 0.0f;
        }
    }

    private void SpawnRandomPowerUp()
    {
        int randomNumber = Random.Range(0, 4);
        Debug.Log(randomNumber);
        float yValue = 1.0f;
        if (randomNumber == 1)
        {
            yValue = 0.0f;
        }
        else if (randomNumber == 3)
        {
            yValue -= 0.55f;
        }

        PhotonNetwork.Instantiate(powerupNames[randomNumber], new Vector3(Random.Range(200.0f, 315.0f), yValue, Random.Range(215.0f, 350.0f)), powerupRotations[randomNumber].transform.rotation);
    }
}
