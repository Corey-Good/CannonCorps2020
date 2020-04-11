/************************************************************************/
/* Author:             Corey Good                                       */
/* Date Created:       2/12/2020                                        */
/* Last Modified Date: 4/11/2020                                        */
/* Modified By:        J. Calas                                         */
/************************************************************************/

#region Libraries

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#endregion

public class KeyBindings : MonoBehaviour
{
    #region Variables

    public  static bool     customKeys            = false;
    public  static bool     XisInverted           = false;
    public  static bool     YisInverted           = false;

    public  static int      clickIndex            = 0;

    public  static KeyCode  forwardKey            = KeyCode.W;
    public  static KeyCode  backwardKey           = KeyCode.S;
    public  static KeyCode  leftKey               = KeyCode.A;
    public  static KeyCode  rightKey              = KeyCode.D;

    public  static KeyCode  switchBulletType      = KeyCode.Space;  // Powerup Keys
    public  static KeyCode  activateReloadBoost   = KeyCode.Alpha1; // Powerup Keys
    public  static KeyCode  activateMovementBoost = KeyCode.Alpha2; // Powerup Keys

    public  GameObject      closeButton;

    public  TextMeshProUGUI forwardButton;
    public  TextMeshProUGUI backwardButton;
    public  TextMeshProUGUI leftButton;
    public  TextMeshProUGUI rightButton;
    public  TextMeshProUGUI switchButton;
    public  TextMeshProUGUI reloadButton;
    public  TextMeshProUGUI speedButton;
    public  TextMeshProUGUI invertXButton;
    public  TextMeshProUGUI invertYButton;

    private bool            lookingForKey         = false;

    private string          objectName;

    private List<KeyCode>   currentKeys           = new List<KeyCode>() { forwardKey, backwardKey, leftKey, rightKey, 
                                                                          switchBulletType, activateReloadBoost, activateMovementBoost};

    #endregion Variables

    private void Start()
    {
        #region Initializes key text

        forwardButton. text = alphaFilter(forwardKey);
        backwardButton.text = alphaFilter(backwardKey);
        leftButton.    text = alphaFilter(leftKey);
        rightButton.   text = alphaFilter(rightKey);
        switchButton.  text = alphaFilter(switchBulletType);
        reloadButton.  text = alphaFilter(activateReloadBoost);
        speedButton.   text = alphaFilter(activateMovementBoost);
        invertXButton. text = XisInverted ? "INVERTED" : "NORMAL";
        invertYButton. text = YisInverted ? "INVERTED" : "NORMAL";

        #endregion
    }

    private void Update()
    {
        #region Prevents same key assignment

        if (CannotLeave())
            closeButton.SetActive(false);
        else
            closeButton.SetActive(true);

        #endregion
    }

    public void OnClick(TextMeshProUGUI text)
    {
        text.text     = "Hit Key";
        lookingForKey = true;
        objectName    = text.name;
    }

    public void InvertX()
    {
        if (XisInverted)
        {
            XisInverted = false;
            invertXButton.text = "NORMAL";
        }
        else
        {
            XisInverted = true;
            invertXButton.text = "INVERTED";
        }
    }

    public void InvertY()
    {
        if (YisInverted)
        {
            YisInverted = false;
            invertYButton.text = "NORMAL";
        }
        else
        {
            YisInverted = true;
            invertYButton.text = "INVERTED";
        }
    }

    public void SetPrimaryFire(TMP_Dropdown dropdown)
    {
        clickIndex = dropdown.value;
    }

    public bool CannotLeave()
    {
        return currentKeys.Contains(KeyCode.None);
    }

    public void OnGUI()
    {
        Event e = Event.current;

        // Change the movement keys
        if (e.isKey && lookingForKey)
        {
            e.keyCode = getKeyPress();
            if (currentKeys.Contains(e.keyCode))
            {
                //Set warning text = Key is already used
                switch (currentKeys.IndexOf(e.keyCode))
                {
                    case 0:
                        forwardButton.text    = "None";
                        forwardKey            = KeyCode.None;
                        currentKeys[0]        = forwardKey;
                        break;                

                    case 1:                   
                        backwardButton.text   = "None";
                        backwardKey           = KeyCode.None;
                        currentKeys[1]        = backwardKey;
                        break;                

                    case 2:                   
                        leftButton.text       = "None";
                        leftKey               = KeyCode.None;
                        currentKeys[2]        = leftKey;
                        break;                

                    case 3:                   
                        rightButton.text      = "None";
                        rightKey              = KeyCode.None;
                        currentKeys[3]        = rightKey;
                        break;                

                    case 4:                   
                        switchButton.text     = "None";
                        switchBulletType      = KeyCode.None;
                        currentKeys[4]        = switchBulletType;
                        break;                

                    case 5:                   
                        reloadButton.text     = "None";
                        activateReloadBoost   = KeyCode.None;
                        currentKeys[5]        = activateReloadBoost;
                        break;

                    case 6:
                        speedButton.text      = "None";
                        activateMovementBoost = KeyCode.None;
                        currentKeys[6]        = activateMovementBoost;
                        break;
                }
            }

            switch (objectName)
            {

                case "ForwardText":
                    forwardButton.text    = alphaFilter(e.keyCode);
                    forwardKey            = e.keyCode;
                    currentKeys[0]        = forwardKey;
                    break;

                case "BackwardText":
                    backwardButton.text   = alphaFilter(e.keyCode);
                    backwardKey           = e.keyCode;
                    currentKeys[1]        = backwardKey;
                    break;

                case "LeftText":
                    leftButton.text       = alphaFilter(e.keyCode);
                    leftKey               = e.keyCode;
                    currentKeys[2]        = leftKey;
                    break;

                case "RightText":
                    rightButton.text      = alphaFilter(e.keyCode);
                    rightKey              = e.keyCode;
                    currentKeys[3]        = rightKey;
                    break;

                case "SwitchText":
                    switchButton.text     = alphaFilter(e.keyCode);
                    switchBulletType      = e.keyCode;
                    currentKeys[4]        = switchBulletType;
                    break;

                case "ReloadText":
                    reloadButton.text     = alphaFilter(e.keyCode);
                    activateReloadBoost   = e.keyCode;
                    currentKeys[5]        = activateReloadBoost;
                    break;

                case "SpeedText":
                    speedButton.text      = alphaFilter(e.keyCode);
                    activateMovementBoost = e.keyCode;
                    currentKeys[6]        = activateMovementBoost;
                    break;
            }
            lookingForKey = false;
            customKeys    = true;
        }
    }

    private KeyCode getKeyPress()
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

    private string alphaFilter(KeyCode oldKeyPress)
    {
        string newKeyPress = oldKeyPress.ToString();

        if (newKeyPress.Contains("Alpha"))
            newKeyPress = newKeyPress.Replace("Alpha", "");

        return newKeyPress;
    }
}