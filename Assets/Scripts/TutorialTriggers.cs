/************************************************************************/
/* Author:             Corey Good                                       */
/* Date Created:       3/28/2020                                        */
/* Last Modified Date: 4/13/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/
#region Libraries

using UnityEngine;

#endregion
public class TutorialTriggers : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerGO" && this.gameObject.name == "Trigger (1)")
        {
            UIManager.tutorialSteps[4] = true;
            Destroy(this.gameObject);
        }
        if (other.tag == "Bullet"   && this.gameObject.name == "Trigger (2)")
        {
            UIManager.tutorialSteps[5] = true;
            Destroy(this.gameObject);
        }
    }
}