/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       1/27/2020                                        */
/* Last Modified Date: 2/26/2020                                        */
/* Modified By:        M. Agamalian                                     */
/************************************************************************/
using UnityEngine;
using Photon.Pun;

public class CameraMovement : MonoBehaviourPun
{
    #region Variables
    private float cameraTargetOffset = 1.2f;

    public static float cameraRotateSpeed  = 300.0f;
    
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
            ZoomCamera();
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
        cameraTransform.transform.LookAt(target);
    }

    // Set the camera at the appropriate distance from the target
    public void SetCameraTarget()
    {
        target.position = new Vector3(player.transform.position.x, 
									 (player.transform.position.y + cameraTargetOffset), 
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