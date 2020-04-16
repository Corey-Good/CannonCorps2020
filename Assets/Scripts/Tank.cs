using UnityEngine;
using UnityEngine.EventSystems;

/************************************************************************/
/* Author:        Eddie Habal                                           */
/* Date Created:  01/29/2020                                            */
/* Last Modified: 03/04/2020                                            */
/* Modified By:   Michael Agamalian                                     */
/************************************************************************/

public class Tank : MonoBehaviour, ITakeDamage
{
    #region get set functions

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
    public string tankProjectile { get; set; }
    public Color tankColor { get; set; }
    public static Tank tankInstance { get; set; }
    public bool tankHit = false;
    
    #endregion get set functions

    private void Awake()
    {
        if (tankInstance == null)
        {
            tankInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //CreateTank();

        //tankColor = Color.white;
    }

    public void CreateTank(string tankModelChosen)
    {
        #region Max and Min and Position values

        float speedMovementMax = 30f,
              speedMovementMin = 20f,

              speedRotationMax = 12f,
              speedRotationMin = 8f,

              healthCurrentMax = 120f,
              healthCurrentMin = 80f,

              //Max here refers to the shortest time
              reloadTimeMax = 1.25f,
              reloadTimeMin = 3f,

              bulletDamageMax = 10f,
              bulletDamageMin = 50f,

              /*  How to Calculate Position

                  Max is always 1
                  Min is always 0
                  anything in between = 1 / (number_of_tanks - 1) * (position - 1)

                  Example: secondPosition = 1 / (4 - 1) * (2 - 1) -> 0.333... */

              firstPosition = 1f,
              secondPosition = .66f,
              thirdPosition = .33f,
              fourthPosition = 0f;

        #endregion Max and Min and Position values

        switch (tankModelChosen)
        {
            case "cartoonTank":
                //tankColor = tankColorChosen;
                healthMax = 95;
                healthCurrent = CalculateStat(healthCurrentMax, healthCurrentMin, firstPosition);
                healthRegen = 5;
                speedRotation = CalculateStat(speedRotationMax, speedRotationMin, thirdPosition);
                speedMovement = CalculateStat(speedMovementMax, speedMovementMin, fourthPosition);
                bulletSpeed = 50;
                bulletDamage = CalculateStat(bulletDamageMax, bulletDamageMin, thirdPosition);
                reloadTime = CalculateStat(reloadTimeMax, reloadTimeMin, secondPosition);
                tankModel = "cartoonTank";
                tankProjectile = "Bullet";
                break;

            case "futureTank":
                //tankColor = tankColorChosen;
                healthMax = 80;
                healthCurrent = CalculateStat(healthCurrentMax, healthCurrentMin, thirdPosition);
                healthRegen = 5;
                speedRotation = CalculateStat(speedRotationMax, speedRotationMin, secondPosition);
                speedMovement = CalculateStat(speedMovementMax, speedMovementMin, secondPosition);
                bulletSpeed = 50;
                bulletDamage = CalculateStat(bulletDamageMax, bulletDamageMin, fourthPosition);
                reloadTime = CalculateStat(reloadTimeMax, reloadTimeMin, firstPosition);
                tankModel = "futureTank";
                tankProjectile = "SphereLazer";
                break;

            case "catapult":
                // tankColor = tankColorChosen;
                healthMax = 90f;
                healthCurrent = CalculateStat(healthCurrentMax, healthCurrentMin, fourthPosition);
                healthRegen = 5;
                speedRotation = CalculateStat(speedRotationMax, speedRotationMin, firstPosition);
                speedMovement = CalculateStat(speedMovementMax, speedMovementMin, thirdPosition);
                bulletSpeed = 50;
                bulletDamage = CalculateStat(bulletDamageMax, bulletDamageMin, firstPosition);
                reloadTime = 3.8f;
                tankModel = "catapult";
                tankProjectile = "RockBullet";
                break;

            case "baseTank":
                // tankColor = tankColorChosen;
                healthMax = 100;
                healthCurrent = CalculateStat(healthCurrentMax, healthCurrentMin, secondPosition);
                healthRegen = 5;
                speedRotation = CalculateStat(speedRotationMax, speedRotationMin, fourthPosition);
                speedMovement = CalculateStat(speedMovementMax, speedMovementMin, firstPosition);
                bulletSpeed = 50;
                bulletDamage = CalculateStat(bulletDamageMax, bulletDamageMin, secondPosition);
                reloadTime = CalculateStat(reloadTimeMax, reloadTimeMin, thirdPosition);
                tankModel = "baseTank";
                tankProjectile = "Bullet";
                break;
        }
    }

    public void damageTaken(float damage)
    {
        if (damage > 0)
        {
            SendPlayerHurtMessages();
        }

        healthCurrent -= damage;

        if (healthCurrent >= healthMax)
        {
            healthCurrent = healthMax;
        }
    }

    public void OnDestroy()
    {
        //play animation and sound
    }

    private void SendPlayerHurtMessages()
    {
        // Send message to any listeners
        if (EventSystemListeners.main.listeners != null)
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

    // Calculate the tank statistic based on its ranking relative to the other tanks
    public float CalculateStat(float max, float min, float position)
    {
        return ((max - min) * (1f + position) + (-((max - min) - min)));
    }
}