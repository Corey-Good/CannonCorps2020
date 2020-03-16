/************************************************************************/
/* Author:  */
/* Date Created: */
/* Last Modified Date: */
/* Modified By: */
/************************************************************************/

using UnityEngine;

public class TutorialModeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerGO" && this.gameObject.name == "Panel")
        {
            TutorialMode.startTime -= 60;
            Destroy(this.gameObject);
        }
        else if (other.tag == "Bullet" && this.gameObject.name == "Block")
        {
            TutorialMode.startTime -= 120;
        }
    }
}