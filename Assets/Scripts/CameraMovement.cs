/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       1/27/2020                                        */
/* Last Modified Date: 3/6/2020                                        */
/* Modified By:        Corey Good                                     */
/************************************************************************/
using UnityEngine;
using Photon.Pun;

public class CameraMovement : MonoBehaviourPun
{
    #region Variables
    private float cameraTargetOffset = 1.2f;

    public static float cameraRotateSpeed  = 300.0f;
    public float verticalSensitivity = 0.15f;
    
    public Camera     tankCamera;
    public Transform  target;
    public Transform  cameraTransform;
    public GameObject player;
    #endregion

    // Lock the cursor from moveing and disable the tank camera view on start
    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (!photonView.IsMine)
        {
            tankCamera.GetComponent<AudioListener>().gameObject.SetActive(false);
            tankCamera.enabled = false;
            return;
        }
    }

    // Check that the camera is in the correct mode
    public void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (((!PauseMenuAnimations.GameIsPaused) && (!TutorialMode.TutorialModeOn)) || (TutorialMode.tutorialStep > 1))
        {
            //ZoomCamera();
        }
    }

    // Ensure camera is pointing at right target
    public void LateUpdate()
    {
        if (!photonView.IsMine)
            return;
        SetCameraTarget();
        LookAtCameraTarget();
    }

    // Sometimes, your code documents itself
    public void LookAtCameraTarget()
    {
        float mouseInput = Input.GetAxis("Mouse Y") * verticalSensitivity;
        // Set the new target position height based on mouse input 
        if(!(PauseMenuAnimations.GameIsPaused))
        { 
            target.transform.position = 
                new Vector3(target.transform.position.x, 
                            target.transform.position.y + mouseInput, 
                            target.transform.position.z);
        }


        // Adjust the camera rotation
        cameraTransform.transform.LookAt(target);        
    }

    // Set the camera at the appropriate distance from the target
    public void SetCameraTarget()
    {
        // Move the target position
        target.position = new Vector3(player.transform.position.x, 
									 (target.transform.position.y), 
									  player.transform.position.z);
    }

    // Set the zoom paramaters for the camera
    public void ZoomCamera()
    {
        float zoomFOV      = tankCamera.fieldOfView;
        float zoomDistance = 7.0f;
        float zoomMin      = 15.0f;
        float zoomMax      = 75.0f;

        zoomFOV -= Input.GetAxis("Mouse ScrollWheel") * zoomDistance;
        zoomFOV  = Mathf.Clamp(zoomFOV, zoomMin, zoomMax);

        tankCamera.fieldOfView = zoomFOV;
    }
}