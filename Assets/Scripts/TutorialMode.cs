/************************************************************************/
/* Author:             Jaben Calas                                      */
/* Date Created:       02/12/20                                         */
/* Last Modified Date: 02/28/20                                         */
/* Modified By:        Jaben Calas                                      */
/************************************************************************/
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialMode : MonoBehaviour
{
    #region Variables
    public  static bool     TutorialModeOn  = false; // Affects CameraMovement, TurretRotation, and PlayerController, respectively.
    public  static bool     ActionRequired  = false;
    public  static bool     TaskIsComplete  = false;
            
    public  static int      tutorialStep;
    public  static float    delayTime       = 2.0f;

    public  GameObject      PlayerUI;
    public  GameObject      TutorialUI;

    public  TextMeshProUGUI headingText;
    public  TextMeshProUGUI subtitleText;
    public  TextMeshProUGUI promptText;

    private string          sceneName;
    #endregion

    void Start()
    {
        tutorialStep = 1;

        #region Check for Tutorial Scene
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (sceneName == "Tutorial")
        {
            PlayerUI.SetActive(false);
            TutorialUI.SetActive(true);
            TutorialModeOn = true;
        }
        #endregion

        promptText.text = string.Format("Press {0} to continue.", KeyCode.Space.ToString().ToLower());
    }

    void Update()
    {
        if (sceneName == "Tutorial")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                tutorialStep++;
            }

            GuideText();
        }
    }

    void GuideText()
    {
        switch (tutorialStep)
        {
            #region Step 1
            case 1:
                headingText.text  = "Welcome to the training camp!";
                subtitleText.text = "Here you will learn the basic skills required in battle.";
                break;
            #endregion

            #region Step 2
            case 2:
                headingText.text  = "Use your mouse wheel to zoom in or out.";
                subtitleText.text = "";
                break;
            #endregion

            #region Step 3
            case 3:
                headingText.text  = "Use your mouse to control the camera.";
                subtitleText.text = "The turret follows the camera as you move it.";
                break;
            #endregion

            #region Step 4
            case 4:
                headingText.text  = string.Format(
                                     "Use the {0}, {1}, {2}, and {3} keys to control your vehicle.",
                                        KeyBindings.forwardKey,  KeyBindings.leftKey, 
                                        KeyBindings.backwardKey, KeyBindings.rightKey);
                subtitleText.text = "";
                break;
            #endregion

            #region Step 5
            case 5:
                headingText.text  = "Move to the designated location.";
                subtitleText.text = "";
                break;
            #endregion

            #region Step 6
            case 6:
                headingText.text  = "Aim at the [target] and {click} to fire.";
                subtitleText.text = "";
                break;
            #endregion

            #region Step 7
            case 7:
                headingText.text  = "CONGRATULATIONS!";
                subtitleText.text = "You have successfully completed the tutorial.";
                break;
            #endregion

            #region Step 8
            case 8:
                StartCoroutine(DisconnectAndLoad());
                break;
            #endregion
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject);
    }

    private IEnumerator DisconnectAndLoad()
    {
        TutorialModeOn = false;
        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.UnloadSceneAsync(1);
        PhotonNetwork.LoadLevel(0);
    }
}