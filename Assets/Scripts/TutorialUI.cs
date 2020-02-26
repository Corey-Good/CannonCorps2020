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

public class TutorialUI : MonoBehaviour
{
    public  KeyCode         nextKey;
    public  KeyCode         prevKey;

    private int             stepCounter   = 0;
    private float           promptDelay   = 2.0f;

    public  TextMeshProUGUI headingText;
    public  TextMeshProUGUI subtitleText;
    public  TextMeshProUGUI actionText;

    [TextArea(2, 10)]
    public string[] sentences;

    void   Start()
    {

    }
    void   Update()
    {
        if ((Input.GetKeyDown(nextKey)))
        {
            stepCounter++;
        }
        if ((Input.GetKeyDown(prevKey)))
        {
            stepCounter--;
        }
        ShowAllSteps();
    }

    #region TextUI
    void   ShowAllSteps()
    {
        switch (stepCounter)
        {
            case 0:
                headingText.text = sentences[0]; // Welcome to the training camp!
                subtitleText.text  = sentences[1]; // Here you will learn the basic skills required in battle.
                Invoke("PressKeyToContinue", promptDelay);
                break;

            case 1:
                headingText.text = sentences[2]; // Use your mouse to control the camera.
                subtitleText.text  = sentences[3]; // The turret follows the camera as you move it.
                Invoke("PressKeyToContinue", promptDelay);
                break;

            case 2:
                headingText.text = GetKeys(); // Use the W, A, S, and D keys to control your vehicle.
                Invoke("PressKeyToContinue", promptDelay);
                break;

            case 3:
                headingText.text = sentences[6]; // Move to the designated location.
                Invoke("PressKeyToContinue", promptDelay);
                break;

            case 4:
                headingText.text = sentences[8]; // CONGRATULATIONS!
                subtitleText.text  = sentences[9]; // You have successfully completed the tutorial.
                Invoke("PressKeyToContinue", promptDelay);
                break;
        }
    }
    #endregion
    string GetKeys()
    {
        string keybindings = string.Format("Use the {0}, {1}, {2}, and {3} keys to control your vehicle.",
            KeyBindings.forwardKey, KeyBindings.leftKey, KeyBindings.backwardKey, KeyBindings.rightKey);

        return keybindings;
    }

    void PressKeyToContinue()
    {
        if (Input.anyKeyDown)
        {
            actionText.text = sentences[13]; // Completed!
        }
        else
        {
            actionText.text = sentences[12]; // Press any key to continue.
        }
    }
}