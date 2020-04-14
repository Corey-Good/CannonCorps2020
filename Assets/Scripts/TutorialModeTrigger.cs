/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       3/03/2020                                        */
/* Last Modified Date: 3/30/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/

using UnityEngine;

public class TutorialModeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerGO" && this.gameObject.name == "Panel")
        {
            TutorialPrompts.step5 = true;
            Destroy(this.gameObject);
        }
        else if (other.tag == "Bullet" && this.gameObject.name == "Block")
        {
            TutorialPrompts.step6 = true;
        }
    }
}