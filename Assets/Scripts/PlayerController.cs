/************************************************************************/
/* Author:             Corey Good                                       */
/* Date Created:       1/27/2020                                        */
/* Last Modified Date: 4/11/2020                                        */
/* Modified By:        Corey Good                                       */
/************************************************************************/

using Photon.Pun;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    #region Variables

    private float lagAdjustSpeed = 20f;
    private float timeElapsed = 0f;
    public bool readyToFire = true;
    public bool invulnerable = false;

    #region BulletVariables
    public float numOfFreezeBullets;
    public float numOfDynamiteBullets;
    public float numOfLaserBullets;
    public float maxNumOfFreezeBullets = 10;
    public float maxNumOfDynamiteBullets = 5;
    public float maxNumOfLaserBullets = 15;

    public enum BulletType
    {
        Normal,
        FreezeBullet,
        DynamiteBullet,
        LaserBullet
    }

    public BulletType currentBulletType;
    private int numberOfBulletTypes = System.Enum.GetValues(typeof(BulletType)).Length;
    #endregion

    #region Reload Powerup Variables
    public float reloadBoostTimer = 0.0f;
    public float maxReloadBoostTimer = 10.0f;
    public bool reloadBoostTimerRunning = false;
    
    private float reloadBoost = 1.0f;
    private float originalReloadBoost = 1.0f;
    #endregion

    #region Movement Speed Powerup Variables
    public float speedBoostTimer;
    public float maxSpeedBoostTimer = 6.0f;
    private float oneSpeedCharge = 2.0f;
    public bool speedBoostTimerRunning = false;
    public bool isFrozen = false;

    private float timeLeftOnCharge;
    private float movementForce;
    private float rotateSpeed;
    private float movementMultiplier;
    private float rotateMultiplier;
    private float originalMovementMultiplier = 1.0f;
    private float originalRotateMultiplier = 8.0f;
    #endregion

    #region Public Reference Variables
    public Animator fireAnimation;
    public Camera tankCamera;
    public GameObject tankBody;
    public GameObject tankHead;
    #endregion

    #region Private Reference Variables
    private Vector3 headPosition;
    private Vector3 bodyPosition;
    private Quaternion headRotation;
    private Quaternion bodyRotation;
    private Tank tank;
    private Player player;
    private FireMechanism fireMechanism;

    #endregion

    #endregion

    void Start()
    {
        //playerState = states.Stationary;

        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        fireMechanism = GetComponentInChildren<FireMechanism>();

        movementForce = tank.speedMovement;
        movementMultiplier = originalMovementMultiplier;
        rotateMultiplier = originalRotateMultiplier;
        rotateSpeed = tank.speedRotation;
        player.photonView = this.gameObject.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            LagAdjust();
            return;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            tank.damageTaken(10f);
        }

        if (!PauseMenuManager.gameIsPaused && TutorialPrompts.MovementIsEnabled)
        {
            MovePlayer();

            FireBullet();

            ManageReloadProcess();

            ManagePowerups();
        }
    }

    private void ManagePowerups()
    {
        if (Input.GetKeyDown(KeyBindings.switchBulletType)) // Cycle through bullets
        {
            currentBulletType += 1;
            SendBulletSwitchMessage();

            if (numOfFreezeBullets == 0.0f)
                if ((int)currentBulletType == 1)
                {
                    currentBulletType += 1;
                    SendBulletSwitchMessage();
                }

            if (numOfDynamiteBullets == 0.0f)
                if ((int)currentBulletType == 2)
                {
                    SendBulletSwitchMessage();
                    currentBulletType += 1;
                }

            if (numOfLaserBullets == 0.0f)
                if ((int)currentBulletType == 3)
                {
                    SendBulletSwitchMessage();
                    currentBulletType += 1;
                }

            if ((int)currentBulletType >= numberOfBulletTypes)
                currentBulletType = 0;
        }

        if (Input.GetKeyDown(KeyBindings.activateReloadBoost))
        {
            SendReloadToggleMessage();
        }

        if (reloadBoostTimerRunning)
        {
            reloadBoostTimer -= Time.deltaTime;
            if (reloadBoostTimer <= 0.0f)
            {
                reloadBoostTimer = 0.0f;
                SendReloadPowerUpExpiredMessage();
            }

        }

        if (Input.GetKeyDown(KeyBindings.activateMovementBoost))
        {
            HandleSpeedBoostCharge();
        }

        if (speedBoostTimerRunning)
        {
            timeLeftOnCharge -= Time.deltaTime;
            speedBoostTimer -= Time.deltaTime;

            if (timeLeftOnCharge <= 0.0f) // Only let one charge run at a time
            {
                SendSpeedToggleMessage();
            }

            if (speedBoostTimer <= 0.0f)
            {
                speedBoostTimer = 0.0f;
                SendSpeedPowerUpExpiredMessage();
            }
        }
    }

    private void MovePlayer()
    {
        // Move play forwards and backwards,
        if (Input.GetKey(KeyBindings.forwardKey))
        {
            transform.position += transform.forward * Time.deltaTime * movementForce * movementMultiplier;
        }
        else if (Input.GetKey(KeyBindings.backwardKey))
        {
            transform.position += -transform.forward * Time.deltaTime * movementForce * movementMultiplier;
        }

        if(tank.tankModel == "catapult")
        {
            if (KeyBindings.XisInverted)
            {
                transform.Rotate(0f, Input.GetAxis("Mouse X") * -300 * Time.deltaTime, 0f);
            }
            else
            {
                transform.Rotate(0f, Input.GetAxis("Mouse X") * 300 * Time.deltaTime, 0f);
            }
        }
        
        if (Input.GetKey(KeyBindings.rightKey))
        {
            transform.Rotate(Vector3.up * rotateSpeed * rotateMultiplier * Time.deltaTime);
        }
        else if (Input.GetKey(KeyBindings.leftKey))
        {
            transform.Rotate(-Vector3.up * rotateSpeed * rotateMultiplier * Time.deltaTime);
        }
        

    }

    private void FireBullet()
    {
        if (Input.GetMouseButtonDown(KeyBindings.clickIndex) && TutorialPrompts.FiringIsEnabled && readyToFire)
        {
            if (tank.tankModel == "catapult")
            {
                StartCoroutine(DelayFire());
                fireAnimation.SetTrigger("Fire");
                readyToFire = false;
            }
            else
            {
                fireMechanism.FireBullet(currentBulletType);
                readyToFire = false;
            }

            HandleBullets();
        }
    }

    private void HandleBullets()
    {
        if (!readyToFire && currentBulletType != BulletType.Normal) // If just fired, check if any bullets are left
        {
            switch (currentBulletType)
            {
                case BulletType.FreezeBullet:
                    numOfFreezeBullets -= 1.0f;
                    if (numOfFreezeBullets == 0.0f)
                    {
                        SendBulletSwitchMessage();
                        SendBulletSwitchMessage();
                        SendBulletSwitchMessage();
                        currentBulletType = BulletType.Normal;
                    }

                    break;

                case BulletType.DynamiteBullet:
                    numOfDynamiteBullets -= 1.0f;
                    if (numOfDynamiteBullets == 0.0f)
                    {
                        SendBulletSwitchMessage();
                        SendBulletSwitchMessage();
                        currentBulletType = BulletType.Normal;
                    }
                    break;

                case BulletType.LaserBullet:
                    numOfLaserBullets -= 1.0f;
                    if (numOfLaserBullets == 0.0f)
                    {
                        SendBulletSwitchMessage();
                        currentBulletType = BulletType.Normal;
                    }
                    break;
            }
        }       
    }

    private void ManageReloadProcess()
    {
        if (!readyToFire)
        {
            // Increase time and update the reloadBar progress
            timeElapsed += Time.deltaTime;
            tank.reloadProgress = timeElapsed / (tank.reloadTime * reloadBoost);

            // When a bullet is reloaded, reset timer
            if (timeElapsed >= (tank.reloadTime * reloadBoost))
            {
                timeElapsed = 0f;
                readyToFire = true;
            }
        }
    }

    private void HandleSpeedBoostCharge()
    {
        if (speedBoostTimerRunning) // Don't use a charge if one is currently running
            return;
        else if (!speedBoostTimerRunning)
        {
            if(!isFrozen) // Allow a charge to be used
            {
                SendSpeedToggleMessage();
            }
            else if(isFrozen && (speedBoostTimer >= oneSpeedCharge)) // Burn a charge to break out of freeze
            {
                isFrozen = false;
                SendSpeedToggleMessage();
                SendSpeedToggleMessage();
                speedBoostTimer -= oneSpeedCharge;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            stream.SendNext(tankBody.transform.position);
            stream.SendNext(tankBody.transform.rotation);
            
            
            if (tank.tankModel == "catapult")
            {                
                stream.SendNext(new Vector3(tankBody.transform.position.x, tankHead.transform.position.y, tankBody.transform.position.z));
                stream.SendNext(tankBody.transform.rotation);
                Debug.Log("Is this even working?");
            }
            else
            {
                stream.SendNext(tankHead.transform.position);
                stream.SendNext(tankHead.transform.rotation);
            }
            


        }
        else
        {
            bodyPosition = (Vector3)stream.ReceiveNext();
            bodyRotation = (Quaternion)stream.ReceiveNext();
            headPosition = (Vector3)stream.ReceiveNext();
            headRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    public void LagAdjust()
    {
        #region CalculateLag

        var bodyPositionLag = tankBody.transform.position - bodyPosition;
        var bodyRotationLag = tankBody.transform.rotation.eulerAngles - bodyRotation.eulerAngles;
        var headPositionLag = tankHead.transform.position - headPosition;
        var headRotationLag = tankHead.transform.rotation.eulerAngles - headRotation.eulerAngles;

        #endregion CalculateLag

        #region AdjustBodyPosition

        if (bodyPositionLag.magnitude > 5f)
        {
            tankBody.transform.position = bodyPosition;
        }
        else if (bodyPositionLag.magnitude < 0.11f)
        {
            // do nothing
        }
        else
        {
            Vector3.MoveTowards(tankBody.transform.position, bodyPosition, lagAdjustSpeed * Time.deltaTime);
        }

        #endregion AdjustBodyPosition

        #region AdjustBodyRotation

        if (bodyRotationLag.magnitude > 5.0f)
        {
            tankBody.transform.rotation = bodyRotation;
        }
        else if (bodyRotationLag.magnitude < 0.11f)
        {
            // do nothing
        }
        else
        {
            Quaternion.RotateTowards(tankBody.transform.rotation, bodyRotation, lagAdjustSpeed * Time.deltaTime);
        }

        #endregion AdjustBodyRotation

        #region AdjustHeadPosition

        if (headPositionLag.magnitude > 5.0f)
        {
            tankHead.transform.position = headPosition;
        }
        else if (headPositionLag.magnitude < 0.11f)
        {
            // do nothing
        }
        else
        {
            Vector3.MoveTowards(tankHead.transform.position, headPosition, lagAdjustSpeed * Time.deltaTime);
        }

        #endregion AdjustHeadPosition

        #region AdjustHeadRotation

        if (headRotationLag.magnitude > 5.0f)
        {
            tankHead.transform.rotation = headRotation;
        }
        else if (headRotationLag.magnitude < 0.11f)
        {
            // do nothing
        }
        else
        {
            Quaternion.RotateTowards(tankHead.transform.rotation, headRotation, lagAdjustSpeed * Time.deltaTime);
        }

        #endregion AdjustHeadRotation
    }

    #region Powerups

    #region Speed Logic
    public void FreezeTank(bool frozen)
    {
        isFrozen = frozen;
    }
    public void PickupSpeedPowerup()
    {
        if (photonView.IsMine)
        {
            SendSpeedPowerUpExpiredMessage();
            speedBoostTimer += oneSpeedCharge;
            timeLeftOnCharge = oneSpeedCharge;
            if(speedBoostTimer >= maxSpeedBoostTimer)
            {
                speedBoostTimer = maxSpeedBoostTimer;
            }

        }
    }
    public void SetSpeedBoostOn(float newMovementMultiplier, float newRotateMultiplier, bool isFreezePowerup)
    {
        if (photonView.IsMine)
        {
            movementMultiplier = newMovementMultiplier;
            rotateMultiplier = newRotateMultiplier;

            if(!isFreezePowerup)
                speedBoostTimerRunning = true;

            FreezeTank(isFreezePowerup);
        }
    }

    public void SetSpeedBoostOn(float newMovementMultiplier, float newRotateMultiplier, float newSpeedTime)
    {
        if (photonView.IsMine)
        {
            speedBoostTimer = newSpeedTime;
            movementMultiplier = newMovementMultiplier;
            rotateMultiplier = newRotateMultiplier;
            speedBoostTimerRunning = true;
        }
    }

    public void SetSpeedBoostOff()
    {
        if(photonView.IsMine)
        {
            movementMultiplier = originalMovementMultiplier;
            rotateMultiplier = originalRotateMultiplier;
            timeLeftOnCharge = oneSpeedCharge;
            speedBoostTimerRunning = false;
        }
    }

    #endregion

    public void SetHealthBoost(float damage)
    {
        if (photonView.IsMine)
        {
            if(damage > 0.0f)
            {
                if (invulnerable) // Set invulnerable to false don't take damage for that hit
                {
                    invulnerable = false;
                    return;
                }
                else
                {
                    tank.damageTaken(damage);
                    tank.tankHit = true;
                }
                    
            }
            else if (damage < 0.0f) // Let health boost pass through
            {   
                tank.damageTaken(damage);
            }
          
        }
            
    }

    #region Reload Logic

    public void SetReloadBoostOn(float newReloadBoost)
    {
        if (photonView.IsMine)
        {
            SendReloadPowerUpExpiredMessage();
            reloadBoostTimer = maxReloadBoostTimer;
            reloadBoost = newReloadBoost;
            reloadBoostTimerRunning = true;
        }
    }

    public void SetReloadBoostOn(float newReloadBoost, float newReloadTime)
    {
        if (photonView.IsMine)
        {
            reloadBoostTimer = newReloadTime;
            reloadBoost = newReloadBoost;
            reloadBoostTimerRunning = true;
        }
    }

    public void SetReloadBoostOff()
    {
        if (photonView.IsMine)
        {
            reloadBoost = originalReloadBoost;
            reloadBoostTimerRunning = false;
        }
    }

    #endregion

    public void SetShieldBoostOn()
    {
        if (photonView.IsMine)
        {
            invulnerable = true;
        }
            
    }
    #endregion

    #region Collect Bullets
    public void CollectFreezeBullets()
    {
        if (photonView.IsMine)
        {
                numOfFreezeBullets = maxNumOfFreezeBullets;
        }

    }

    public void CollectDynamiteBullets()
    {
        if (photonView.IsMine)
        {
                numOfDynamiteBullets = maxNumOfDynamiteBullets;
        }
            
    }

    public void CollectLaserBullets()
    {
        if (photonView.IsMine)
        {
                numOfLaserBullets = maxNumOfLaserBullets;
        }
            
    }
    #endregion

    #region Messages

    #region Send BulletSwitch Messages
    private void SendBulletSwitchMessage()
    {
        // Send message to any listeners
        if (EventSystemListeners.main.listeners != null)
        {
            foreach (GameObject go in EventSystemListeners.main.listeners)  // 1
            {
                ExecuteEvents.Execute<ICarouselEvents>                   // 2
                    (go, null,                                               // 3
                     (x, y) => x.OnRotateCarousel()            // 4
                    );
            }
        }
    }

    #endregion

    #region Reload Powerup Messages
    public void SendReloadPowerUpExpiredMessage()
    {
        // Send message to any listeners
        if (EventSystemListeners.main.listeners != null)
        {
            foreach (GameObject go in EventSystemListeners.main.listeners)  // 1
            {
                ExecuteEvents.Execute<IPowerUpEvents>                   // 2
                    (go, null,                                               // 3
                     (x, y) => x.OnReloadBoostExpired()            // 4
                    );
            }
        }
    }

    public void SendReloadToggleMessage()
    {
        // Send message to any listeners
        if (EventSystemListeners.main.listeners != null)
        {
            foreach (GameObject go in EventSystemListeners.main.listeners)  // 1
            {
                ExecuteEvents.Execute<IPowerUpEvents>                   // 2
                    (go, null,                                               // 3
                     (x, y) => x.ToggleReloadBoost()            // 4
                    );
            }
        }
    }

    #endregion

    #region Speed Powerup Messages
    public void SendSpeedPowerUpExpiredMessage()
    {
        // Send message to any listeners
        if (EventSystemListeners.main.listeners != null)
        {
            foreach (GameObject go in EventSystemListeners.main.listeners)  // 1
            {
                ExecuteEvents.Execute<IPowerUpEvents>                   // 2
                    (go, null,                                               // 3
                     (x, y) => x.OnSpeedBoostExpired()            // 4
                    );
            }
        }
    }

    private void SendSpeedToggleMessage()
    {
        // Send message to any listeners
        if (EventSystemListeners.main.listeners != null)
        {
            foreach (GameObject go in EventSystemListeners.main.listeners)  // 1
            {
                ExecuteEvents.Execute<IPowerUpEvents>                   // 2
                    (go, null,                                               // 3
                     (x, y) => x.ToggleSpeedBoost()            // 4
                    );
            }
        }
    }
    #endregion

    #endregion

    private IEnumerator DelayFire()
    {
        yield return new WaitForSeconds(0.3f);
        fireMechanism.FireBullet(currentBulletType);
    }
}
