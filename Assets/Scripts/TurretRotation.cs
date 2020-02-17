/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       02/14/2020                                       */
/* Last Modified Date:                                                  */
/* Modified By:                                                         */
/************************************************************************/

using UnityEngine;

public class TurretRotation : MonoBehaviour
{
    public GameObject turretObject;

    private void Update()
    {
        if (!PauseMenuAnimations.GameIsPaused)
        {
            turretObject.transform.Rotate(0f, 0, Input.GetAxis("Mouse X") * 300 * Time.deltaTime);
        }
    }
}