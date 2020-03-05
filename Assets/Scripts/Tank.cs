using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/************************************************************************/
/* Author: Eddie Habal */
/* Date Created: 1/29/2020 */
/* Last Modified Date: */
/* Modified By: */
/************************************************************************/

public class Tank : MonoBehaviour, ITakeDamage
{
    public float healthMax { get; set; }
    public float healthCurrent { get; set; }
    public float healthRegen { get; set; } 
    public float speedRotation { get; set; }
    public float speedMovement { get; set; }
    public float bulletSpeed { get; set; }
    public float bulletDamage { get; set; }
    public float reloadTime { get; set; }
    public float reloadProgress { get; set; }
    public string tankModel { get; set; }
    public GameObject tankProjectile { get; set; }
    public Color tankColor { get; set; }
    public static Tank tankInstance { get; set; }
    public bool tankHit = false;


    void Awake()
    {
        if (tankInstance == null)
        {
            tankInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        CreateTank();

        tankColor = Color.white;
    }
    public void CreateTank(string tankModelChosen = "baseTank")
    {
        switch (tankModelChosen)
        {
            case "cartoonTank":
                //tankColor = tankColorChosen;
                healthMax = 95;
                healthCurrent = 95;
                healthRegen = 5;
                speedRotation = 9;
                speedMovement = 22.5f;
                bulletSpeed = 50;
                bulletDamage = 20;
                reloadTime = 1.75f;
                tankModel = "cartoonTank";
                break;

            case "futureTank":
                //tankColor = tankColorChosen;
                healthMax = 80;
                healthCurrent = 80;
                healthRegen = 5;
                speedRotation = 10;
                speedMovement = 30f;
                bulletSpeed = 50;
                bulletDamage = 20;
                reloadTime = 1.45f;
                tankModel = "futureTank";
                break;

            case "Catapult":
               // tankColor = tankColorChosen;
                healthMax = 90f;
                healthCurrent = 90f;
                healthRegen = 5f;
                speedRotation = 12f;
                speedMovement = 18.75f;
                bulletSpeed = 50f;
                bulletDamage = 20f;
                reloadTime = 3f;
                tankModel = "Catapult";
                break;

            case "baseTank":
               // tankColor = tankColorChosen;
                healthMax = 100;
                healthCurrent = 100;
                healthRegen = 5;
                speedRotation = 9;
                speedMovement = 15;
                bulletSpeed = 50;
                bulletDamage = 20;
                reloadTime = 2f;
                tankModel = "baseTank";
                break;

        }
    }

    public void damageTaken(float damage)
    {
        if (healthCurrent >= 0)
        {
            healthCurrent -= damage;
        }
        else
        {
            Destroy(this);
        }
    }

    public void OnDestroy()
    {
        //play animation and sound
    }
}
