/************************************************************************/
/* Author:             Corey Good                                       */
/* Date Created:       02/12/20                                         */
/* Last Modified Date: 02/13/20                                         */
/* Modified By:        Jaben Calas                                      */
/************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBindings : MonoBehaviour
{
    private  bool            lookingForKey   = false;
    private  bool            lookingForClick = false;

    public   static int      clickIndex      = 0;
                             
    public   static string   forwardKey      = "W";
    public   static string   backwardKey     = "S";
    public   static string   leftKey         = "A";
    public   static string   rightKey        = "D";
                             
    private  string          keyHit;
    private  string          objectName;

    public   TextMeshProUGUI forwardButton;
    public   TextMeshProUGUI backwardButton;
    public   TextMeshProUGUI leftButton;
    public   TextMeshProUGUI rightButton;
    public   TextMeshProUGUI fireButton;

    private void Start()
    {
        forwardButton. text = forwardKey;
        backwardButton.text = backwardKey;
        leftButton.    text = leftKey;
        rightButton.   text = rightKey;

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

        if (e.isKey && lookingForKey)
        {
            keyHit = e.keyCode.ToString();
            switch (objectName)
            {
                case "ForwardText":
                    forwardButton.text  = keyHit;
                    forwardKey          = forwardButton.text;
                    break;

                case "BackwardText":
                    backwardButton.text = keyHit;
                    backwardKey         = backwardButton.text;
                    break;

                case "LeftText":
                    leftButton.text     = keyHit;
                    leftKey             = leftButton.text;
                    break;

                case "RightText":
                    rightButton.text    = keyHit;
                    rightKey            = rightButton.text;
                    break;
            }
            lookingForKey = false;
        }

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
        }
    }
}