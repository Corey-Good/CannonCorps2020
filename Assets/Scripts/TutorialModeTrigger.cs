using UnityEngine;

public class TutorialModeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        TutorialMode.startTime += -60;
    }
}
