/************************************************************************/
/* Author:             Corey Good */
/* Date Created:       1/27/2020  */
/* Last Modified Date: 1/27/2020  */
/* Modified By:        C. Good    */
/************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    #region Variables
    private Vector3   cursorPosition;
    public  Animator  fireAnimation;
    public  Camera    tankCamera;
    public  GameObject tankHead;
    public  GameObject tankBase;

    #endregion

    #region Movement Keys
    KeyCode forwardMovement;
    KeyCode backwardMovement;
    KeyCode leftMovement;
    KeyCode rightMovement;
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
        forwardMovement  = KeyCode.W;
        backwardMovement = KeyCode.S;
        leftMovement     = KeyCode.A;
        rightMovement    = KeyCode.D;
        playerState      = states.Stationary;
    }

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine) { return; }

        if (Input.anyKey && !(PauseMenuAnimations.GameIsPaused))
        {
            MovePlayer();            
        }
        else
        {
            playerState = states.Stationary;
        }  
        
        if(Input.GetMouseButtonDown(0) && !(PauseMenuAnimations.GameIsPaused) && fireAnimation != null)
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
        if(stream.IsWriting)
        {
            stream.SendNext(tankHead.transform.position);
            stream.SendNext(tankBase.transform.position);
        }
        else
        {
            tankHead.transform.position = (Vector3)stream.ReceiveNext();
            tankBase.transform.position = (Vector3)stream.ReceiveNext();
        }

    }
}


