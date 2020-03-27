/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       02/12/20                                         */
/* Last Modified Date: 03/25/20                                         */
/* Modified By:        J. Calas                                         */
/************************************************************************/

using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMode : MonoBehaviour
{
    #region Variables

    // Affects CameraMovement, TurretRotation, and PlayerController
    public static bool tutorialModeOn;

    public static bool CameraIsEnabled   = true;
    public static bool MovementIsEnabled = true;
    public static bool FiringIsEnabled   = true;

    public static bool step1;
    public static bool step2;
    public static bool step3;
    public static bool step4;
    public static bool step5;
    public static bool step6;

    public GameObject PlayerUI;
    public GameObject TutorialUI;

    private GameObject wall;
    private GameObject wall2;
    private GameObject panel;
    private GameObject block;

    public TextMeshProUGUI headingText;
    public TextMeshProUGUI subtitleText;

    private Player player;
    private bool firstCall;
    private string sceneName;
    private string mouseClick;

    public static int startTime;
    public static int gameTimer;

    #endregion Variables

    private void Awake()
    {
        #region Changes GameUI to TutorialUI

        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (sceneName == "Tutorial")
        {
            PlayerUI.SetActive(false);
            TutorialUI.SetActive(true);
            tutorialModeOn = true;
            #region Initializes variables

            CameraIsEnabled = false;
            MovementIsEnabled = false;
            FiringIsEnabled = false;

            step1 = true;
            step2 = false;
            step3 = false;
            step4 = false;
            step5 = false;
            step6 = false;

            player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
            wall = GameObject.Find("Checkpoint (1)");
            wall2 = GameObject.Find("Checkpoint (2)");
            panel = GameObject.Find("Panel");
            block = GameObject.Find("Block");

            firstCall = true;
            tutorialModeOn = false;

            gameTimer = 0;
            startTime = (int)PhotonNetwork.Time;

            if (KeyBindings.clickIndex == 0)
            {
                mouseClick = "Left Click";
            }
            else if (KeyBindings.clickIndex == 1)
            {
                mouseClick = "Right Click";
            }

            #endregion Initialize variables
        }
        else
        {
            Destroy(this);
        }

        #endregion Changes GameUI to TutorialUI

       
    }

    private void FixedUpdate()
    {
        #region Debug

        //Debug.Log(gameTimer);

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    CameraIsEnabled = true;
        //    MovementIsEnabled = true;
        //    FiringIsEnabled = true;

        //    startTime -= 11;
        //}

        #endregion Debug

        #region Handles GameTimer

        gameTimer = (int)(PhotonNetwork.Time - startTime);

        #endregion Handles GameTimer

        #region Moves tutorial to the next slide on GameUI

        if (sceneName == "Tutorial")
        {
            TutorialUIText();
        }

        #endregion Moves tutorial to the next slide on GameUI

        #region Moves player to MainMenu after completing tutorial

        if (firstCall && step6)
        {
            Invoke("exitTutorial", 3);
            firstCall = false;
        }

        #endregion Moves player to MainMenu after completing tutorial
    }

    private void TutorialUIText()
    {
        #region Automates Step 1-3
        switch (gameTimer)
        {
            case 5:
                step1 = false;
                step2 = true;
                break;

            case 10:
                step2 = false;
                step3 = true;
                break;

            case 15:
                step3 = false;
                step4 = true;
                break;

            default:
                break;
        }
        #endregion

        //Welcome Message
        #region Step 1

        if (step1)
        {
            headingText.text = "Welcome to the training camp!";
            subtitleText.text = "Here you will learn the basic skills required in battle.";
        }

        #endregion Step 1

        //Camera Control
        #region Step 2

        if (step2)
        {
            CameraIsEnabled = true;

            headingText.text = "Use your mouse to control the camera.";
            subtitleText.text = "The turret follows the camera as you move it.";
        }

        #endregion Step 2

        //Movement Control
        #region Step 3

        if (step3)
        {
            MovementIsEnabled = true;

            headingText.text = string.Format(
                                 "Use the {0}, {1}, {2}, and {3} keys to control your vehicle.",
                                    KeyBindings.forwardKey, KeyBindings.leftKey,
                                    KeyBindings.backwardKey, KeyBindings.rightKey);
            subtitleText.text = "";
        }

        #endregion Step 3
        
        //Move to Designated Area
        #region Step 4

        if (step4)
        {
            wall.LeanMoveLocalY(-5, 2f);

            headingText.text = "Move to the designated location.";
            subtitleText.text = "";
        }

        #endregion Step 4

        //Firing Control
        #region Step 5

        if (step5)
        {
            FiringIsEnabled = true;

            wall2.LeanMoveLocalY(-5, 2f);
            headingText.text = string.Format("Aim at the designated target and {0} to fire.", mouseClick);
            subtitleText.text = "";
        }

        #endregion Step 5

        //Finish
        #region Step 6

        if (step6)
        {
            headingText.text = "CONGRATULATIONS!";
            subtitleText.text = "You have successfully completed the tutorial.";
        }

        #endregion Step 6
    }

    public void exitTutorial()
    {
        player.leaveGame = true;
        TutorialUI.SetActive(false);
        //PlayerUI.SetActive(true);
        CameraIsEnabled = true;
        MovementIsEnabled = true;
        FiringIsEnabled = true;
    }
}