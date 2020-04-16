/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       2/14/2020                                        */
/* Last Modified Date: 4/12/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/

#region Libraries

using UnityEngine;

#endregion

public class TurretRotation : MonoBehaviour
{
    public GameObject turretObject;

    // Every frame, sync the turrent rotation to the movement of the mouse
    private void Update()
    {
        if (!PauseMenuManager.gameIsPaused && UIManager.cameraIsEnabled)
        {
            if (KeyBindings.XisInverted)
            {
                turretObject.transform.Rotate(0f, 0, Input.GetAxis("Mouse X") * -300 * Time.deltaTime);
            }
            else
            {
                turretObject.transform.Rotate(0f, 0, Input.GetAxis("Mouse X") * 300 * Time.deltaTime);
            }
        }
    }
}