/************************************************************************/
/* Author:             Corey Good                                       */
/* Date Created:       02/12/20                                         */
/* Last Modified Date: 02/13/20                                         */
/* Modified By:        Jaben Calas                                      */
/************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Compilation;

public class TutorialUI : MonoBehaviour
{
    public static int             tutorialStep    = 0;
    public static float           delayTime       = 2.0f;

    public  TextMeshProUGUI headingText;
    public  TextMeshProUGUI subtitleText;
    public  TextMeshProUGUI promptText;

    void   Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            tutorialStep++;
        }
        //Debug.Log(tutorialStep);
    }
    //void   Update()
    //{
    //    GuideText();

    //    //ClearText();

    //    if (ActionRequired)
    //    {
    //        StartCoroutine(DelayedPrompt());

    //        if (Input.anyKeyDown)
    //        {
    //            stepCounter++;
    //            //promptText.gameObject.SetActive(true);
    //            //DisplayPrompt = true;
    //        }

    //        //if (TaskCompleted)
    //        //{
    //        //    promptText.text = "Task complete!";

    //        //    stepCounter++;

    //        //    promptText.gameObject.SetActive(true);

    //        //    DisplayPrompt = false;
    //        //    TaskCompleted = false;
    //        //}
    //    }
    //    else
    //    {
    //        if (Input.anyKeyDown)
    //        {
    //            StartCoroutine(ClearPrompt());
    //            //ActionRequired = true;
    //            stepCounter++;
    //        }

    //    }

    //    //Debug.Log(ActionRequired);
    //}

    void   GuideText()
    {
        switch (tutorialStep)
        {
            case 0:
                headingText.text   = "Welcome to the training camp!";
                subtitleText.text  = "Here you will learn the basic skills required in battle.";
                break;

            case 1:
                headingText.text  = "Use your mouse wheel to zoom in/out.";
                subtitleText.text = "You can adjust it to your preference.";
                break;

            case 2:
                headingText.text   = "Use your mouse to control the camera.";
                subtitleText.text  = "The turret follows the camera as you move it.";
                break;

            case 3:
                headingText.text   = string.Format(
                                     "Use the {0}, {1}, {2}, and {3} keys to control your vehicle.",
                                        KeyBindings.forwardKey,  KeyBindings.leftKey, 
                                        KeyBindings.backwardKey, KeyBindings.rightKey);
                subtitleText.text = "";
                break;

            case 4:
                headingText.text   = "Move to the designated location.";
                subtitleText.text = "";
                break;

            case 5:
                headingText.text   = "CONGRATULATIONS!";
                subtitleText.text  = "You have successfully completed the tutorial.";
                promptText.gameObject.SetActive(false);
                break;
        }
    }

    public IEnumerator ClearPrompt()
    {
        promptText.gameObject.SetActive(false);
        promptText.text = "";
        yield return new WaitForSecondsRealtime(0);
        promptText.gameObject.SetActive(true);
    }

    public IEnumerator DelayedPrompt()
    {
        yield return new WaitForSecondsRealtime(delayTime);
        promptText.text = "Press any key to continue.";
    }

}