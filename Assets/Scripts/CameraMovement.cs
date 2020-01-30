/************************************************************************/
/* Author:            Jaben Calas*/
/* Date Created:       1/27/2020 */
/* Last Modified Date: 1/27/2020 */
/* Modified By:        C. Good   */
/************************************************************************/

//using Photon.Pun;
using UnityEngine;

public class CameraMovement : MonoBehaviour /*MonoBehaviourPun*/
{
    public static float cameraRotateSpeed = 500.0f;
    public Transform cameraTransform;
    public GameObject player;

    public float cameraTargetOffset = 1.2f;
    //same as tankCameraC, needed for transform.LookAt and transform.RotateAround
    public Camera tankCamera;

    public Transform target;
    //same as tankCameraT, needed for camera.fieldOfView
    public void Awake()
    {

        Cursor.lockState = CursorLockMode.Locked;
        /*if (!photonView.IsMine)
        {
            tankCamera.enabled = true;
            return;
        }  Note, keep this to disable all cameras if it does not belong to the player*/
    }

    public void FixedUpdate()
    {
        ZoomCamera();
    }

    public void LateUpdate()
    {
        SetCameraTarget();
        OrbitCamera();
        LookAtCameraTarget();
    }

    public void LookAtCameraTarget()
    {
        cameraTransform.transform.LookAt(target);
    }

    public void OrbitCamera()
    {
        float orbitSpeed = Input.GetAxis("Mouse X") * cameraRotateSpeed * Time.deltaTime;

        cameraTransform.transform.RotateAround(player.transform.position, Vector3.up, orbitSpeed);
    }

    public void SetCameraTarget()
    {      

        target.position = new Vector3(player.transform.position.x, (player.transform.position.y + cameraTargetOffset), player.transform.position.z);
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
