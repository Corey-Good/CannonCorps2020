/************************************************************************/
/* Author:             Corey Good                                       */
/* Date Created:       2/12/2020                                        */
/* Last Modified Date: 4/06/2020                                        */
/* Modified By:        Jaben Calas                                      */
/************************************************************************/

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyBindings : MonoBehaviour
{
    #region Variables

    public static bool CustomKeys = false;
    public static bool XisInverted = false;
    public static bool YisInverted = false;
    public static int clickIndex = 0;
    public static KeyCode forwardKey = KeyCode.W;
    public static KeyCode backwardKey = KeyCode.S;
    public static KeyCode leftKey = KeyCode.A;
    public static KeyCode rightKey = KeyCode.D;
    public static KeyCode switchBulletType = KeyCode.Space;  // Powerup Keys
    public static KeyCode activateReloadBoost = KeyCode.Alpha1; // Powerup Keys
    public static KeyCode activateMovementBoost = KeyCode.Alpha2; // Powerup Keys
    public GameObject closeButton;
    public TextMeshProUGUI forwardButton;
    public TextMeshProUGUI backwardButton;
    public TextMeshProUGUI leftButton;
    public TextMeshProUGUI rightButton;
    public TextMeshProUGUI invertXButton;
    public TextMeshProUGUI invertYButton;
    public TextMeshProUGUI switchButton;
    public TextMeshProUGUI reloadButton;
    public TextMeshProUGUI speedButton;
    private bool lookingForKey = false;
    private string objectName;
    private List<KeyCode> currentKeys = new List<KeyCode>() { forwardKey, backwardKey, leftKey, rightKey, 
                                                              switchBulletType, activateReloadBoost, activateMovementBoost};

    #endregion Variables

    private void Start()
    {
        #region Key Text Initialization

        forwardButton.text = forwardKey.ToString();
        backwardButton.text = backwardKey.ToString();
        leftButton.text = leftKey.ToString();
        rightButton.text = rightKey.ToString();
        invertXButton.text = XisInverted ? "INVERTED" : "NORMAL";
        invertYButton.text = YisInverted ? "INVERTED" : "NORMAL";
        switchButton.text = switchBulletType.ToString();
        reloadButton.text = activateReloadBoost.ToString();
        speedButton.text = activateMovementBoost.ToString();

        #endregion Key Text Initialization
    }

    private void Update()
    {
        if (CannotLeave())
        {
            closeButton.SetActive(false);
        }
        else
        {
            closeButton.SetActive(true);
        }
    }

    public void OnClick(TextMeshProUGUI text)
    {
        text.text = "Hit Key";
        lookingForKey = true;
        objectName = text.name;
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
                        forwardButton.text = "NONE";
                        forwardKey = KeyCode.None;
                        currentKeys[0] = forwardKey;
                        break;

                    case 1:
                        backwardButton.text = "NONE";
                        backwardKey = KeyCode.None;
                        currentKeys[1] = backwardKey;
                        break;

                    case 2:
                        leftButton.text = "NONE";
                        leftKey = KeyCode.None;
                        currentKeys[2] = leftKey;
                        break;

                    case 3:
                        rightButton.text = "NONE";
                        rightKey = KeyCode.None;
                        currentKeys[3] = rightKey;
                        break;

                    case 4:
                        switchButton.text = "NONE";
                        switchBulletType = KeyCode.None;
                        currentKeys[4] = switchBulletType;
                        break;

                    case 5:
                        reloadButton.text = "NONE";
                        activateReloadBoost = KeyCode.None;
                        currentKeys[5] = activateReloadBoost;
                        break;

                    case 6:
                        speedButton.text = "NONE";
                        activateMovementBoost = KeyCode.None;
                        currentKeys[6] = activateMovementBoost;
                        break;
                }
            }

            switch (objectName)
            {
                case "ForwardText":
                    forwardButton.text = e.keyCode.ToString();
                    forwardKey = e.keyCode;
                    currentKeys[0] = forwardKey;
                    break;

                case "BackwardText":
                    backwardButton.text = e.keyCode.ToString();
                    backwardKey = e.keyCode;
                    currentKeys[1] = backwardKey;
                    break;

                case "LeftText":
                    leftButton.text = e.keyCode.ToString();
                    leftKey = e.keyCode;
                    currentKeys[2] = leftKey;
                    break;

                case "RightText":
                    rightButton.text = e.keyCode.ToString();
                    rightKey = e.keyCode;
                    currentKeys[3] = rightKey;
                    break;

                case "SwitchText":
                    switchButton.text = e.keyCode.ToString();
                    switchBulletType = e.keyCode;
                    currentKeys[4] = switchBulletType;
                    break;

                case "ReloadText":
                    reloadButton.text = e.keyCode.ToString();
                    activateReloadBoost = e.keyCode;
                    currentKeys[5] = activateReloadBoost;
                    break;

                case "SpeedText":
                    speedButton.text = e.keyCode.ToString();
                    activateMovementBoost = e.keyCode;
                    currentKeys[6] = activateMovementBoost;
                    break;
            }
            lookingForKey = false;
            CustomKeys = true;
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
}