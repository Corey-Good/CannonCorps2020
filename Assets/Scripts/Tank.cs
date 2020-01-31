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
    private float healthMax     { get; set; }
    private float healthCurrent { get; set; }
    private float healthRegen   { get; set; }
    private float speedRotation { get; set; }
    private float speedMovement { get; set; }
    private float bulletSpeed   { get; set; }
    private float bulletDamage  { get; set; }
    private float bulletReload  { get; set; }

    private GameObject tankModel { get; set; }
    private GameObject tankProjectile { get; set; }

    private Color tankColor { get; set; }

    public Tank (GameObject tankModelChosen, Color tankColorChosen)
    {
        switch(tankModelChosen.name)
        {
            case "cartoonTank":
                tankColor     = tankColorChosen;
                healthMax     = 100;
                healthCurrent = 100;
                healthRegen   = 5;
                speedRotation = 5;
                speedMovement = 50;
                bulletSpeed   = 50;
                bulletDamage  = 20;
                bulletReload  = 10;
                break;

            case "futureTank":
                tankColor     = tankColorChosen;
                healthMax     = 100;
                healthCurrent = 100;
                healthRegen   = 5;
                speedRotation = 5;
                speedMovement = 50;
                bulletSpeed   = 50;
                bulletDamage  = 20;
                bulletReload  = 10;
                break;

            case "catapult":
                tankColor     = tankColorChosen;
                healthMax     = 100;
                healthCurrent = 100;
                healthRegen   = 5;
                speedRotation = 5;
                speedMovement = 50;
                bulletSpeed   = 50;
                bulletDamage  = 20;
                bulletReload  = 10;
                break;

            case "boxTank":
                tankColor     = tankColorChosen;
                healthMax     = 100;
                healthCurrent = 100;
                healthRegen   = 5;
                speedRotation = 5;
                speedMovement = 50;
                bulletSpeed   = 50;
                bulletDamage  = 20;
                bulletReload  = 10;
                break;
               
        }
    }

    public void damageTaken(float damage)
    {
        healthCurrent -= damage;
    }
}
