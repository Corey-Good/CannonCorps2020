/************************************************************************/
/* Author:             Corey Good */
/* Date Created:       1/27/2020  */
/* Last Modified Date: 2/27/2020  */
/* Modified By:        J. Calas   */
/************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    #region Variables
    private Vector3    headPosition;
    private Vector3    bodyPosition;
    private Quaternion headRotation;
    private Quaternion bodyRotation;
    private float      lagAdjustSpeed = 20f;

    public  Animator   fireAnimation;
    public  Camera     tankCamera;
    public  GameObject tankBody;
    public  GameObject tankHead;
    
    #endregion

    #region Movement Keys
    KeyCode forwardMovement ;
    KeyCode backwardMovement;
    KeyCode leftMovement    ;
    KeyCode rightMovement   ;
    #endregion

    #region Movement Speeds
    private float movementForce = 10.0f;
    private float movementMultiplier = 1.0f;
    private float rotateMultiplier = 8.0f;
    private float rotateSpeed = 15.0f;
    #endregion

    #region States
    private enum states
    {
        Stationary, 
        Moving
    }
    states playerState;
    #endregion    


    // Start is called before the first frame update
    void Start()
    {
        #region Key Function Initialization
        if ((forwardMovement  == KeyCode.None) &&
            (backwardMovement == KeyCode.None) &&
            (leftMovement     == KeyCode.None) &&
            (rightMovement    == KeyCode.None))
        {
             forwardMovement   = KeyCode.W;
             backwardMovement  = KeyCode.S;
             leftMovement      = KeyCode.A;
             rightMovement     = KeyCode.D;
        }
        else
        {
             forwardMovement   = KeyBindings.forwardKey;
             backwardMovement  = KeyBindings.backwardKey;
             leftMovement      = KeyBindings.leftKey;
             rightMovement     = KeyBindings.rightKey;
        }
        #endregion

        playerState = states.Stationary;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!photonView.IsMine) 
        {
            LagAdjust();
            return;
        }

        if (Input.anyKey && !(PauseMenuAnimations.GameIsPaused) && (TutorialUI.tutorialStep >= 3))
        {
            MovePlayer();            
        }
        else
        {
            playerState = states.Stationary;
        }  
        
        if(Input.GetMouseButtonDown(0) && !(PauseMenuAnimations.GameIsPaused) && (TutorialMode.ActionRequired) && fireAnimation != null)
        {
            fireAnimation.SetTrigger("LaunchCatapult");
        }

    }

    private void MovePlayer()
    {
        // Move play forwards and backwards, 
        if (Input.GetKey(forwardMovement))
        {
            transform.position += transform.forward * Time.deltaTime * movementForce * movementMultiplier;
            playerState = states.Moving;
        }
        else if (Input.GetKey(backwardMovement))
        {
            transform.position += -transform.forward * Time.deltaTime * movementForce * movementMultiplier;
            playerState = states.Moving;
        }

        if(playerState == states.Moving)
        {
            if (Input.GetKey(rightMovement))
            {
                transform.Rotate(Vector3.up * rotateSpeed * rotateMultiplier * Time.deltaTime);
            }
            else if (Input.GetKey(leftMovement))
            {
                transform.Rotate(-Vector3.up * rotateSpeed * rotateMultiplier * Time.deltaTime);
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
            else if(bodyPositionLag.magnitude < 0.11f)
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
            else if(bodyRotationLag.magnitude < 0.11f)
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
}


