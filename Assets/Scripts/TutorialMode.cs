/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       02/12/20                                         */
/* Last Modified Date: 03/03/20                                         */
/* Modified By:        J. Calas                                         */
/************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialMode : MonoBehaviour
{
    #region Symbolic Constants
    public const int step1 = 1;
    public const int step2 = 2;
    public const int step3 = 3;
    public const int step4 = 4;
    public const int step5 = 5;
    public const int step6 = 6;
    public const int step7 = 7;
    public const int step8 = 8;
    #endregion

    #region Variables
    public static bool TutorialModeOn = false; // Affects CameraMovement, TurretRotation, and PlayerController, respectively.
    //public static bool ActionRequired = false;
    //public static bool TaskIsComplete = false;
    bool readyForNextStep = false;

    public static int tutorialStep = 1;
    public static float delayTime = 2.0f;

    public GameObject PlayerUI;
    public GameObject TutorialUI;

    private GameObject wall;
    private GameObject wall2;
    private GameObject panel;
    private GameObject block;

    public TextMeshProUGUI headingText;
    public TextMeshProUGUI subtitleText;
    public TextMeshProUGUI promptText;

    private Player player;
    private bool firstCall = true;
    private bool waitForPanel = false;
    private string sceneName;
    private int lastStep = step8;

    //public  RectTransform   panel;
    #endregion

    void Awake()
    {
        #region Initialize variables
        player = GameObject.FindGameObjectWithTag("PlayerClass").GetComponent<Player>();
        wall = GameObject.Find("FirstWall");
        wall2 = GameObject.Find("SecondWall");
        panel = GameObject.Find("Panel");
        block = GameObject.Find("Block");

        promptText.text = string.Format("Press {0} to continue.", KeyCode.Space.ToString().ToLower());
        #endregion

        #region Changes GameUI to TutorialUI
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (sceneName == "Tutorial")
        {
            PlayerUI.SetActive(false);
            TutorialUI.SetActive(true);
            TutorialModeOn = true;
        }
        #endregion
    }

    void Update()
    {
        //Debug.Log(tutorialStep);

        #region Moves tutorial to the next slide on GameUI
        if (sceneName == "Tutorial")
        {
            GuideText();
        }



        //if (readyForNextStep)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space)) ; // && (!waitForPanel))
        //    {
                
        //    }
        //}
        //else
        //{
        //    promptText.gameObject.SetActive(false);
        //}



        //if (readyForNextStep)
        //{
        //    Invoke("NextStep", 1.5f);
        //    readyForNextStep = false;
        //}
        //else
        //{
        //    promptText.gameObject.SetActive(false);
        //}

        #endregion
    }

    void GuideText()
    {
        switch (tutorialStep)
        {
            #region Step 1
            case step1:
                headingText.text = "Welcome to the training camp!";
                subtitleText.text = "Here you will learn the basic skills required in battle.";
                break;
            #endregion

            #region Step 2
            case step2:
                headingText.text = "Use your mouse wheel to zoom in or out.";
                subtitleText.text = "";
                break;
            #endregion

            #region Step 3
            case step3:
                headingText.text = "Use your mouse to control the camera.";
                subtitleText.text = "The turret follows the camera as you move it.";
                break;
            #endregion

            #region Step 4
            case step4:
                headingText.text = string.Format(
                                     "Use the {0}, {1}, {2}, and {3} keys to control your vehicle.",
                                        KeyBindings.forwardKey, KeyBindings.leftKey,
                                        KeyBindings.backwardKey, KeyBindings.rightKey);
                subtitleText.text = "";
                break;
            #endregion

            #region Step 5
            case step5:
                // Debug.Log("Working!");
                wall.LeanMoveLocalY(-5, 1.5f);
                waitForPanel = true;

                headingText.text = "Move to the designated location.";
                subtitleText.text = "";
                break;
            #endregion

            #region Step 6
            case step6:
                headingText.text = "!";
                subtitleText.text = ".";
                break;
            #endregion

            #region Step 7
            case step7:
                wall2.LeanMoveLocalY(-5, 1.5f);

                headingText.text = "Aim at the [target] and {click} to fire.";
                subtitleText.text = "";
                break;
            #endregion

            #region Step 8
            case step8:
                headingText.text = "CONGRATULATIONS!";
                subtitleText.text = "You have successfully completed the tutorial.";
                break;
                #endregion
        }

        if (tutorialStep > lastStep && firstCall)
        {
            player.leaveGame = true;
            firstCall = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        waitForPanel = false;
        tutorialStep++;
    }

    void NextStep()
    {
        promptText.gameObject.SetActive(true);
        tutorialStep++;
    }
}