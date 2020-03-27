/************************************************************************/
/* Author:             Corey Good                                       */
/* Date Created:       02/12/20                                         */
/* Last Modified Date: 02/13/20                                         */
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

    private bool lookingForKey = false;

    public static int clickIndex = 0;
    public static KeyCode forwardKey = KeyCode.W;
    public static KeyCode backwardKey = KeyCode.S;
    public static KeyCode leftKey = KeyCode.A;
    public static KeyCode rightKey = KeyCode.D;
    private List<KeyCode> currentKeys = new List<KeyCode>() { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };

    private string keyHit;
    private string objectName;

    public TextMeshProUGUI forwardButton;
    public TextMeshProUGUI backwardButton;
    public TextMeshProUGUI leftButton;
    public TextMeshProUGUI rightButton;
    public TextMeshProUGUI invertXButton;
    public TextMeshProUGUI invertYButton;

    public GameObject closeButton;

    #endregion Variables

    private void Start()
    {
        #region Key Text Initialization

        forwardButton.text = forwardKey.ToString();
        backwardButton.text = backwardKey.ToString();
        leftButton.text = leftKey.ToString();
        rightButton.text = rightKey.ToString();

        #endregion Key Text Initialization

        invertXButton.text = XisInverted ? "INVERTED" : "NORMAL";
        invertYButton.text = YisInverted ? "INVERTED" : "NORMAL";
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
                }
            }

            keyHit = e.keyCode.ToString();

            switch (objectName)
            {
                case "ForwardText":
                    forwardButton.text = keyHit;
                    forwardKey = e.keyCode;
                    currentKeys[0] = forwardKey;
                    break;

                case "BackwardText":
                    backwardButton.text = keyHit;
                    backwardKey = e.keyCode;
                    currentKeys[1] = backwardKey;
                    break;

                case "LeftText":
                    leftButton.text = keyHit;
                    leftKey = e.keyCode;
                    currentKeys[2] = leftKey;
                    break;

                case "RightText":
                    rightButton.text = keyHit;
                    rightKey = e.keyCode;
                    currentKeys[3] = rightKey;
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