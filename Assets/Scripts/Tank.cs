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
    private float healthMax;
    private float healthCurrent;
    private float healthRegen;
    private float speedRotation;
    private float speedMovement;
    private float bulletSpeed;
    private float bulletDamage;
    private float bulletReload;
    private GameObject tankModel;
    private GameObject tankProjectile;
    private Color tankColor;
    private static Tank tankInstance;

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
    }
    public void CreateTank(string tankModelChosen, Color tankColorChosen)
    {
        switch (tankModelChosen)
        {
            case "cartoonTank":
                tankColor = tankColorChosen;
                healthMax = 100;
                healthCurrent = 100;
                healthRegen = 5;
                speedRotation = 5;
                speedMovement = 50;
                bulletSpeed = 50;
                bulletDamage = 20;
                bulletReload = 10;
                break;

            case "futureTank":
                tankColor = tankColorChosen;
                healthMax = 100;
                healthCurrent = 100;
                healthRegen = 5;
                speedRotation = 5;
                speedMovement = 50;
                bulletSpeed = 50;
                bulletDamage = 20;
                bulletReload = 10;
                break;

            case "catapult":
                tankColor = tankColorChosen;
                healthMax = 100;
                healthCurrent = 100;
                healthRegen = 5;
                speedRotation = 5;
                speedMovement = 50;
                bulletSpeed = 50;
                bulletDamage = 20;
                bulletReload = 10;
                break;

            case "boxTank":
                tankColor = tankColorChosen;
                healthMax = 100;
                healthCurrent = 100;
                healthRegen = 5;
                speedRotation = 5;
                speedMovement = 50;
                bulletSpeed = 50;
                bulletDamage = 20;
                bulletReload = 10;
                break;

        }
    }

    public void damageTaken(float damage)
    {
        if (healthCurrent > 0)
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
