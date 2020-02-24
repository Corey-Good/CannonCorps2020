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
    public List<GameObject> tutorialText = new List<GameObject>();

    public KeyCode nextKey;
    public KeyCode prevKey;
    private int   stepCounter = 0;

    private float promptDelay = 2.0f;

    public TextMeshProUGUI movementKeys;

    public TextMeshProUGUI mainText;
    public TextMeshProUGUI subText;

    public GameObject moveHere;
    public GameObject lookHere;

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
                mainText.text = sentences[0]; // Welcome to the training camp!
                subText.text  = sentences[1]; // Here you will learn the basic skills required in battle.
                Invoke("PressToContinue", promptDelay);
                break;

            case 1:
                mainText.text = sentences[2]; // Use your mouse to control the camera.
                subText.text  = sentences[3]; // The turret follows the camera as you move it.
                Invoke("PressToContinue", promptDelay);
                break;

            case 2:
                mainText.text = GetKeys(); // Use the W, A, S, and D keys to control your vehicle.
                Invoke("PressToContinue", promptDelay);
                break;

            case 3:
                mainText.text = sentences[6]; // Move to the designated location.
                Invoke("PressToContinue", promptDelay);
                break;

            case 4:
                mainText.text = sentences[8]; // CONGRATULATIONS!
                subText.text  = sentences[9]; // You have successfully completed the tutorial.
                Invoke("PressToContinue", promptDelay);
                break;
        }
    }
    string GetKeys()
    {
        string keybindings;

        keybindings = string.Format("Use the {0}, {1}, {2}, and {3} keys to control your vehicle.", 
            KeyBindings.forwardKey, KeyBindings.leftKey, KeyBindings.backwardKey, KeyBindings.rightKey);

        return keybindings;
    }
    #endregion

    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.collider.tag == "TutorialUI")
        {
            if(collisionInfo.collider.gameObject == moveHere)
            {

            }
        }
    }
}