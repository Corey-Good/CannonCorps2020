/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       02/14/2020                                       */
/* Last Modified Date: 02/26/2020                                       */
/* Modified By:        M. Agamalian                                     */
/************************************************************************/

using UnityEngine;

public class TurretRotation : MonoBehaviour
{
    public GameObject turretObject;

    // Every frame, sync the turrent rotation to the movement of the mouse
    private void Update()
    {
        if ((!PauseMenuAnimations.GameIsPaused) && (!TutorialMode.TutorialModeOn)) //(TutorialMode.tutorialStep >= 2))
        {
            turretObject.transform.Rotate
            (0f, 0, Input.GetAxis("Mouse X") * 300 * Time.deltaTime);
        }
    }
}