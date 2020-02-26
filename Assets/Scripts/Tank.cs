using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


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
                speedRotation = 10;
                speedMovement = 25;
                bulletSpeed = 50;
                bulletDamage = 20;
                reloadTime = 2f;
                tankModel = "cartoonTank";
                break;

            case "futureTank":
                //tankColor = tankColorChosen;
                healthMax = 100;
                healthCurrent = 100;
                healthRegen = 5;
                speedRotation = 10;
                speedMovement = 25;
                bulletSpeed = 50;
                bulletDamage = 20;
                reloadTime = 1.8f;
                tankModel = "futureTank";
                break;

            case "Catapult":
               // tankColor = tankColorChosen;
                healthMax = 100;
                healthCurrent = 100;
                healthRegen = 5;
                speedRotation = 10;
                speedMovement = 25;
                bulletSpeed = 50;
                bulletDamage = 20;
                reloadTime = 3f;
                tankModel = "Catapult";
                break;

            case "baseTank":
               // tankColor = tankColorChosen;
                healthMax = 100;
                healthCurrent = 100;
                healthRegen = 5;
                speedRotation = 10;
                speedMovement = 25;
                bulletSpeed = 50;
                bulletDamage = 20;
                reloadTime = 2f;
                tankModel = "baseTank";
                break;

        }
    }

    public void damageTaken(float damage)
    {
        SendPlayerHurtMessages();
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

    private void SendPlayerHurtMessages()
    {
        // Send message to any listeners
        if(EventSystemListeners.main.listeners != null)
        {
            foreach (GameObject go in EventSystemListeners.main.listeners)  // 1
            {
                ExecuteEvents.Execute<IPlayerEvents>                   // 2
                    (go, null,                                               // 3
                     (x, y) => x.OnPlayerHurt((int)healthCurrent)            // 4
                    );
            }
        }
        
    }
}
