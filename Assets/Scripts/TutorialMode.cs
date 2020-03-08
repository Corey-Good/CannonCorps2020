/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       02/12/20                                         */
/* Last Modified Date: 03/03/20                                         */
/* Modified By:        J. Calas                                         */
/************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Photon.Pun;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialMode : MonoBehaviour
{
    #region Symbolic Constants
    public const int step1 = 10;
    public const int step2 = 20;
    public const int step3 = 30;
    public const int step4 = 40;
    public const int step5 = 50;
    public const int step6 = 60;
    public const int step7 = 70;
    public const int step8 = 80;
    #endregion

    #region Variables
    // Affects CameraMovement, TurretRotation, and PlayerController
    public  static int      currentStep;

    public  static bool     tutorialModeOn;
            
    public  GameObject      PlayerUI;
    public  GameObject      TutorialUI;

    private GameObject      wall;
    private GameObject      wall2;
    private GameObject      panel;
    private GameObject      block;

    public  TextMeshProUGUI headingText;
    public  TextMeshProUGUI subtitleText;
    public  TextMeshProUGUI promptText;

    private Player          player;
    private int             lastStep;
    private float           delayTime = 5.0f;
    private bool            firstCall      = true;
    private string          sceneName;
    #endregion

    #region Game Timer
    public static double matchTimer = 0;
    private int minute;
    private int second;
    private double startTime;
    private double matchLength = 80;
    private double timer;
    #endregion

    void Awake()
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
        #endregion

        #region Initialize variables
        player          = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        wall            = GameObject.Find("FirstWall");
        wall2           = GameObject.Find("SecondWall");
        panel           = GameObject.Find("Panel");
        block           = GameObject.Find("Block");
                        
        currentStep     = step1;
        lastStep        = step8;

        tutorialModeOn = false;

        promptText.gameObject.SetActive(false);
        promptText.text = string.Format("Press {0} to continue.", KeyCode.Space.ToString().ToLower());
        #endregion

        SetTimer();
        matchTimer = 0;
    }

    void FixedUpdate()
    {
        UpdateTimer();

        Debug.Log(timer);

        #region Moves tutorial to the next slide on GameUI
        if (sceneName == "Tutorial")
        {
            TutorialUIText();
        }

        currentStep = (int)matchLength;
        #endregion
    }

    void TutorialUIText()
    {
        switch (currentStep)
        {
            #region Step 1
            case step1:
                headingText.text  = "Welcome to the training camp!";
                subtitleText.text = "Here you will learn the basic skills required in battle.";
                break;
            #endregion

            #region Step 2
            case step2:
                headingText.text  = "Use your mouse to control the camera.";
                subtitleText.text = "The turret follows the camera as you move it.";
                break;
            #endregion

            #region Step 3
            case step3:
                headingText.text  = string.Format(
                                     "Use the {0}, {1}, {2}, and {3} keys to control your vehicle.",
                                        KeyBindings.forwardKey, KeyBindings.leftKey,
                                        KeyBindings.backwardKey, KeyBindings.rightKey);
                subtitleText.text = "";
                break;
            #endregion

            #region Step 4
            case step4:
                wall.LeanMoveLocalY(-5, 1.5f);
                headingText.text  = "Move to the designated location.";
                subtitleText.text = "";
                break;
            #endregion

            #region Step 5
            case step5:
                headingText.text  = "5";
                subtitleText.text = "5";
                break;
            #endregion

            #region Step 6
            case step6:
                wall2.LeanMoveLocalY(-5, 1.5f);
                headingText.text  = "Aim at the [target] and {click} to fire.";
                subtitleText.text = "";
                break;
            #endregion

            #region Step 7
            case step7:
                headingText.text  = "7";
                subtitleText.text = "7";
                break;
            #endregion

            #region Step 8
            case step8:
                headingText.text  = "CONGRATULATIONS!";
                subtitleText.text = "You have successfully completed the tutorial.";
                break;
                #endregion
        }

        #region Moves player to MainMenu after completing tutorial
        if (currentStep > lastStep && firstCall)
        {
            player.leaveGame = true;
            firstCall = false;
        }
        #endregion
    }

    void OnTriggerEnter(Collider other)
    {
        currentStep = 4;
    }

    IEnumerator NextStep()
    {
        yield return new WaitForSeconds(delayTime);
        currentStep = 2;
    }

    public void SetTimer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startTime = PhotonNetwork.Time;
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "StartTime", startTime } });
        }
        else
        {
            try
            {
                startTime = (double)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
            }
            catch
            {
                startTime = PhotonNetwork.Time;
            }
        }
    }

    public void UpdateTimer()
    {
        matchTimer = PhotonNetwork.Time - startTime;
        timer = matchLength + matchTimer;
        second = (int)(timer % 60.0f);
        minute = (int)(timer / 60.0f);
    }
}