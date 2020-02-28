using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMode : MonoBehaviour
{
    public static bool ActionRequired = false;
    public static bool TaskIsComplete = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GuideActions()
    {
        switch (TutorialUI.tutorialStep)
        {
            case 0:

                break;
            case 1:

                break;

            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject);
    }


}
