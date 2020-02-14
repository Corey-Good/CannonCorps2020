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

public class PlayerController : MonoBehaviourPun
{
    #region Variables
    private Vector3   cursorPosition;
    public  Animator  fireAnimation;
    public  Camera    tankCamera;
    public  GameObject tankHead;

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

            if(tankHead != null) 
            { 
                TurretRotation(); 
            }
            
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

    private void TurretRotation()
    {
        //Debug.Log(Input.GetAxis("Mouse X"));
        //Vector3 turretFinalLookDirection = new Vector3();
        //Ray screenRay = tankCamera.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(screenRay, out hit))
        //{
        //    cursorPosition = hit.point;
        //}
        Debug.Log(Input.mousePosition);
        //Vector3 turretLookDirection = cursorPosition - tankHead.position;
        //turretLookDirection.y = 0.0f;


        //turretFinalLookDirection = Vector3.RotateTowards(turretFinalLookDirection, turretLookDirection, 0.7f * Time.deltaTime, 10.0f);
        //tankHead.rotation = Quaternion.LookRotation(turretFinalLookDirection);

        //if (Input.GetKey(KeyCode.Q))
        //{
        //    //tankHead.LeanRotate(new Vector3(tankHead.rotation.x, tankHead.rotation.y - 5.0f, tankHead.rotation.z), 10.0f);
        //    LeanTween.rotateAround(tankHead, Vector3.down, 15f, 1f).setEase(LeanTweenType.once);
        //}
        //if (Input.GetKey(KeyCode.E))
        //{
        //    //tankHead.LeanRotate(new Vector3(tankHead.rotation.x, tankHead.rotation.y - 5.0f, tankHead.rotation.z), 10.0f);
        //    LeanTween.rotateAround(tankHead, Vector3.up, 15f, 1f).setEase(LeanTweenType.once);
        //}
    }

}


