/************************************************************************/
/* Author:             Corey Good                                       */
/* Date Created:       02/12/20                                         */
/* Last Modified Date: 02/13/20                                         */
/* Modified By:        Jaben Calas                                      */
/************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class KeyBindings : MonoBehaviour
{
    #region Variables
    public   static bool     CustomKeys      = false;
    private  bool            lookingForKey   = false;
    private  bool            lookingForClick = false;

    public   static int      clickIndex      = 0;

    public   static KeyCode  forwardKey      =  KeyCode.W;
    public   static KeyCode  backwardKey     =  KeyCode.S;
    public   static KeyCode  leftKey         =  KeyCode.A;
    public   static KeyCode  rightKey        =  KeyCode.D;

    private  string          keyHit;
    private  string          objectName;

    public   TextMeshProUGUI forwardButton;
    public   TextMeshProUGUI backwardButton;
    public   TextMeshProUGUI leftButton;
    public   TextMeshProUGUI rightButton;
    public   TextMeshProUGUI fireButton;
    #endregion

    private void Start()
    {
        #region Key Text Initialization
        forwardButton. text = forwardKey. ToString();
        backwardButton.text = backwardKey.ToString();
        leftButton.    text = leftKey.    ToString();
        rightButton.   text = rightKey.   ToString();
        #endregion

        //Note: Modify to accommodate FiringMechanism
        if (clickIndex == 0)
            fireButton.text = "LeftClick";
        else if (clickIndex == 1)
            fireButton.text = "RightClick";
    }

    public  void OnClick(TextMeshProUGUI text)
    {
        text.text       = "Hit Key";
        lookingForKey   = true;
        objectName      = text.name;
    }

    public  void OnMouseClick(TextMeshProUGUI text)
    {
        text.text       = "Click Mouse";
        lookingForClick = true;
        objectName      = text.name;
    }

    public  void OnGUI()
    {
        Event e = Event.current;

        // Change the movement keys
        if (e.isKey && lookingForKey)
        {
            e.keyCode = getKeyPress();
            keyHit    = e.keyCode.ToString();

            switch (objectName)
            {
                case "ForwardText":
                    forwardButton.text  = keyHit;
                    forwardKey          = e.keyCode;
                    break;

                case "BackwardText":
                    backwardButton.text = keyHit;
                    backwardKey         = e.keyCode;
                    break;

                case "LeftText":
                    leftButton.text     = keyHit;
                    leftKey             = e.keyCode;
                    break;

                case "RightText":
                    rightButton.text    = keyHit;
                    rightKey            = e.keyCode;
                    break;
            }
            lookingForKey = false;
            CustomKeys = true;
        }

        //Note: Modify to accommodate FiringMechanism
        if (e.isMouse && lookingForClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                clickIndex = 0;
                fireButton.text = "LeftClick";
            }
            else if (Input.GetMouseButtonDown(1))
            {
                clickIndex = 1;
                fireButton.text = "RightClick";
            }
            lookingForClick = false;
            CustomKeys = true;
        }
    }
    KeyCode getKeyPress()
    {
        KeyCode keyHit = KeyCode.None;

        foreach (KeyCode getKey in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(getKey))
            {
                keyHit = getKey;
            }
        }

        return keyHit;
    }
}