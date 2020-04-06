/************************************************************************/
/* Author:             Corey Good                                       */
/* Date Created:       1/27/2020                                        */
/* Last Modified Date: 2/27/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/

using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    #region Variables

    private Vector3 headPosition;
    private Vector3 bodyPosition;
    private Quaternion headRotation;
    private Quaternion bodyRotation;
    private float lagAdjustSpeed = 20f;
    private float timeElapsed = 0f;
    public bool readyToFire = true;

    public float numOfFreezeBullets;
    public float numOfDynamiteBullets;
    public float numOfLaserBullets;
    public float maxNumOfFreezeBullets = 10;
    public float maxNumOfDynamiteBullets = 5;
    public float maxNumOfLaserBullets = 15;

    private float reloadBoost = 1.0f;
    private float originalReloadBoost = 1.0f;
    private bool speedBoostOn = false;

    public enum BulletType
    {
        Normal,
        FreezeBullet,
        DynamiteBullet,
        LaserBullet
    }

    public BulletType currentBulletType;
    private int numberOfBulletTypes = System.Enum.GetValues(typeof(BulletType)).Length;

    public Animator fireAnimation;
    public Camera tankCamera;
    public GameObject tankBody;
    public GameObject tankHead;

    private FireMechanism fireMechanism;

    #endregion Variables

    #region Movement Speeds

    private float movementForce;
    private float movementMultiplier;
    private float originalMovementMultiplier;
    private float originalRotateMultiplier;
    private float rotateMultiplier;
    private float rotateSpeed;
    #endregion Movement Speeds

    private Tank tank;
    private Player player;
    private CollisionDetection collisionDetection;

    // Start is called before the first frame update
    private void Start()
    {
        //playerState = states.Stationary;

        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        collisionDetection = GetComponent<CollisionDetection>();
        fireMechanism = GetComponentInChildren<FireMechanism>();

        movementForce = tank.speedMovement;
        movementMultiplier = 1f;
        originalMovementMultiplier = movementMultiplier;
        rotateMultiplier = 8f;
        originalRotateMultiplier = rotateMultiplier;
        rotateSpeed = tank.speedRotation;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!photonView.IsMine)
        {
            LagAdjust();
            return;
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            tank.damageTaken(10f);
        }

        if (!PauseMenuManager.gameIsPaused && TutorialMode.MovementIsEnabled)
        {
            MovePlayer();
            if (Input.GetMouseButtonDown(KeyBindings.clickIndex)
               && (!PauseMenuManager.gameIsPaused && TutorialMode.FiringIsEnabled)
               && (readyToFire))
            {
                readyToFire = false;

                if (tank.tankModel == "catapult")
                {
                    StartCoroutine(DelayFire());
                }
                else
                {
                    fireMechanism.FireBullet();
                }

                if (fireAnimation != null)
                {
                    fireAnimation.SetTrigger("Fire");
                }
                fireMechanism.ReceivePlayerControllerClick(readyToFire, currentBulletType);
                if (!readyToFire)
                {
                    switch (currentBulletType)
                    {
                        case BulletType.FreezeBullet:
                            numOfFreezeBullets -= 1.0f;
                            if (numOfFreezeBullets == 0.0f)
                                currentBulletType = BulletType.Normal;
                            break;

                        case BulletType.DynamiteBullet:
                            numOfDynamiteBullets -= 1.0f;
                            if (numOfDynamiteBullets == 0.0f)
                                currentBulletType = BulletType.Normal;
                            break;

                        case BulletType.LaserBullet:
                            numOfLaserBullets -= 1.0f;
                            if (numOfLaserBullets == 0.0f)
                                currentBulletType = BulletType.Normal;
                            break;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentBulletType += 1;

                if (numOfFreezeBullets == 0.0f)
                    if ((int)currentBulletType == 1)
                        currentBulletType += 1;
                if (numOfDynamiteBullets == 0.0f)
                    if ((int)currentBulletType == 2)
                        currentBulletType += 1;
                if (numOfLaserBullets == 0.0f)
                    if ((int)currentBulletType == 3)
                        currentBulletType += 1;

                if ((int)currentBulletType >= numberOfBulletTypes)
                    currentBulletType = 0;
            }
        }

        ReloadBullet();
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

        if (Input.GetKey(KeyBindings.rightKey))
        {
            transform.Rotate(Vector3.up * rotateSpeed * rotateMultiplier * Time.deltaTime);
        }
        else if (Input.GetKey(KeyBindings.leftKey))
        {
            transform.Rotate(-Vector3.up * rotateSpeed * rotateMultiplier * Time.deltaTime);
        }
    }

    public void ReloadBullet()
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if(tank.tankModel == "Catapult")
            {
                stream.SendNext(tankBody.transform.position);
                stream.SendNext(tankBody.transform.rotation);
                stream.SendNext(tankBody.transform.position);
                stream.SendNext(tankBody.transform.rotation);
            }
            else
            {
                stream.SendNext(tankBody.transform.position);
                stream.SendNext(tankBody.transform.rotation);
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

    public void SetSpeedBoostOn(float newMovementMultiplier, float newRotateMultiplier)
    {
        if (speedBoostOn)
        {
            return;
        }

        movementMultiplier = newMovementMultiplier;
        rotateMultiplier = newRotateMultiplier;
        speedBoostOn = true;
    }

    public void SetSpeedBoostOff()
    {
        movementMultiplier = originalMovementMultiplier;
        rotateMultiplier = originalRotateMultiplier;
        speedBoostOn = false;
    }

    public void SetHealthBoost(float healthBoost)
    {
        healthBoost = -healthBoost;
        tank.damageTaken(healthBoost);
    }

    public void SetReloadBoostOn(float newReloadBoost)
    {
        originalReloadBoost = reloadBoost;
        reloadBoost = newReloadBoost;
    }

    public void SetReloadBoostOff()
    {
        reloadBoost = originalReloadBoost;
    }

    public void SetShieldBoostOn()
    {
        collisionDetection.shieldBoostOn = true;
    }

    public void CollectFreezeBullets(float freezeBullets)
    {
        numOfFreezeBullets += freezeBullets;
        if (numOfFreezeBullets >= maxNumOfFreezeBullets)
        {
            numOfFreezeBullets = maxNumOfFreezeBullets;
        }
    }

    public void CollectDynamiteBullets(float dynamiteBullets)
    {
        numOfDynamiteBullets += dynamiteBullets;
        if (numOfDynamiteBullets >= maxNumOfDynamiteBullets)
        {
            numOfDynamiteBullets = maxNumOfDynamiteBullets;
        }
    }

    public void CollectLaserBullets(float laserBullets)
    {
        numOfLaserBullets += laserBullets;
        if (numOfLaserBullets >= maxNumOfLaserBullets)
        {
            numOfLaserBullets = maxNumOfLaserBullets;
        }
    }

    public void DealDamage(float damage)
    {
        tank.damageTaken(damage);
    }

    /* Using collision detection script instead */
    //public void DealDamage(float damage)
    //{
    //    if(shieldBoostOn)
    //    {
    //        shieldBoostOn = false;
    //        return;
    //    }
    //    else
    //    {
    //        tank.damageTaken(damage);
    //    }

    //}

    private IEnumerator DelayFire()
    {
        yield return new WaitForSeconds(0.3f);
        fireMechanism.FireBullet();
    }
}