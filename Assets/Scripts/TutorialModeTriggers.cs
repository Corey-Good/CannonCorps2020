using UnityEngine;

public class TutorialModeTriggers : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        TutorialMode.currentStep = 5;
    }
}
