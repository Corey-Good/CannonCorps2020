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
    private int stepCounter = 0;
    private float promptDelay = 2.0f;
    public TextMeshProUGUI movementKeys;

    void Start()
    {
        ShowAllSteps();

        movementKeys.text = GetKeys();
    }
    void Update()
    {
        if ((Input.GetKeyDown(nextKey)))
        {
            stepCounter++;
            TurnOffText();
            ShowAllSteps();
        }

        if ((Input.GetKeyDown(prevKey)))
        {
            stepCounter--;
            TurnOffText();
            ShowAllSteps();
        }
        //Debug.Log(stepCounter);
    }
    void ShowAllSteps()
    {
        switch (stepCounter)
        {
            case 0:
                Welcome();
                Invoke("PressToContinue", promptDelay);
                break;

            case 1:
                ControlMouse();
                Invoke("PressToContinue", promptDelay);
                break;

            case 2:
                ControlKeys();
                Invoke("PressToContinue", promptDelay);
                break;

            case 3:
                Move();
                Invoke("PressToContinue", promptDelay);
                break;

            case 4:
                Congrats();
                Invoke("PressToContinue", promptDelay);
                break;
        }
    }
    void TurnOffText()
    {
        tutorialText[0].SetActive(false); // Press any key to continue.
        tutorialText[1].SetActive(false); // Completed!
        tutorialText[2].SetActive(false); // Welcome to the training camp!
        tutorialText[3].SetActive(false); // Here you will learn the basic skills required in battle.
        tutorialText[4].SetActive(false); // Use your mouse to control the camera.
        tutorialText[5].SetActive(false); // The turret follows the camera as you move it.
        tutorialText[6].SetActive(false); // Use the W, A, S, and D keys to control your vehicle.
        tutorialText[7].SetActive(false); // Move to the designated location.
        tutorialText[8].SetActive(false); // CONGRATULATIONS!
        tutorialText[9].SetActive(false); // You have successfully completed the tutorial.
    }
    void PressToContinue()
    {
        tutorialText[0].SetActive(true); // Press any key to continue.
    }
    void Completed()
    {
        tutorialText[1].SetActive(true); // Completed!
    }
    void Welcome()
    {
        tutorialText[2].SetActive(true); // Welcome to the training camp!
        tutorialText[3].SetActive(true); // Here you will learn the basic skills required in battle.
    }
    void ControlMouse()
    {
        tutorialText[4].SetActive(true); // Use your mouse to control the camera.
        tutorialText[5].SetActive(true); // The turret follows the camera as you move it.
    }
    void ControlKeys()
    {
        tutorialText[6].SetActive(true); // Use the W, A, S, and D keys to control your vehicle.
    }
    void Move()
    {
        tutorialText[7].SetActive(true); // Move to the designated location.
    }
    void Congrats()
    {
        tutorialText[8].SetActive(true); // CONGRATULATIONS!
        tutorialText[9].SetActive(true); // You have successfully completed the tutorial.
    }

    string GetKeys()
    {
        string keybindings;

        keybindings = string.Format("Use the {0}, {1}, {2}, and {3} keys to control your vehicle.", 
            KeyBindings.forwardKey, KeyBindings.leftKey, KeyBindings.backwardKey, KeyBindings.rightKey);

        return keybindings;
    }
}