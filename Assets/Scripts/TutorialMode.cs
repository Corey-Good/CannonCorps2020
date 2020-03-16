/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       02/12/20                                         */
/* Last Modified Date: 03/08/20                                         */
/* Modified By:        J. Calas                                         */
/************************************************************************/

using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMode : MonoBehaviour
{
    #region Symbolic Constants

    public const int step1 = 00;
    public const int step2 = 05;
    public const int step3 = 10;
    public const int step4 = 15;
    public const int step5 = 60;
    public const int step6 = 120;
    public const int step7 = 140;

    #endregion Symbolic Constants

    #region Variables

    // Affects CameraMovement, TurretRotation, and PlayerController
    public static int currentStep;

    public static bool tutorialModeOn;

    public GameObject PlayerUI;
    public GameObject TutorialUI;

    private GameObject wall;
    private GameObject wall2;
    private GameObject panel;
    private GameObject block;

    public TextMeshProUGUI headingText;
    public TextMeshProUGUI subtitleText;

    private Player player;
    private int lastStep;
    private bool firstCall;
    private string sceneName;

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
        }

        #endregion Changes GameUI to TutorialUI

        #region Initialize variables

        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        wall = GameObject.Find("Checkpoint (1)");
        wall2 = GameObject.Find("Checkpoint (2)");
        panel = GameObject.Find("Panel");
        block = GameObject.Find("Block");

        currentStep = step1;
        lastStep = step7;

        firstCall = true;
        tutorialModeOn = false;

        gameTimer = 0;
        startTime = (int)PhotonNetwork.Time;

        #endregion Initialize variables
    }

    private void FixedUpdate()
    {
        #region Debug

        if (Input.GetKeyDown(KeyCode.Space))
        {
            startTime += -5;
        }

        #endregion Debug

        #region Handles GameTimer

        gameTimer = (int)(PhotonNetwork.Time - startTime);

        currentStep = gameTimer;

        #endregion Handles GameTimer

        #region Moves tutorial to the next slide on GameUI

        if (sceneName == "Tutorial")
        {
            TutorialUIText();
        }

        #endregion Moves tutorial to the next slide on GameUI
    }

    private void TutorialUIText()
    {
        switch (currentStep)
        {
            #region Step 1

            case step1:
                headingText.text = "Welcome to the training camp!";
                subtitleText.text = "Here you will learn the basic skills required in battle.";
                break;

            #endregion Step 1

            #region Step 2

            case step2:
                headingText.text = "Use your mouse to control the camera.";
                subtitleText.text = "The turret follows the camera as you move it.";
                break;

            #endregion Step 2

            #region Step 3

            case step3:
                headingText.text = string.Format(
                                     "Use the {0}, {1}, {2}, and {3} keys to control your vehicle.",
                                        KeyBindings.forwardKey, KeyBindings.leftKey,
                                        KeyBindings.backwardKey, KeyBindings.rightKey);
                subtitleText.text = "";
                break;

            #endregion Step 3

            #region Step 4

            case step4:
                wall.LeanMoveLocalY(-5, 1.5f);
                headingText.text = "Move to the designated location.";
                subtitleText.text = "";
                break;

                #endregion Step 4
        }

        #region Step 5

        if (gameTimer >= step5)
        {
            wall2.LeanMoveLocalY(-5, 1.5f);
            headingText.text = "Aim at the block and click to fire.";
            subtitleText.text = "";
        }

        #endregion Step 5

        #region Step 6

        if (gameTimer >= step6)
        {
            headingText.text = "CONGRATULATIONS!";
            subtitleText.text = "You have successfully completed the tutorial.";
        }

        #endregion Step 6

        #region Moves player to MainMenu after completing tutorial

        if (currentStep > lastStep && firstCall)
        {
            player.leaveGame = true;
            firstCall = false;
        }

        #endregion Moves player to MainMenu after completing tutorial
    }
}