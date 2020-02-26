/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       1/27/2020                                        */
/* Last Modified Date: 2/26/2020                                        */
/* Modified By:        M. Agamalian                                     */
/************************************************************************/

using Photon.Pun;
using UnityEngine;

public class CameraMovement : MonoBehaviourPun /*MonoBehaviourPun*/
{
    private       float cameraTargetOffset = 1.2f;

    public static float cameraRotateSpeed  = 300.0f;
    
    public Camera     tankCamera;
    public Transform  target;
    public Transform  cameraTransform;
    public GameObject player;

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (!photonView.IsMine)
        {
            tankCamera.enabled = false;
            return;
        }
    }

    public void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (!PauseMenuAnimations.GameIsPaused)
        {
            ZoomCamera();
        }
    }

    public void LateUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        SetCameraTarget();
        LookAtCameraTarget();
    }

    public void LookAtCameraTarget()
    {
        cameraTransform.transform.LookAt(target);
    }

    public void SetCameraTarget()
    {
        target.position = new Vector3(player.transform.position.x, 
									 (player.transform.position.y + cameraTargetOffset), 
									  player.transform.position.z);
    }

    public void ZoomCamera()
    {
        float zoomFOV = tankCamera.fieldOfView;
        float zoomDistance = 7.0f;
        float zoomMin = 15.0f;
        float zoomMax = 75.0f;

        zoomFOV -= Input.GetAxis("Mouse ScrollWheel") * zoomDistance;
        zoomFOV = Mathf.Clamp(zoomFOV, zoomMin, zoomMax);

        tankCamera.fieldOfView = zoomFOV;
    }
}