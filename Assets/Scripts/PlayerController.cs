/************************************************************************/
/* Author:             Corey Good                                       */
/* Date Created:       1/27/2020                                        */
/* Last Modified Date: 2/27/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/
using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    #region Variables
    private Vector3 headPosition;
    private Vector3 bodyPosition;
    private Quaternion headRotation;
    private Quaternion bodyRotation;
    private float lagAdjustSpeed = 20f;
    private float timeElapsed = 0f;
    public bool bulletActive = false;

    public Animator fireAnimation;
    public Camera tankCamera;
    public GameObject tankBody;
    public GameObject tankHead;

    private FireMechanism fireMechanism;
    #endregion

    #region Movement Keys
    KeyCode forwardMovement;
    KeyCode backwardMovement;
    KeyCode leftMovement;
    KeyCode rightMovement;
    #endregion

    #region Movement Speeds
    private float movementForce;
    private float movementMultiplier;
    private float rotateMultiplier;
    private float rotateSpeed;
    #endregion  

    public Tank tank;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        #region Key Function Initialization
        forwardMovement  = KeyBindings.forwardKey;
        backwardMovement = KeyBindings.backwardKey;
        leftMovement     = KeyBindings.leftKey;
        rightMovement    = KeyBindings.rightKey;
        #endregion

        //playerState = states.Stationary;

        tank = GameObject.FindGameObjectWithTag("TankClass").GetComponent<Tank>();
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        fireMechanism = GetComponentInChildren<FireMechanism>();

        movementForce = tank.speedMovement;
        movementMultiplier = 1f;
        rotateMultiplier = 8f;
        rotateSpeed = tank.speedRotation;
    }

    // Update is called once per frame
    void Update()
    {

        if (!photonView.IsMine)
        {
            LagAdjust();
            return;
        }

        if (((!PauseMenuAnimations.GameIsPaused) && (!TutorialMode.TutorialModeOn)) || (TutorialMode.tutorialStep > 3))
        {
            MovePlayer();            
        }
        
        if(Input.GetMouseButtonDown(0) && !(PauseMenuAnimations.GameIsPaused) && (!TutorialMode.TutorialModeOn) && !bulletActive)
        {
            bulletActive = true;
            
            if(tank.tankModel == "Catapult")
            {
                StartCoroutine(DelayStart());
            }
            else
            {
                fireMechanism.FireBullet();
            }
            if (fireAnimation != null)
            {
                fireAnimation.SetTrigger("LaunchCatapult");
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            tank.healthCurrent -= 10;
        }
        ReloadBullet();
    }

    private void MovePlayer()
    {
        // Move play forwards and backwards, 
        if (Input.GetKey(forwardMovement))
        {
            transform.position += transform.forward * Time.deltaTime * movementForce * movementMultiplier;
        }
        else if (Input.GetKey(backwardMovement))
        {
            transform.position += -transform.forward * Time.deltaTime * movementForce * movementMultiplier;
        }

        if (Input.GetKey(rightMovement))
        {
            transform.Rotate(Vector3.up * rotateSpeed * rotateMultiplier * Time.deltaTime);
        }
        else if (Input.GetKey(leftMovement))
        {
            transform.Rotate(-Vector3.up * rotateSpeed * rotateMultiplier * Time.deltaTime);
        }
    }

    public void ReloadBullet()
    {
        if (bulletActive)
        {
            // Increase time and update the reloadBar progress
            timeElapsed += Time.deltaTime;
            tank.reloadProgress = timeElapsed / tank.reloadTime;

            // When a bullet is reloaded, reset timer
            if (timeElapsed >= tank.reloadTime)
            {
                timeElapsed = 0f;
                bulletActive = false;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tankBody.transform.position);
            stream.SendNext(tankBody.transform.rotation);
            stream.SendNext(tankHead.transform.position);
            stream.SendNext(tankHead.transform.rotation);
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
        #endregion

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
        #endregion

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
        #endregion

        #region AdjustHeadPosition
        if (headPositionLag.magnitude > 5.0f)
        {
            tankHead.transform.position = headPosition;
            Debug.Log("The Head is being teleported!!");
        }
        else if (headPositionLag.magnitude < 0.11f)
        {
            // do nothing
        }
        else
        {
            Vector3.MoveTowards(tankHead.transform.position, headPosition, lagAdjustSpeed * Time.deltaTime);
        }
        #endregion

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
        #endregion
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.75f);
        fireMechanism.FireBullet();
    }

    public void DealDamage(float damage)
    {
        tank.damageTaken(damage);
    }
}


