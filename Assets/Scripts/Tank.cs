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
    public float bulletReload { get; set; }
    public string tankModel { get; set; }
    public GameObject tankProjectile { get; set; }
    public Color tankColor { get; set; }
    public static Tank tankInstance { get; set; }


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
    }
    public void CreateTank(string tankModelChosen = "baseTank")
    {
        switch (tankModelChosen)
        {
            case "cartoonTank":
                //tankColor = tankColorChosen;
                healthMax = 100;
                healthCurrent = 100;
                healthRegen = 5;
                speedRotation = 5;
                speedMovement = 50;
                bulletSpeed = 50;
                bulletDamage = 20;
                bulletReload = 10;
                tankModel = "cartoonTank";
                break;

            case "futureTank":
                //tankColor = tankColorChosen;
                healthMax = 100;
                healthCurrent = 100;
                healthRegen = 5;
                speedRotation = 5;
                speedMovement = 50;
                bulletSpeed = 50;
                bulletDamage = 20;
                bulletReload = 10;
                tankModel = "futureTank";
                break;

            case "Catapult":
               // tankColor = tankColorChosen;
                healthMax = 100;
                healthCurrent = 100;
                healthRegen = 5;
                speedRotation = 5;
                speedMovement = 50;
                bulletSpeed = 50;
                bulletDamage = 20;
                bulletReload = 10;
                tankModel = "Catapult";
                break;

            case "baseTank":
               // tankColor = tankColorChosen;
                healthMax = 100;
                healthCurrent = 100;
                healthRegen = 5;
                speedRotation = 5;
                speedMovement = 50;
                bulletSpeed = 50;
                bulletDamage = 20;
                bulletReload = 10;
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
